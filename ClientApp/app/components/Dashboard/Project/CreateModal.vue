<template>
  <div class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center">
    <div class="bg-light-surface dark:bg-dark-surface p-6 rounded-lg w-full max-w-md">
      <h2 class="text-xl font-semibold mb-4 text-light-textbase dark:text-dark-textbase">
        Create New Project
      </h2>
      <form @submit.prevent="handleSubmit">
        <div class="mb-4">
          <label
            for="projectName"
            class="block text-sm font-medium text-light-textbase dark:text-dark-textbase mb-1"
          >Project Name</label>
          <input
            id="projectName"
            v-model="projectName"
            type="text"
            required
            class="w-full px-3 py-2 border rounded-md text-light-textbase dark:text-dark-textbase bg-light-background dark:bg-dark-background"
          >
        </div>
        <div class="mb-4">
          <label
            for="projectDescription"
            class="block text-sm font-medium text-light-textbase dark:text-dark-textbase mb-1"
          >Description</label>
          <textarea
            id="projectDescription"
            v-model="projectDescription"
            rows="3"
            class="w-full px-3 py-2 border rounded-md text-light-textbase dark:text-dark-textbase bg-light-background dark:bg-dark-background"
          />
        </div>
        <div class="flex justify-end">
          <UButton
            type="button"
            class="mr-2 px-4 py-2 text-light-secondary dark:text-dark-secondary hover:underline"
            @click="$emit('close')"
          >
            Cancel
          </UButton>
          <UButton
            type="submit"
            :disabled="isPending"
            class="bg-light-primary dark:bg-dark-primary text-white px-4 py-2 rounded-md hover:opacity-90 transition-opacity disabled:opacity-50"
          >
            {{ isPending ? 'Creating...' : 'Create Project' }}
          </UButton>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useMutation, useQueryClient } from '@tanstack/vue-query'
import useProjects from '~/composables/use-projects-service'
import type { ICreateProjectRequest } from '~/types'

const emit = defineEmits(['close'])
const { createProject } = useProjects()
const queryClient = useQueryClient()

const projectName = ref('')
const projectDescription = ref('')

const { mutate, isPending } = useMutation({
  mutationFn: async (newProject: ICreateProjectRequest) => {
    const result = await createProject(newProject)
    if (result.data) {
      return result.data
    }
    throw new Error('Failed to create project')
  },
  onSuccess: () => {
    queryClient.invalidateQueries({ queryKey: ['projects'] })
    emit('close')
  },
  onError: (error: Error) => {
    console.error('Failed to create project:', error)
    // Add user-facing error handling here
  },
})

const handleSubmit = () => {
  mutate({
    name: projectName.value,
    description: projectDescription.value,
    ownerId: '7F58AFB9-CFEB-4117-998A-A6658C3BCC90', // TODO: Replace with actual user ID from auth system
  })
}
</script>
