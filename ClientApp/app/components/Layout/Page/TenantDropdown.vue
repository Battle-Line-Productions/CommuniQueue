<script setup lang="ts">
import type { IAppTenantInfo } from '~/types'

/**
 * Props
 */
const props = defineProps<{
  /**
     * All tenants for this user
     */
  tenants: IAppTenantInfo[]

  /**
     * The currently selected tenant ID
     */
  currentTenantId: string | null

  /**
     * If true, use "mobile" layout instead of absolute dropdown
     */
  mobile?: boolean
}>()

/**
 * Emitted events
 */
const emit = defineEmits<{
  /**
     * Fires when the user selects a tenant in the dropdown
     */
  (e: 'select-tenant', tenantId: string): void
}>()

/**
 * Local state
 */
const showDropdown = ref(false)
const searchTerm = ref('')
const maxToShow = ref(5) // how many tenants to show at once

/**
 * Computed: Filter tenants by name if searchTerm is entered
 */
const filteredTenants = computed(() => {
  if (!searchTerm.value.trim()) {
    return props.tenants
  }
  return props.tenants.filter(t =>
    t.name.toLowerCase().includes(searchTerm.value.toLowerCase()),
  )
})

/**
 * Computed: Apply "maxToShow" limit for large lists
 */
const visibleTenants = computed(() => {
  return filteredTenants.value.slice(0, maxToShow.value)
})

/**
 * Toggle the dropdown open/closed
 */
function toggleDropdown() {
  showDropdown.value = !showDropdown.value
}

/**
 * Select tenant => emit event to parent
 */
function selectTenant(tenantId: string) {
  emit('select-tenant', tenantId)
  showDropdown.value = false
}

/**
 * Load more tenants
 */
function loadMoreTenants() {
  maxToShow.value += 5
}

/**
 * Helper to get the name for the currently selected tenant
 */
const currentTenantName = computed(() => {
  return props.tenants.find(t => t.id === props.currentTenantId)?.name || 'Select Tenant'
})
</script>

<template>
  <!-- Outer container: changes layout based on mobile or desktop -->
  <div
    :class="props.mobile
      ? 'relative w-full text-left'
      : 'relative inline-block text-left'"
  >
    <!-- The trigger button -->
    <button
      type="button"
      class="inline-flex items-center gap-1 px-4 py-2 bg-white border border-gray-300 rounded-md shadow-xs text-sm font-medium text-gray-700 hover:bg-gray-50"
      @click="toggleDropdown"
    >
      <!-- Current tenant name OR fallback text -->
      <span>{{ currentTenantName }}</span>
      <!-- Down arrow icon -->
      <Icon
        name="mdi:chevron-down"
        class="h-4 w-4"
      />
    </button>

    <!-- The dropdown panel -->
    <div
      v-if="showDropdown"
      :class="props.mobile
        ? 'static mt-2 w-full bg-white border border-gray-200 rounded-md shadow-lg ring-1 ring-black ring-opacity-5'
        : 'origin-top-right absolute right-0 mt-2 w-64 bg-white border border-gray-200 rounded-md shadow-lg ring-1 ring-black ring-opacity-5'"
    >
      <!-- Search box -->
      <div class="p-2">
        <input
          v-model="searchTerm"
          type="text"
          class="w-full border border-gray-300 rounded-sm px-2 py-1 text-sm"
          placeholder="Search tenants..."
        >
      </div>

      <!-- 'Create Tenant' link -->
      <NuxtLink
        to="/tenant/create"
        class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100"
      >
        <Icon
          name="mdi:plus"
          class="mr-2 h-4 w-4"
        />
        Create Tenant
      </NuxtLink>

      <!-- Tenants list (scrollable if large) -->
      <div class="max-h-60 overflow-y-auto border-t border-gray-200">
        <div
          v-for="tenant in visibleTenants"
          :key="tenant.id"
          class="flex items-center justify-between px-4 py-2 text-sm text-gray-700 hover:bg-gray-100"
        >
          <!-- Clicking the tenant name triggers selection -->
          <button
            class="grow text-left flex items-center gap-2"
            @click="selectTenant(tenant.id)"
          >
            <!-- Show a check icon if this tenant is the active one -->
            <Icon
              v-if="tenant.id === props.currentTenantId"
              name="mdi:check"
              class="h-4 w-4 text-blue-600"
            />
            {{ tenant.name }}
          </button>

          <!-- Gear icon => tenant settings -->
          <NuxtLink
            :to="`/tenant/${tenant.id}/settings`"
            class="ml-2 text-gray-400 hover:text-gray-600"
            title="Tenant Settings"
          >
            <Icon
              name="mdi:cog"
              class="h-4 w-4"
            />
          </NuxtLink>
        </div>
      </div>

      <!-- 'Load more...' if not all are shown -->
      <div
        v-if="visibleTenants.length < filteredTenants.length"
        class="p-2"
      >
        <button
          class="w-full text-left text-blue-600 hover:underline text-sm"
          @click="loadMoreTenants"
        >
          Load more...
        </button>
      </div>
    </div>
  </div>
</template>
