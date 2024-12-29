<template>
  <!-- The container has role="status" so screen readers know it’s a status component -->
  <div
    class="flex items-center justify-center"
    role="status"
    aria-label="Loading"
  >
    <!-- Screen reader-only text for accessibility -->
    <span class="sr-only">Loading...</span>

    <!-- Spinner ring -->
    <div
      class="inline-block w-8 h-8 rounded-full border-4 border-current border-t-transparent
                 text-light-primary dark:text-dark-primary animate-spin"
    />
  </div>
</template>
