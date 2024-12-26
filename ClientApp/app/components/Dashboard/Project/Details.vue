<template>
  <div class="space-y-6">
    <div class="bg-light-surface dark:bg-dark-surface p-6 rounded-lg shadow-sm">
      <h2 class="text-xl font-semibold mb-4 text-light-textbase dark:text-dark-textbase"> Project Details </h2>

      <div v-if="!project" class="py-8 text-center text-light-secondary dark:text-dark-secondary">
        <LoadingSpinner v-if="isLoading" />
        <p v-else>Project details not available</p>
      </div>

      <form v-else class="max-w-2xl" @submit.prevent="handleSubmit">
        <div class="space-y-4">
          <FormField v-model="form.name" label="Project Name" placeholder="Enter project name" :error="errors.name" :max-length="100" show-char-count required :disabled="isPending" />

          <FormField
            v-model="form.description"
            label="Description"
            type="textarea"
            placeholder="Describe your project"
            :error="errors.description"
            helper-text="Provide a clear description of your project's purpose and scope"
            :max-length="500"
            show-char-count
            :rows="4"
            :disabled="isPending"
          />

          <div class="flex justify-end space-x-3">
            <button type="button" class="text-light-secondary dark:text-dark-secondary hover:underline px-4 py-2" :disabled="isPending" @click="resetForm"> Reset </button>
            <button
              type="submit"
              :disabled="isPending || !isFormChanged"
              class="bg-light-primary dark:bg-dark-primary text-white px-4 py-2 rounded-md hover:opacity-90 transition-opacity disabled:opacity-50"
            >
              {{ isPending ? 'Saving...' : 'Save Changes' }}
            </button>
          </div>
        </div>
      </form>
    </div>

    <DashboardProjectStats :project="project!" />
  </div>
</template>

<script setup lang="ts">
import { useMutation, useQueryClient } from '@tanstack/vue-query';
import useProjects from '~/composables/use-projects-service';
import type { IProject, IUpdateProjectRequest } from '~/types';

const props = withDefaults(
  defineProps<{
    project?: IProject;
    isLoading?: boolean;
  }>(),
  {
    isLoading: false
  }
);

const { updateProject } = useProjects();
const queryClient = useQueryClient();
const { showToast } = useToast();

// Form state
const initialForm = reactive({
  name: '',
  description: ''
});

const form = reactive({
  name: '',
  description: ''
});

watch(
  () => props.project,
  (newProject) => {
    if (newProject) {
      initialForm.name = newProject.name;
      initialForm.description = newProject.description || '';

      // Reset form to initial values
      form.name = initialForm.name;
      form.description = initialForm.description;
    }
  },
  { immediate: true }
);

const errors = reactive({
  name: '',
  description: ''
});

const isFormChanged = computed(() => {
  return form.name !== initialForm.name || form.description !== initialForm.description;
});

const validateForm = (): boolean => {
  let isValid = true;
  errors.name = '';
  errors.description = '';

  if (!form.name.trim()) {
    errors.name = 'Project name is required';
    isValid = false;
  } else if (form.name.length < 3) {
    errors.name = 'Project name must be at least 3 characters';
    isValid = false;
  }

  if (form.description && form.description.length < 10) {
    errors.description = 'Description must be at least 10 characters';
    isValid = false;
  }

  return isValid;
};

const { mutate, isPending } = useMutation({
  mutationFn: async (updateData: IUpdateProjectRequest) => {
    if (!props.project) throw new Error('Project not found');
    const result = await updateProject(props.project.id, updateData);
    if (result.data) {
      return result.data;
    }
    throw new Error('Failed to update project');
  },
  onSuccess: (data) => {
    if (!props.project) return;

    // Update the cache
    queryClient.setQueryData(['project', props.project.id], {
      data,
      isSuccess: true,
      status: 200,
      errors: []
    });

    // Update initial form state
    Object.assign(initialForm, {
      name: data.name,
      description: data.description
    });

    showToast({
      type: 'success',
      title: 'Project Updated',
      message: 'Project details have been successfully updated.'
    });
  },
  onError: (error: Error) => {
    console.error(error);
    showToast({
      type: 'error',
      title: 'Update Failed',
      message: error.message || 'Failed to update project details.'
    });
  }
});

// Form submission
const handleSubmit = () => {
  if (!validateForm()) {
    showToast({
      type: 'error',
      title: 'Validation Error',
      message: 'Please check the form for errors.'
    });
    return;
  }

  mutate({
    name: form.name.trim(),
    description: form.description.trim() || ''
  });
};

// Reset form
const resetForm = () => {
  form.name = initialForm.name;
  form.description = initialForm.description;
  errors.name = '';
  errors.description = '';
};
</script>
