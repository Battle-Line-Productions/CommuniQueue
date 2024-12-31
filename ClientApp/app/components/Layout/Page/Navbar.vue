<script setup lang="ts">
import { ref, computed, watch } from 'vue'
import { useQuery } from '@tanstack/vue-query'
import type { UserInfoResponse } from '@logto/nuxt'
import type { RouteLocationRaw } from 'vue-router'
import { useRouter } from 'vue-router'
import useUsers from '~/composables/use-user-service'
import useTenants from '~/composables/use-tenant-service'
import type { IUser, IAppTenantInfo } from '~/types'

// ----------- Existing imports & declarations -----------
interface NavbarItem {
  label: string
  to?: RouteLocationRaw
  requiresAuth?: boolean
  isTenantSpecific?: boolean
  children?: NavbarItem[]
}

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
  brand: string
  infoBar?: InfoBarData
  menuItems: NavbarItem[]
}>()

const mobileMenuOpen = ref(false)
const colorMode = useColorMode()
const user: UserInfoResponse = useLogtoUser()

const router = useRouter()

/**
 * Composable for tenant selection
 */
const { currentTenantId, setTenant } = useTenant()

/**
 * We'll fetch user+tenant data via Vue Query
 */
interface IUserTenantsResult {
  user: IUser
  tenants: IAppTenantInfo[]
}

const { getOrCreateUser } = useUsers()
const { getTenantsByUser } = useTenants()

/**
 * Query to create or get user => fetch tenants => return them
 */
const {
  data: userTenantsResult,
  error,
} = useQuery<IUserTenantsResult, Error>({
  // Use the same query key you've used elsewhere:
  queryKey: ['userTenants', user?.sub],
  enabled: !!user?.sub, // only run if we have a user ID
  queryFn: async (): Promise<IUserTenantsResult> => {
    // 1) Create or get the user in the backend
    const fullNameSplit = user.name?.split(' ') || []
    const userResp = await getOrCreateUser({
      email: user.email ?? '',
      ssoId: user.sub ?? '',
      isActive: true,
      firstName: fullNameSplit[0] ?? '',
      lastName: fullNameSplit[1] ?? '',
    })

    if (!userResp?.data) {
      throw new Error('Failed to create/find user')
    }
    const backendUser = userResp.data

    // 2) Fetch tenants for that user by their SSO ID
    const tenantsResp = await getTenantsByUser(backendUser.ssoId)
    if (!tenantsResp?.data) {
      throw new Error('Failed to load tenants')
    }

    return { user: backendUser, tenants: tenantsResp.data }
  },
})

/**
 * userTenants computed
 * We can use the query's data, which is cached across the app.
 */
const userTenants = computed<IAppTenantInfo[]>(() => {
  return userTenantsResult.value?.tenants ?? []
})

/**
 * Decide if we show the top InfoBar
 */
const showInfoBar = computed(() => {
  const info = props.infoBar
  if (!info) return false
  return Boolean(info.email || info.phone || (info.socials && info.socials.length > 0))
})

/**
 * Filter out items requiring auth if user isn't authenticated
 */
const filteredMenuItems = computed<NavbarItem[]>(() => {
  return props.menuItems
    .map((item) => {
      if (item.requiresAuth && !user) {
        return null
      }
      if (item.isTenantSpecific && user && !currentTenantId.value) {
        return {
          ...item,
          to: '/auth/logging-in',
        }
      }
      return item
    })
    .filter(Boolean) as NavbarItem[]
})

/**
 * Toggle the mobile menu open/close
 */
function toggleMobileMenu() {
  mobileMenuOpen.value = !mobileMenuOpen.value
}

/**
 * Rotate theme preference between dark/light/system
 */
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

/**
 * Utility to check if the route is currently active.
 */
