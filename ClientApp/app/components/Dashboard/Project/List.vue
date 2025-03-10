<template>
  <div class="project-list">
    <h2 class="text-xl font-semibold mb-4 text-light-textbase dark:text-dark-textbase">
      Your Projects
    </h2>
    <div
      v-if="isLoading"
      class="text-light-secondary dark:text-dark-secondary"
    >
      Loading projects...
    </div>
    <div
      v-else-if="isError"
      class="text-light-error dark:text-dark-error"
    >
      Error loading projects: {{ error }}
    </div>
    <div
      v-else-if="projects.length === 0"
      class="text-center py-8"
    >
      <p class="text-light-secondary dark:text-dark-secondary mb-4">
        No projects found.
      </p>
      <p class="text-light-secondary dark:text-dark-secondary mb-6">
        Create a project to get started!
      </p>
    </div>
    <div
      v-else
      class="grid grid-cols-1 lg:grid-cols-2 xl:grid-cols-2 gap-4"
    >
      <DashboardProjectCard
        v-for="project in projects"
        :key="project.id"
        :project="project"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { useQuery } from '@tanstack/vue-query'
import type { UserInfoResponse } from '@logto/nuxt'
import useProjects from '~/composables/use-projects-service'
import type { IProject, IApiResponse } from '~/types'

const { getProjectsByUserId } = useProjects()
const user: UserInfoResponse = useLogtoUser()
const { currentTenantId } = useTenant()

// TODO: Replace with actual user ID from auth system
const userId = user.sub

const { isLoading, isError, data, error } = useQuery<IApiResponse<IProject[]>>({
  queryKey: ['projectsPerTenant', userId, currentTenantId.value],
  queryFn: () => getProjectsByUserId(userId),
  placeholderData: () => ({
    data: [] as IProject[],
    isSuccess: true,
    status: 200,
    errors: [],
    message: 'No projects found',
  }),
})

const projects = computed<IProject[]>(() => {
  if (data.value && 'data' in data.value) {
    return (data.value as IApiResponse<IProject[]>).data || []
  }
  return []
})
</script>
