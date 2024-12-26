<script setup lang="ts">
defineProps<{
  value: number
  label: string
  icon: string
}>()
</script>

<template>
  <div class="bg-light-background dark:bg-dark-background p-4 rounded-lg">
    <div class="flex items-center justify-between mb-2">
      <Icon
        :name="icon"
        class="w-6 h-6 text-light-primary dark:text-dark-primary"
      />
      <span class="text-2xl font-bold text-light-primary dark:text-dark-primary">{{ value }}</span>
    </div>
    <div class="text-sm text-light-secondary dark:text-dark-secondary">
      {{ label }}
    </div>
  </div>
</template>
