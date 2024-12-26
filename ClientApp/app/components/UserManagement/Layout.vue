<template>
  <div class="flex flex-col min-h-screen">
    <!-- Insert a header or a nav if you want -->
    <slot name="main-content" />
    <slot name="side-content" />
  </div>
</template>

<script setup lang="ts">
// minimal layout, expand as needed
</script>

<style scoped>
/* Layout-level styles here */
</style>
