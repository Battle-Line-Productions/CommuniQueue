<template>
  <div class="project-management p-6">
    <div class="mb-6">
      <h1 class="text-2xl font-bold text-light-textbase dark:text-dark-textbase">
        Project Management
      </h1>
    </div>

    <TabNavigation
      v-model="activeTab"
      :tabs="tabs"
    />

    <div class="mt-6">
      <DashboardProjectDetails
        v-if="activeTab === 'general'"
        :project="project!"
      />
      <DashboardAccess
        v-else-if="activeTab === 'access'"
        :project="project!"
      />
      <!-- <ProjectApiKeys
        v-else-if="activeTab === 'api'"
        :project="project"
      /> -->
    </div>
    <ToastNotification ref="toastRef" />
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useQuery } from '@tanstack/vue-query'
import useProjects from '~/composables/use-projects-service'

const route = useRoute()
const projectId = route.params.id as string
const { getProjectById } = useProjects()
const { toastRef } = useToast()

const tabs = [
  { id: 'general', label: 'General' },
  { id: 'access', label: 'Access Control' },
  { id: 'api', label: 'API Keys' },
]

const activeTab = ref('general')

const { data: projectData } = useQuery({
  queryKey: ['project', projectId],
  queryFn: () => getProjectById(projectId),
})

const project = computed(() => projectData.value?.data)

// const handleProjectUpdated = () => {
//   showToast({
//     type: 'success',
//     title: 'Project Updated',
//     message: 'Project details have been successfully updated.'
//   });
// };

// const handleUserAdded = () => {
//   showToast({
//     type: 'success',
//     title: 'Team Member Added',
//     message: 'New team member has been successfully added to the project.'
//   });
// };

// const handleUserRemoved = () => {
//   showToast({
//     type: 'info',
//     title: 'Team Member Removed',
//     message: 'Team member has been removed from the project.'
//   });
// };

// const handleKeyGenerated = () => {
//   showToast({
//     type: 'success',
//     title: 'API Key Generated',
//     message: 'New API key has been successfully generated.'
//   });
// };

// const handleKeyExpired = () => {
//   showToast({
//     type: 'info',
//     title: 'API Key Expired',
//     message: 'The API key has been expired successfully.'
//   });
// };
</script>
