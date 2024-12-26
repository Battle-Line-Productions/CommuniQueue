<template>
  <div class="bg-light-surface dark:bg-dark-surface p-4 rounded-lg shadow-md">
    <h2 class="text-lg font-semibold mb-4 text-light-textbase dark:text-dark-textbase">Quick Actions</h2>
    <div class="space-y-2">
      <NuxtLink
        v-for="action in quickActions"
        :key="action.to"
        :to="action.to"
        class="block bg-light-background dark:bg-dark-background p-3 rounded-md hover:bg-light-accent/10 dark:hover:bg-dark-accent/10 transition-colors"
      >
        <div class="flex items-center justify-between">
          <span class="text-light-textbase dark:text-dark-textbase">{{ action.label }}</span>
          <Icon :name="action.icon" class="text-light-secondary dark:text-dark-secondary" />
        </div>
      </NuxtLink>
    </div>
  </div>
</template>

<script setup lang="ts">
interface QuickAction {
  label: string;
  to: string;
  icon: string;
}

const quickActions: QuickAction[] = [
  {
    label: 'Account Settings',
    to: '/accountsettings',
    icon: 'mdi:account-settings'
  },
  {
    label: 'User Management',
    to: '/usermanagement',
    icon: 'mdi:user-group'
  }
];
</script>
