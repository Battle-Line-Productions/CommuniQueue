<script setup lang="ts">
const currentYear = new Date().getFullYear()
</script>

<template>
  <div class="flex flex-col min-h-screen bg-light-background dark:bg-dark-background text-light-textbase dark:text-dark-textbase">
    <!-- Sticky Navbar -->
    <LayoutPageNavbar class="sticky top-0 z-30 flex-shrink-0 bg-light-surface dark:bg-dark-surface">
      <template #title>
        <NuxtLink
          to="/"
          class="flex items-center space-x-2"
        >
          <img
            alt="CommuniQueue Logo"
            src="/assets/images/communiqueueLogo.png"
            class="h-10 w-auto object-contain"
          >
          <span
            class="font-extrabold text-xl bg-gradient-to-r from-light-primary to-light-accent dark:from-dark-primary dark:to-dark-accent bg-clip-text text-transparent transition-colors duration-300 ease-in-out hover:from-light-accent hover:to-light-primary dark:hover:from-dark-accent dark:hover:to-dark-primary"
          >
            Communi<span class="text-light-accent dark:text-dark-accent">Queue</span>
          </span>
        </NuxtLink>
      </template>
    </LayoutPageNavbar>

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
