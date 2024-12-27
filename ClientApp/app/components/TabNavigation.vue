<script setup lang="ts">
interface Tab {
  id: string
  label: string
}

defineProps<{
  modelValue: string
  tabs: Tab[]
}>()

defineEmits<{
  'update:modelValue': [value: string]
}>()
</script>

<template>
  <div class="border-b border-light-secondary/20 dark:border-dark-secondary/20">
    <nav class="flex space-x-8">
      <UButton
        v-for="tab in tabs"
        :key="tab.id"
        class="px-1 py-4 border-b-2 font-medium text-sm transition-colors"
        :class="[
          modelValue === tab.id
            ? 'border-light-primary dark:border-dark-primary text-light-primary dark:text-dark-primary'
            : 'border-transparent text-light-secondary dark:text-dark-secondary hover:text-light-textbase dark:hover:text-dark-textbase',
        ]"
        @click="$emit('update:modelValue', tab.id)"
      >
        {{ tab.label }}
      </UButton>
    </nav>
  </div>
</template>
