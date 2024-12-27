<script lang="ts" setup>
import { ref, computed, onMounted, provide } from 'vue'
import type { LayoutPageNavbarMenu } from '~/types'

/**
 * Interface describing the portion of 'config' that this component needs.
 * Adjust fields according to your actual data shape.
 */
interface AppLayoutConfig {
  page?: {
    navbar?: {
      menus?: LayoutPageNavbarMenu[]
    }
  }
}

interface AppProjectLinks {
  links?: {
    github?: string
  }
}

interface AppConfig {
  /**
     * The display name of your application.
     */
  name: string

  /**
     * A nested object describing layout configs (menus, pages, etc.).
     */
  layout?: AppLayoutConfig

  /**
     * A nested object describing project-related links, e.g. GitHub link.
     */
  project?: AppProjectLinks
}

/**
 * Props definition for this component.
 */
const props = defineProps<{
  config: AppConfig
}>()

/**
 * Checks authentication status (using Logto).
 */
const checkAuthenticationStatus = async (): Promise<boolean> => {
  const client = useLogtoClient()

  if (!client) {
    throw new Error('Logto client is not available')
  }

  return await client.isAuthenticated()
}

/**
 * Example composable usage (like your original code).
 */
const $screen = useScreen()

/**
 * A computed array of menus, derived from the passed-in config prop.
 */
const menus = computed(() => {
  return (props.config?.layout?.page?.navbar?.menus || []) as LayoutPageNavbarMenu[]
})

// Drawer
const showDrawer = ref<boolean>(false)

/**
 * Provide a way to close the drawer from child components.
 */
provide('closeDrawer', () => {
  showDrawer.value = false
})

/**
 * Keep track of whether the user is authenticated.
 */
const isAuthenticated = ref<boolean>(false)

/**
 * When the component is mounted, check if the user is authenticated.
 */
onMounted(async () => {
  isAuthenticated.value = await checkAuthenticationStatus()
})

/**
 * Filter the menus so that items requiring auth are only shown if user is authenticated.
 */
const filteredMenus = computed(() => {
  return menus.value.filter((menu) => {
    return !menu.shouldRequireAuth || (menu.shouldRequireAuth && isAuthenticated.value)
  })
})
</script>

<template>
  <div class="fixed top-0 z-40 w-full transition-colors duration-300 lg:z-50">
    <div class="bg-light-surface dark:bg-dark-surface shadow">
      <!-- InfoBar Component - Placed at the very top -->
      <LayoutPageInfobar class="py-1" />
      <!-- Divider Line -->
      <div class="h-px bg-light-surface dark:bg-dark-surface max-w-screen-2xl mx-auto" />

      <!-- content -->
      <div class="flex items-center justify-between max-w-screen-2xl mx-auto py-3 px-4">
        <!-- title -->
        <div>
          <slot name="title">
            <NuxtLink
              to="/"
              class="font-bold text-lg text-light-primary dark:text-dark-primary"
            >
              <Icon
                name="simple-icons:nuxtdotjs"
                class="font-black text-xl font-mono mr-2 inline-block"
              />
              <!-- Use config prop rather than app config -->
              <span class="capitalize">{{ props.config.name }}</span>
            </NuxtLink>
          </slot>
        </div>

        <!-- menus -->
        <div
          v-if="$screen.higherThan('md', $screen.current.value)"
          class="flex space-x-4 items-center"
          :class="{ 'divide-x divide-light-secondary dark:divide-dark-secondary': menus.length > 0 }"
        >
          <div class="flex space-x-4 text-sm items-center">
            <!-- dynamic menus -->
            <template
              v-for="(item, i) in filteredMenus"
              :key="i"
            >
              <LayoutPageNavbarMenuRecursiveMenuComponent :menu="item" />
            </template>
          </div>
          <!-- others -->
          <div class="pl-4 flex space-x-3 text-xl">
            <LayoutPageNavbarDropdownThemeSwitcher />
          </div>
        </div>

        <!-- drawer:btn -->
        <div
          v-else
          class="flex space-x-4 items-center"
          :class="{ 'divide-x divide-light-secondary dark:divide-dark-secondary': menus.length > 0 }"
        >
          <div class="pl-4 flex space-x-3 text-xl">
            <!-- Example usage of config's GitHub link (if present) -->
            <BLLink
              v-if="props.config?.project?.links?.github"
              class="text-light-textbase dark:text-dark-textbase hover:text-light-primary dark:hover:text-dark-primary"
              @click.prevent="() => (showDrawer = !showDrawer)"
            >
              <Icon name="heroicons:bars-3-bottom-right-20-solid" />
            </BLLink>
          </div>
        </div>
      </div>

      <!-- misc -->
      <!-- drawer -->
      <ActionSheet
        v-if="!$screen.higherThan('md', $screen.current.value) && showDrawer"
        @close="() => (showDrawer = false)"
      >
        <ActionSheetGroup>
          <ActionSheetHeader>
            <ActionSheetHeaderTitle text="Menu" />
          </ActionSheetHeader>
          <!-- dynamic menus -->
          <ActionSheetItem>
            <div
              class="flex flex-col text-sm items-center divide-y divide-light-secondary dark:divide-dark-secondary text-center"
            >
              <template
                v-for="(item, i) in filteredMenus"
                :key="i"
              >
                <LayoutPageNavbarMenuRecursiveDrawerMenuComponent
                  :menu="item"
                  :index="i"
                />
              </template>
            </div>
          </ActionSheetItem>
          <ActionSheetItem class="flex flex-col">
            <div class="pb-2">
              <div class="mt-2 mb-2 text-sm font-bold capitalize">
                Change Theme
              </div>
              <LayoutPageNavbarDropdownThemeSwitcher type="select-box" />
            </div>
          </ActionSheetItem>
        </ActionSheetGroup>
      </ActionSheet>
    </div>
  </div>
</template>
