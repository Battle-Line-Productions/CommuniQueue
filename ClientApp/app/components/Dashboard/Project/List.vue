<template>
  <div class="project-list">
    <h2 class="text-xl font-semibold mb-4 text-light-textbase dark:text-dark-textbase">Your Projects</h2>
    <div v-if="isLoading" class="text-light-secondary dark:text-dark-secondary">Loading projects...</div>
    <div v-else-if="isError" class="text-light-error dark:text-dark-error">Error loading projects: {{ error }}</div>
    <div v-else-if="projects.length === 0" class="text-center py-8">
      <p class="text-light-secondary dark:text-dark-secondary mb-4">No projects found.</p>
      <p class="text-light-secondary dark:text-dark-secondary mb-6">Create a project to get started!</p>
    </div>
    <div v-else class="grid grid-cols-1 lg:grid-cols-2 xl:grid-cols-2 gap-4">
      <DashboardProjectCard v-for="project in projects" :key="project.id" :project="project" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useQuery } from '@tanstack/vue-query';
import useProjects from '~/composables/use-projects-service';
import type { IProject, IApiResponse } from '~/types';

const { getProjectsByUserId } = useProjects();

// TODO: Replace with actual user ID from auth system
const userId = '7F58AFB9-CFEB-4117-998A-A6658C3BCC90';

const { isLoading, isError, data, error } = useQuery<IApiResponse<IProject[]>>({
  queryKey: ['projects', userId],
  queryFn: () => getProjectsByUserId(userId),
  placeholderData: () => ({
    data: [] as IProject[],
    isSuccess: true,
    status: 200,
    errors: [],
    message: 'No projects found'
  })
});

const projects = computed<IProject[]>(() => {
  if (data.value && 'data' in data.value) {
    return (data.value as IApiResponse<IProject[]>).data || [];
  }
  return [];
});
</script>
