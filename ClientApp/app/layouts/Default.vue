<script setup lang="ts">
const currentYear = new Date().getFullYear()

const navItems = [
  { label: 'Home', to: '/', requiresAuth: false },
  { label: 'Features', to: '/features', requiresAuth: false },
  { label: 'Projects', to: '/dashboard/projects', requiresAuth: false },
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
