<template>
  <div class="dashboard-layout">
    <div class="flex justify-between items-center ml-4 mb-6 mr-4 mt-2">
      <h1 class="text-2xl font-bold text-light-textbase dark:text-dark-textbase">
        Dashboard
      </h1>
      <UButton
        type="button"
        color="primary"
        variant="solid"
        class="px-4 py-2 rounded-md
         text-white
         bg-light-primary dark:bg-dark-primary
         hover:bg-light-primary/90 dark:hover:bg-dark-primary/90
         focus:ring-2 focus:ring-light-primary/50 dark:focus:ring-dark-primary/50
         transition-all duration-300"
        @click.prevent="openCreateProjectModal"
      >
        Create New Project
      </UButton>
    </div>

    <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
      <div class="md:col-span-2 ml-4">
        <DashboardProjectList @create-project="openCreateProjectModal" />
      </div>
      <div class="mr-4">
        <DashboardQuickActions />
      </div>
    </div>

    <DashboardProjectCreateModal
      v-if="showCreateProjectModal"
      @close="closeCreateProjectModal"
    />
  </div>
</template>

<script setup lang="ts">
const showCreateProjectModal = ref(false)

const openCreateProjectModal = () => {
  console.log('Attempting to open modal', showCreateProjectModal.value)
  showCreateProjectModal.value = true
}

const closeCreateProjectModal = () => {
  showCreateProjectModal.value = false
}
</script>
