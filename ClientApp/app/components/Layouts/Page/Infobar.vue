<script lang="ts" setup>
import { computed } from 'vue'

/**
 * Interface describing your social links.
 */
interface SocialLink {
  name: string
  link: string
  icon: string
}

/**
 * Interface describing the infobar data.
 */
interface NavbarInfobar {
  email?: string
  phone?: string
  socials?: SocialLink[]
}

/**
 * Interface describing optional fields in the layout configuration
 * that this component needs.
 */
interface NavbarConfig {
  infobar?: NavbarInfobar
}

interface PageConfig {
  navbar?: NavbarConfig
}

interface LayoutConfig {
  page?: PageConfig
}

/**
 * Main app config interface for what this component depends on.
 */
interface AppConfig {
  layout?: LayoutConfig
}

/**
 * Define props for this component. We only need the `config` object here.
 */
const props = defineProps<{
  config: AppConfig
}>()

/**
 * Extract the information bar data using computed properties.
 * Defaults to an empty object if none is present.
 */
const infoBarData = computed<NavbarInfobar>(() => {
  return props.config?.layout?.page?.navbar?.infobar || {}
})

/**
 * Compute a flag to determine if the info bar should be displayed.
 * It checks whether:
 *  - The object is not empty
 *  - There's at least one truthy property
 */
const showInfoBar = computed<boolean>(() => {
  const data = infoBarData.value
  const keys = Object.keys(data)
  if (keys.length === 0) return false

  // Check if any key has a truthy value (e.g. non-empty string, array with length)
  return Object.values(data).some(v => v)
})
</script>

<template>
  <div
    v-if="showInfoBar"
    class="bg-light-surface dark:bg-dark-surface shadow"
  >
    <div class="max-w-7xl mx-auto flex justify-between items-center py-2 px-6">
      <!-- Email on the left -->
      <div class="flex items-center space-x-2">
        <Icon
          name="ic:outline-email"
          class="text-light-primary dark:text-dark-primary h-4 w-4"
        />
        <a
          v-if="infoBarData.email"
          :href="`mailto:${infoBarData.email}`"
          class="text-sm text-light-textbase dark:text-dark-textbase hover:text-light-primary dark:hover:text-dark-primary"
        >
          {{ infoBarData.email }}
        </a>
      </div>
      <!-- Phone and socials on the right -->
      <div class="flex items-center space-x-4">
        <!-- Phone -->
        <div class="flex items-center space-x-2">
          <Icon
            name="ic:outline-settings-phone"
            class="text-light-primary dark:text-dark-primary h-4 w-4"
          />
          <a
            v-if="infoBarData.phone"
            :href="`tel:${infoBarData.phone}`"
            class="text-sm text-light-textbase dark:text-dark-textbase hover:text-light-primary dark:hover:text-dark-primary"
          >
            {{ infoBarData.phone }}
          </a>
        </div>
        <!-- Divider -->
        <span class="border-r border-light-secondary dark:border-dark-secondary h-6" />
        <!-- Social Icons -->
        <a
          v-for="social in infoBarData.socials"
          :key="social.name"
          :href="social.link"
          target="_blank"
          class="text-light-textbase dark:text-dark-textbase hover:text-light-primary dark:hover:text-dark-primary"
        >
          <Icon
            :name="social.icon"
            class="h-4 w-4"
          />
        </a>
      </div>
    </div>
  </div>
</template>
