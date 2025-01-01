<template>
  <div
    class="project-card bg-light-surface dark:bg-dark-surface p-4 rounded-lg shadow-md hover:shadow-lg transition-shadow relative"
  >
    <UButton
      class="absolute top-2 right-2 text-light-secondary dark:text-dark-secondary hover:text-light-error dark:hover:text-dark-error transition-colors z-10"
      aria-label="Delete Project"
      @click="openDeleteConfirmation"
    >
      <Icon
        name="mdi:trash-can-outline"
        class="w-5 h-5"
      />
    </UButton>

    <div class="flex justify-between items-center mb-2 pr-8">
      <h3 class="text-lg font-semibold text-light-textbase dark:text-dark-textbase">
        {{ project.name }}
      </h3>
      <span class="text-xs text-light-secondary dark:text-dark-secondary"> Updated: {{
        localTime.toLocalTime(project.updatedDateTime) }} </span>
    </div>
    <p class="text-sm text-light-secondary dark:text-dark-secondary mb-4 line-clamp-2">
      {{ project.description || 'No description provided' }}
    </p>

    <div class="grid grid-cols-3 gap-2 mb-4 text-center">
      <div class="bg-light-background dark:bg-dark-background rounded-sm p-2">
        <p class="text-xs text-light-secondary dark:text-dark-secondary">
          Templates
        </p>
        <p class="text-sm font-semibold">
          {{ projectKpis.templateCount }}
        </p>
      </div>
      <div class="bg-light-background dark:bg-dark-background rounded-sm p-2">
        <p class="text-xs text-light-secondary dark:text-dark-secondary">
          Containers
        </p>
        <p class="text-sm font-semibold">
          {{ projectKpis.containerCount }}
        </p>
      </div>
      <div class="bg-light-background dark:bg-dark-background rounded-sm p-2">
        <p class="text-xs text-light-secondary dark:text-dark-secondary">
          Stages
        </p>
        <p class="text-sm font-semibold">
          {{ projectKpis.stageCount }}
        </p>
      </div>
    </div>

    <div class="flex justify-between items-center">
      <div class="flex space-x-2">
        <NuxtLink
          :to="`/dashboard/projects/${project.id}/manage`"
          class="bg-light-secondary dark:bg-dark-secondary text-white px-3 py-2 rounded-md text-sm hover:opacity-90 transition-opacity"
        >
          Manage Project
        </NuxtLink>
      </div>

      <NuxtLink
        :to="`/dashboard/projects/${project.id}/templates`"
        class="text-light-accent dark:text-dark-accent text-sm font-semibold hover:underline flex items-center"
      >
        Manage Templates
        <Icon
          name="mdi:chevron-right"
          class="w-4 h-4"
        />
      </NuxtLink>
    </div>

    <ConfirmationModal
      v-if="showDeleteConfirmation"
      title="Confirm Project Deletion"
      :message="`Are you sure you want to delete the project '${props.project.name}'? This action cannot be undone. All Containers and related Templates will also be deleted!`"
      :is-processing="isDeleting"
      processing-text="Deleting..."
      @cancel="showDeleteConfirmation = false"
      @confirm="handleDelete"
    />
  </div>
</template>

<script setup lang="ts">
import { useMutation, useQueryClient, useQuery } from '@tanstack/vue-query'
import useProjects from '~/composables/use-projects-service'
import type { IApiResponse, IProject, IProjectKpis } from '~/types'

const props = defineProps<{
  project: IProject
}>()

const showDeleteConfirmation = ref(false)
const isDeleting = ref(false)

const { deleteProject: deleteProjectService, getProjectKpis } = useProjects()
const queryClient = useQueryClient()
const localTime = useLocalTime()

const {
  data: kpisData,
  isLoading: _isKpisLoading,
  isError: _isKpisError,
  error: _kpisError,
} = useQuery<IApiResponse<IProjectKpis>>({
  queryKey: ['projectKpis', props.project.id],
  queryFn: () => getProjectKpis(props.project.id),
  placeholderData: () => ({
    data: {
      templateCount: props.project.templates.length,
      containerCount: props.project.containers.length,
      stageCount: props.project.stages.length,
    },
    isSuccess: true,
    status: 200,
    errors: [],
    message: 'Project KPIs not found',
  }),
})

const projectKpis = computed(() => {
  const defaultKpis: IProjectKpis = {
    templateCount: props.project.templates.length,
    containerCount: props.project.containers.length,
    stageCount: props.project.stages.length,
  }

  if (!kpisData.value) return defaultKpis

  const apiResponse = kpisData.value as unknown as IApiResponse<IProjectKpis>
  return apiResponse.data || defaultKpis
})

const openDeleteConfirmation = () => {
  showDeleteConfirmation.value = true
}

const { mutate: deleteProject } = useMutation({
  mutationFn: async () => {
    // Ensure isDeleting is set to true only when the mutation starts
    return await deleteProjectService(props.project.id)
  },
  onMutate: () => {
    // Set isDeleting to true when mutation starts
    isDeleting.value = true
  },
  onSuccess: () => {
    // Invalidate and refetch project list
    queryClient.invalidateQueries({ queryKey: ['projects'] })
    showDeleteConfirmation.value = false
  },
  onError: (error) => {
    console.error('Failed to delete project:', error)
    isDeleting.value = false
    showDeleteConfirmation.value = false
  },
  onSettled: () => {
    // Ensure isDeleting is always set to false, regardless of success or failure
    isDeleting.value = false
    showDeleteConfirmation.value = false
  },
})

const handleDelete = () => {
  deleteProject()
}
</script>
