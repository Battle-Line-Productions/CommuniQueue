<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import type { RouteLocationRaw } from 'vue-router'

// Example auth tracking (replace with your logic)
const isAuthenticated = ref(false)

/** A sub-menu item or direct link item */
interface NavbarItem {
  label: string
  to?: RouteLocationRaw
  requiresAuth?: boolean
  children?: NavbarItem[] // if it has sub-menu items
}

/** Optional info bar data */
interface InfoBarData {
  email?: string
  phone?: string
  socials?: {
    name: string
    link: string
    icon: string
  }[]
}

const props = defineProps<{
  /**
   * The brand or site name, e.g. "CommuniQueue"
   */
  brand: string

  /**
   * InfoBar data. Omit or provide an empty object to hide.
   */
  infoBar?: InfoBarData

  /**
   * Array of menu items
   */
  menuItems: NavbarItem[]
}>()

/** Whether the mobile menu is open */
const mobileMenuOpen = ref(false)
/** Color mode composable (dark/light/system) */
const colorMode = useColorMode()

/** Decide if we show the top InfoBar */
const showInfoBar = computed(() => {
  const info = props.infoBar
  if (!info) return false
  return Boolean(info.email || info.phone || (info.socials && info.socials.length > 0))
})

/** Filter out items requiring auth if user isn't authenticated */
const filteredMenuItems = computed<NavbarItem[]>(() => {
  return props.menuItems.filter((item) => {
    if (item.requiresAuth && !isAuthenticated.value) return false
    return true
  })
})

onMounted(async () => {
  // isAuthenticated.value = await checkAuthenticationStatus() or something
})

/** Toggle the mobile menu open/close */
function toggleMobileMenu() {
  mobileMenuOpen.value = !mobileMenuOpen.value
}

/** Rotate theme preference between dark/light/system */
function toggleTheme() {
  if (colorMode.preference === 'dark') {
    colorMode.preference = 'light'
  }
  else if (colorMode.preference === 'light') {
    colorMode.preference = 'system'
  }
  else {
    colorMode.preference = 'dark'
  }
}

/** Placeholder for a login function */
function handleLogin() {
  // TODO: Replace with your actual login code
  console.log('User wants to log in...')
}
</script>

