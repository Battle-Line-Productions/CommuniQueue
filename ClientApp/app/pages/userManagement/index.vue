<!-- pages/userManagement/index.vue -->
<template>
  <UserManagementLayout>
    <template #main-content>
      <UserManagementList />
    </template>

    <template #side-content>
      <div class="p-4 sm:p-6 lg:p-8">
        <!-- Any optional side content or help tips can go here -->
        <p class="text-light-secondary dark:text-dark-secondary"> Manage your users here. Add new members or remove existing ones. </p>
      </div>
    </template>
  </UserManagementLayout>
</template>

<script setup lang="ts">
// No logic needed, just references the layout & child
</script>

<style scoped>
/* Minimal styling or none if you prefer */
</style>
