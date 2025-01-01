<template>
  <div class="bg-light-surface dark:bg-dark-surface p-6 rounded-lg shadow-xs">
    <h2 class="text-xl font-semibold mb-4 text-light-textbase dark:text-dark-textbase">
      Project Statistics
    </h2>

    <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
      <!-- <StatCard :value="project.templates?.length || 0" label="Templates" icon="mdi:file-document-outline" /> -->
      <StatCard
        :value="0"
        label="Templates"
        icon="mdi:file-document-outline"
      />
      <StatCard
        :value="teamMembersCount"
        label="Team Members"
        icon="mdi:account-group-outline"
      />
      <StatCard
        :value="activeApiKeysCount"
        label="Active API Keys"
        icon="mdi:key-outline"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
// import type { IProject } from '~/types';

// defineProps<{
//   project: IProject;
// }>();

const teamMembersCount = computed(() => {
  // return props.project.permissions?.length || 0;
  return 0
})

const activeApiKeysCount = computed(() => {
  // return props.project.apiKeys?.filter((key) => !key.isExpired).length || 0;
  return 0
})
</script>
