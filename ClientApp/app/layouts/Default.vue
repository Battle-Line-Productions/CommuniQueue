<script setup lang="ts">
const currentYear = new Date().getFullYear()

const tenant = useTenant()

// Reactive reference for project route
const projectRoute = ref('/auth/logging-in')

// Watch for changes in currentTenantId
watch(() => tenant.currentTenantId.value, (newTenantId) => {
  // Update projectRoute when currentTenantId changes
  projectRoute.value = newTenantId
    ? `/tenant/${newTenantId}/dashboard/projects`
    : '/auth/logging-in'
}, {
  // Immediate will trigger the watch on initial load
  immediate: true,
})

const navItems = [
  { label: 'Home', to: '/', requiresAuth: false },
  { label: 'Features', to: '/features', requiresAuth: false },
  { label: 'Projects', to: projectRoute.value, requiresAuth: true, isTenantSpecific: true },
]
</script>

<template>
  <div class="flex flex-col min-h-screen bg-light-background dark:bg-dark-background text-light-textbase dark:text-dark-textbase">
    <!-- Sticky Navbar -->
    <LayoutPageNavbar
      :brand="'CommuniQueue'"
      :menu-items="navItems"
      class="sticky top-0 z-30 flex-shrink-0 bg-light-surface dark:bg-dark-surface"
    />

    <!-- Main Content - now takes up full width -->
    <div class="flex-grow overflow-auto">
      <slot />
    </div>

    <!-- Sticky Footer -->
    <LayoutPageFooter
      :app-name="'CommuniQueue'"
      :year="currentYear"
      name="Battleline Productions"
      class="sticky bottom-0 z-30 bg-light-surface dark:bg-dark-surface"
    />
  </div>
</template>
