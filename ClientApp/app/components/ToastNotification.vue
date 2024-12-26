<script setup lang="ts">
interface Toast {
  id: number
  type: 'success' | 'error' | 'info' | 'warning'
  title: string
  message?: string
  timeout: number
}

const toasts = ref<Toast[]>([])
const timeouts = new Map<number, NodeJS.Timeout>()

const toastTypeClasses = {
  success: 'bg-light-success/10 text-light-success dark:bg-dark-success/10 dark:text-dark-success',
  error: 'bg-light-error/10 text-light-error dark:bg-dark-error/10 dark:text-dark-error',
  info: 'bg-light-info/10 text-light-info dark:bg-dark-info/10 dark:text-dark-info',
  warning: 'bg-light-warning/10 text-light-warning dark:bg-dark-warning/10 dark:text-dark-warning',
}

const toastTypeIcons = {
  success: 'mdi:check-circle',
  error: 'mdi:alert-circle',
  info: 'mdi:information',
  warning: 'mdi:alert',
}

const addToast = (toast: Omit<Toast, 'id' | 'timeout'>) => {
  const id = Date.now()
  const newToast = {
    ...toast,
    id,
    timeout: 5000,
  }

  toasts.value.push(newToast)
  startTimeout(newToast)
}

const removeToast = (id: number) => {
  const timeout = timeouts.get(id)
  if (timeout) {
    clearTimeout(timeout)
    timeouts.delete(id)
  }
  toasts.value = toasts.value.filter(t => t.id !== id)
}

const startTimeout = (toast: Toast) => {
  const timeout = setTimeout(() => {
    removeToast(toast.id)
  }, toast.timeout)
  timeouts.set(toast.id, timeout)
}

const pauseTimeout = (id: number) => {
  const timeout = timeouts.get(id)
  if (timeout) {
    clearTimeout(timeout)
  }
}

const resumeTimeout = () => {
  toasts.value.forEach(startTimeout)
}

onBeforeUnmount(() => {
  timeouts.forEach(clearTimeout)
})

// Expose the addToast method to be used by other components
defineExpose({ addToast })
</script>

<template>
  <div
    class="fixed bottom-4 right-4 z-50 space-y-2"
    @mouseleave="resumeTimeout"
  >
    <TransitionGroup name="toast">
      <div
        v-for="toast in toasts"
        :key="toast.id"
        class="flex items-center p-4 rounded-lg shadow-lg max-w-md transform transition-all duration-300"
        :class="toastTypeClasses[toast.type]"
        @mouseenter="pauseTimeout(toast.id)"
      >
        <Icon
          :name="toastTypeIcons[toast.type]"
          class="w-5 h-5 mr-3 flex-shrink-0"
        />
        <div class="flex-1">
          <div class="font-medium">
            {{ toast.title }}
          </div>
          <div
            v-if="toast.message"
            class="text-sm mt-1"
          >
            {{ toast.message }}
          </div>
        </div>
        <button
          class="ml-4 hover:opacity-80"
          @click="removeToast(toast.id)"
        >
          <Icon
            name="mdi:close"
            class="w-5 h-5"
          />
        </button>
      </div>
    </TransitionGroup>
  </div>
</template>

<style scoped>
.toast-enter-active,
.toast-leave-active {
  transition: all 0.3s ease;
}

.toast-enter-from {
  opacity: 0;
  transform: translateX(100%);
}

.toast-leave-to {
  opacity: 0;
  transform: translateX(100%);
}
</style>
