<script setup lang="ts">
withDefaults(
  defineProps<{
    title: string
    message: string
    confirmText?: string
    cancelText?: string
    processingText?: string
    isProcessing?: boolean
    variant?: 'danger' | 'primary'
  }>(),
  {
    confirmText: 'Confirm',
    cancelText: 'Cancel',
    processingText: 'Processing...',
    isProcessing: false,
    variant: 'primary',
  },
)

defineEmits<{
  cancel: []
  confirm: []
}>()
</script>

<template>
  <div class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
    <div class="bg-light-surface dark:bg-dark-surface p-6 rounded-lg max-w-sm w-full">
      <h2 class="text-lg font-semibold mb-4 text-light-textbase dark:text-dark-textbase">
        {{ title }}
      </h2>
      <p class="text-sm text-light-secondary dark:text-dark-secondary mb-6">
        {{ message }}
      </p>
      <div class="flex justify-end space-x-2">
        <UButton
          class="px-4 py-2 text-light-secondary dark:text-dark-secondary hover:underline"
          :disabled="isProcessing"
          @click="$emit('cancel')"
        >
          {{ cancelText }}
        </UButton>
        <UButton
          :disabled="isProcessing"
          :class="[
            'text-white px-4 py-2 rounded-md hover:opacity-90 transition-opacity disabled:opacity-50',
            variant === 'danger' ? 'bg-light-error dark:bg-dark-error' : 'bg-light-primary dark:bg-dark-primary',
          ]"
          @click="$emit('confirm')"
        >
          {{ isProcessing ? processingText : confirmText }}
        </UButton>
      </div>
    </div>
  </div>
</template>