function isActiveRoute(routeOrPath?: RouteLocationRaw): boolean {
  if (!routeOrPath) return false
  const currentPath = router.currentRoute.value.path

  if (typeof routeOrPath === 'string') {
    return currentPath === routeOrPath
  }
  if ('path' in routeOrPath && routeOrPath.path) {
    return currentPath === routeOrPath.path
  }
  return false
}

/**
 * Handle the user picking a tenant in the dropdown
 */
function onTenantSelected(tenantId: string) {
  setTenant(tenantId)
  // If you want to close the mobile menu after selection:
  mobileMenuOpen.value = false
}

// Optionally watch for errors (for debugging or UI)
watch(error, (err) => {
  if (err) {
    console.error('Navbar tenants query error:', err.message)
  }
})
</script>

<template>
  <!-- NAV WRAPPER -->
  <nav
    class="w-full shadow bg-light-surface dark:bg-dark-surface text-light-textbase dark:text-dark-textbase transition-colors duration-300"
  >
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
          class="hover:text-light-primary dark:hover:text-dark-primary transition-colors duration-200"
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
            class="hover:text-light-primary dark:hover:text-dark-primary transition-colors duration-200"
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
            class="hover:text-light-primary dark:hover:text-dark-primary transition-colors duration-200"
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
      <div class="flex items-center space-x-2">
        <!-- Logo image -->
        <NuxtLink to="/">
          <img
            src="/assets/images/communiqueueLogo.png"
            alt="CommuniQueue Logo"
            class="w-8 h-8 mr-2 object-contain"
          >
        </NuxtLink>

        <!-- Brand text -->
        <NuxtLink to="/">
          <span class="font-bold text-xl text-light-primary dark:text-dark-primary transition-colors duration-200">
            {{ props.brand }}
          </span>
        </NuxtLink>
      </div>

      <!-- DESKTOP MENU (hidden on small screens) -->
      <ul class="hidden md:flex items-center space-x-4">
        <!-- Navigation Items -->
        <li
          v-for="(item, index) in filteredMenuItems"
          :key="index"
          class="relative group"
        >
          <!-- If the item has children => dropdown -->
          <template v-if="item.children && item.children.length > 0">
            <UButton
              class="inline-flex items-center gap-1 transition-colors duration-200"
              :class="{
                'text-light-primary dark:text-dark-primary font-semibold':
                  isActiveRoute(item.to),
              }"
            >
              {{ item.label }}
              <Icon
                name="mdi:chevron-down"
                class="w-4 h-4"
              />
            </UButton>

            <!-- Sub-menu -->
            <ul
              class="absolute left-0 top-full bg-white dark:bg-gray-700 shadow rounded mt-1 w-48 py-2 invisible opacity-0 group-hover:visible group-hover:opacity-100 transition-opacity"
            >
              <li
                v-for="(child, idx) in item.children"
                :key="idx"
                class="px-4 py-2 transition-colors duration-200 hover:bg-gray-100 dark:hover:bg-gray-600"
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
              class="px-3 py-2 transition-colors duration-200 hover:text-light-primary dark:hover:text-dark-primary"
              :class="{
                'text-light-primary dark:text-dark-primary font-semibold':
                  isActiveRoute(item.to),
              }"
            >
              {{ item.label }}
            </NuxtLink>
            <span
              v-else
              class="px-3 py-2 transition-colors duration-200 cursor-default"
            >
              {{ item.label }}
            </span>
          </template>
        </li>

        <!-- DESKTOP TENANT SWITCHER -->
        <li v-if="user">
          <LayoutPageTenantDropdown
            :tenants="userTenants"
            :current-tenant-id="currentTenantId"
            @select-tenant="onTenantSelected"
          />
        </li>

        <!-- SEPARATOR BEFORE LOGIN & THEME -->
        <li>
          <span class="mx-1 text-gray-400 dark:text-gray-500 select-none">
            |
          </span>
        </li>

        <!-- LOGIN / LOGOUT link -->
        <li>
          <a
            class="px-3 py-2 transition-colors duration-200 rounded hover:text-light-primary dark:hover:text-dark-primary"
            :href="`/sign-${user ? 'out' : 'in'}`"
          >
            Log {{ user ? 'out' : 'in' }}
          </a>
        </li>

        <!-- SEPARATOR BEFORE THEME SWITCHER -->
        <li>
          <span class="mx-1 text-gray-400 dark:text-gray-500 select-none">
            |
          </span>
        </li>

        <!-- THEME SWITCHER (Desktop) -->
        <li>
          <UButton
            class="flex items-center gap-1 px-3 py-2 transition-colors duration-200 rounded hover:text-light-primary dark:hover:text-dark-primary"
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
            <span class="text-sm">Theme</span>
          </UButton>
        </li>
      </ul>

      <!-- MOBILE MENU UButton -->
      <UButton
        class="md:hidden p-2 rounded transition-colors duration-200 hover:bg-gray-100 dark:hover:bg-gray-800"
        @click="toggleMobileMenu"
      >
        <Icon
          name="heroicons:bars-3-bottom-right"
          class="w-6 h-6"
        />
      </UButton>
    </div>

    <!-- MOBILE MENU (visible when mobileMenuOpen) -->
    <transition name="fade">
      <ul
        v-if="mobileMenuOpen"
        class="md:hidden flex flex-col space-y-1 px-4 pb-4 bg-light-surface dark:bg-dark-surface shadow transition-colors duration-300"
      >
        <!-- MOBILE TENANT SWITCHER -->
        <li
          v-if="user"
          class="py-2 border-b border-light-secondary/10 dark:border-dark-secondary/20"
        >
          <LayoutPageTenantDropdown
            :tenants="userTenants"
            :current-tenant-id="currentTenantId"
            @select-tenant="onTenantSelected"
          />
        </li>

        <!-- Navigation Items -->
        <li
          v-for="(item, index) in filteredMenuItems"
          :key="index"
          class="border-b border-light-secondary/10 dark:border-dark-secondary/20 py-2"
        >
          <!-- Check if there's a sub-menu -->
          <template v-if="item.children && item.children.length > 0">
            <div class="flex items-center justify-between">
              <span
                class="font-semibold"
                :class="{
                  'text-light-primary dark:text-dark-primary': isActiveRoute(item.to),
                }"
              >
                {{ item.label }}
              </span>
              <!-- (Optional) Could add a toggle icon for collapsible sub-menu -->
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
                  class="block py-1 text-sm transition-colors duration-200 hover:underline"
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
              class="block py-2 px-1 transition-colors duration-200 hover:text-light-primary dark:hover:text-dark-primary"
              :class="{
                'text-light-primary dark:text-dark-primary font-semibold': isActiveRoute(item.to),
              }"
              @click="mobileMenuOpen = false"
            >
              {{ item.label }}
            </NuxtLink>
            <span
              v-else
              class="block py-2 px-1 transition-colors duration-200"
            >
              {{ item.label }}
            </span>
          </template>
        </li>

        <!-- MOBILE SEPARATOR (above login & theme) -->
        <li class="mt-2">
          <hr class="border-light-secondary/20 dark:border-dark-secondary/20">
        </li>

        <!-- MOBILE LOGIN / LOGOUT -->
        <li class="flex items-center justify-start gap-2 py-2">
          <a
            class="p-2 rounded hover:bg-gray-100 dark:hover:bg-gray-800 transition-colors duration-200"
            :href="user ? '/sign-out' : '/sign-in'"
            @click="mobileMenuOpen = false"
          >
            {{ user ? 'Logout' : 'Login' }}
          </a>
        </li>

        <!-- MOBILE THEME SWITCHER -->
        <li class="flex items-center justify-start gap-2 py-2">
          <UButton
            class="flex items-center gap-1 p-2 rounded transition-colors duration-200 hover:bg-gray-100 dark:hover:bg-gray-800"
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
            <span class="text-sm">Theme</span>
          </UButton>
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