<template>
  <!-- WRAPPER NAV ELEMENT -->
  <nav class="w-full shadow bg-light-surface dark:bg-dark-surface text-light-textbase dark:text-dark-textbase">
    <!-- TOP INFOBAR (optional) -->
    <div
      v-if="showInfoBar"
      class="flex justify-between items-center px-4 py-2 text-sm"
    >
      <!-- Left side: email -->
      <div class="flex items-center gap-2">
        <Icon
          name="ic:outline-email"
          class="h-4 w-4 text-light-primary dark:text-dark-primary"
        />
        <a
          v-if="props.infoBar?.email"
          :href="`mailto:${props.infoBar?.email}`"
          class="hover:text-light-primary dark:hover:text-dark-primary"
        >
          {{ props.infoBar?.email }}
        </a>
      </div>

      <!-- Right side: phone & socials -->
      <div class="flex items-center gap-4">
        <div class="flex items-center gap-2">
          <Icon
            name="ic:outline-settings-phone"
            class="h-4 w-4 text-light-primary dark:text-dark-primary"
          />
          <a
            v-if="props.infoBar?.phone"
            :href="`tel:${props.infoBar?.phone}`"
            class="hover:text-light-primary dark:hover:text-dark-primary"
          >
            {{ props.infoBar?.phone }}
          </a>
        </div>

        <!-- Social icons (if any) -->
        <template
          v-for="social in props.infoBar?.socials || []"
          :key="social.name"
        >
          <a
            :href="social.link"
            target="_blank"
            class="hover:text-light-primary dark:hover:text-dark-primary"
          >
            <Icon
              :name="social.icon"
              class="h-4 w-4"
            />
          </a>
        </template>
      </div>
    </div>

    <!-- MAIN NAVBAR -->
    <div class="flex items-center justify-between px-4 py-3">
      <!-- Brand / Logo -->
      <div>
        <NuxtLink
          to="/"
          class="flex items-center space-x-2"
        >
          <!-- Could do an <img> if you have a logo. Or an icon. -->
          <span class="font-bold text-xl text-light-primary dark:text-dark-primary">
            {{ props.brand }}
          </span>
        </NuxtLink>
      </div>

      <!-- Desktop Menu (hidden on small screens) -->
      <ul class="hidden md:flex items-center space-x-4">
        <!-- Navigation Items -->
        <li
          v-for="(item, index) in filteredMenuItems"
          :key="index"
          class="relative group"
        >
          <!-- If the item has children => dropdown -->
          <template v-if="item.children && item.children.length > 0">
            <UUButton class="inline-flex items-center gap-1 hover:text-light-primary dark:hover:text-dark-primary">
              {{ item.label }}
              <Icon
                name="mdi:chevron-down"
                class="w-4 h-4"
              />
            </UUButton>

            <!-- Sub-menu -->
            <ul
              class="absolute left-0 top-full bg-white dark:bg-gray-700 shadow rounded mt-1 w-48 py-2 invisible opacity-0 group-hover:visible group-hover:opacity-100 transition-opacity"
            >
              <li
                v-for="(child, idx) in item.children"
                :key="idx"
                class="px-4 py-2 hover:bg-gray-100 dark:hover:bg-gray-600"
              >
                <NuxtLink
                  v-if="child.to"
                  :to="child.to"
                  class="block w-full"
                >
                  {{ child.label }}
                </NuxtLink>
                <span v-else>{{ child.label }}</span>
              </li>
            </ul>
          </template>
          <!-- If no sub-items => normal link -->
          <template v-else>
            <NuxtLink
              v-if="item.to"
              :to="item.to"
              class="hover:text-light-primary dark:hover:text-dark-primary"
            >
              {{ item.label }}
            </NuxtLink>
            <span v-else>{{ item.label }}</span>
          </template>
        </li>

        <!-- SEPARATOR BEFORE LOGIN & THEME -->
        <li>
          <span class="mx-1 text-gray-400 dark:text-gray-500 select-none">|</span>
        </li>

        <!-- LOGIN UUButton -->
        <li>
          <UUButton
            class="p-2 rounded hover:bg-gray-100 dark:hover:bg-gray-800"
            @click="handleLogin"
          >
            Login
          </UUButton>
        </li>

        <!-- SEPARATOR BEFORE THEME SWITCHER -->
        <li>
          <span class="mx-1 text-gray-400 dark:text-gray-500 select-none">|</span>
        </li>

        <!-- THEME SWITCHER (Desktop) -->
        <li>
          <UUButton
            class="p-2 rounded hover:bg-gray-100 dark:hover:bg-gray-800"
            :title="'Rotate Theme (light/dark/system)'"
            @click="toggleTheme"
          >
            <!-- Show icon based on preference -->
            <Icon
              v-if="colorMode.preference === 'dark'"
              name="ph:sun-bold"
              class="w-5 h-5"
            />
            <Icon
              v-else-if="colorMode.preference === 'system'"
              name="ph:desktop-tower-bold"
              class="w-5 h-5"
            />
            <Icon
              v-else
              name="ph:moon-bold"
              class="w-5 h-5"
            />
          </UUButton>
        </li>
      </ul>

      <!-- MOBILE MENU UUButton -->
      <UUButton
        class="md:hidden p-2 rounded hover:bg-gray-100 dark:hover:bg-gray-800"
        @click="toggleMobileMenu"
      >
        <Icon
          name="heroicons:bars-3-bottom-right"
          class="w-6 h-6"
        />
      </UUButton>
    </div>

    <!-- MOBILE MENU (visible when mobileMenuOpen) -->
    <transition name="fade">
      <ul
        v-if="mobileMenuOpen"
        class="md:hidden flex flex-col space-y-1 px-4 pb-2"
      >
        <!-- Navigation Items -->
        <li
          v-for="(item, index) in filteredMenuItems"
          :key="index"
          class="border-b border-light-secondary/10 dark:border-dark-secondary/20 py-2"
        >
          <!-- Check if there's a sub-menu -->
          <template v-if="item.children && item.children.length > 0">
            <div class="flex items-center justify-between">
              <span class="font-semibold">{{ item.label }}</span>
              <!-- (Optional) Add a chevron or toggle for collapsible sub-menu -->
            </div>
            <!-- Sub-items (always open in this example) -->
            <ul class="ml-4 mt-2 space-y-1">
              <li
                v-for="(child, idx) in item.children"
                :key="idx"
              >
                <NuxtLink
                  v-if="child.to"
                  :to="child.to"
                  class="block py-1 text-sm hover:underline"
                  @click="mobileMenuOpen = false"
                >
                  {{ child.label }}
                </NuxtLink>
                <span
                  v-else
                  class="block py-1 text-sm"
                >
                  {{ child.label }}
                </span>
              </li>
            </ul>
          </template>
          <!-- If no sub-items => simple link -->
          <template v-else>
            <NuxtLink
              v-if="item.to"
              :to="item.to"
              class="hover:underline"
              @click="mobileMenuOpen = false"
            >
              {{ item.label }}
            </NuxtLink>
            <span v-else>{{ item.label }}</span>
          </template>
        </li>

        <!-- MOBILE SEPARATOR (above login & theme) -->
        <li class="mt-2">
          <hr class="border-light-secondary/20 dark:border-dark-secondary/20">
        </li>

        <!-- MOBILE LOGIN UUButton -->
        <li class="flex items-center justify-start gap-2 py-2">
          <UUButton
            class="p-2 rounded hover:bg-gray-100 dark:hover:bg-gray-800"
            @click="handleLogin"
          >
            Login
          </UUButton>
        </li>

        <!-- MOBILE THEME SWITCHER -->
        <li class="flex items-center justify-start gap-2 py-2">
          <UUButton
            class="p-2 rounded hover:bg-gray-100 dark:hover:bg-gray-800"
            :title="'Rotate Theme (light/dark/system)'"
            @click="toggleTheme"
          >
            <Icon
              v-if="colorMode.preference === 'dark'"
              name="ph:sun-bold"
              class="w-5 h-5"
            />
            <Icon
              v-else-if="colorMode.preference === 'system'"
              name="ph:desktop-tower-bold"
              class="w-5 h-5"
            />
            <Icon
              v-else
              name="ph:moon-bold"
              class="w-5 h-5"
            />
          </UUButton>
        </li>
      </ul>
    </transition>
  </nav>
</template>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.15s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
