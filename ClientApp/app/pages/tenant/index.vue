<script setup lang="ts">
import { useQuery } from '@tanstack/vue-query'
import type { UserInfoResponse } from '@logto/nuxt'
import { useTenant } from '~/composables/use-tenant'
import useTenants from '~/composables/use-tenant-service'
import type { IAppTenantInfo, IApiResponse } from '~/types'

// composables
const { setTenant } = useTenant()
const { getTenantsByUser } = useTenants()

// Get current user info (email, sub, etc.) from Logto
const user: UserInfoResponse = useLogtoUser()

/**
 * Query to load tenants by user SSO ID
 * TQueryFnData = IApiResponse<IAppTenantInfo[]> (what our request returns)
 * TError = Error
 */
const {
  data, // A ref containing the query result
  isLoading, // A ref indicating if it's still loading
  isError, // A ref indicating if an error occurred
  error, // A ref holding the actual error object, if any
} = useQuery<IApiResponse<IAppTenantInfo[]>, Error>({
  // Unique key identifies this query
  queryKey: ['tenantsByUser', user.sub],
  // Actual function that fetches the data
  queryFn: () => getTenantsByUser(user.sub),
})

// Helper to expose tenants array (or empty if data is null/undefined)
const tenants = computed<IAppTenantInfo[]>(() => {
  return data.value?.data ?? []
})

// Show a user-friendly error message
const errorMessage = computed(() => {
  if (error.value?.message) {
    return error.value.message
  }
  return 'An unknown error occurred.'
})

// When the user selects a tenant, store it and go to the dashboard
function selectTenant(tenantId: string) {
  setTenant(tenantId)
  navigateTo(`/tenant/${tenantId}/dashboard/projects`)
}
</script>

<template>
  <div
    class="min-h-screen flex items-center justify-center
           bg-light-background dark:bg-dark-background
           text-light-textbase dark:text-dark-textbase"
  >
    <!-- Card-like container -->
    <div
      class="bg-light-surface dark:bg-dark-surface
             shadow-md rounded-lg p-6 w-full max-w-md"
    >
      <h1 class="text-2xl font-semibold mb-4">
        Select a Tenant
      </h1>

      <!-- Loading State -->
      <div
        v-if="isLoading"
        class="flex items-center space-x-2"
      >
        <LoadingSpinner />
        <span>Loading tenants...</span>
      </div>

      <!-- Error State -->
      <div
        v-else-if="isError"
        class="space-y-2"
      >
        <p class="text-light-error dark:text-dark-error">
          {{ errorMessage }}
        </p>
        <!-- Optionally show a retry or other fallback here -->
      </div>

      <!-- No tenants -->
      <div
        v-else-if="tenants.length === 0"
        class="space-y-4"
      >
        <p class="text-light-secondary dark:text-dark-secondary">
          You have no tenants yet.
        </p>
        <nuxt-link
          to="/tenant/create"
          class="bg-light-primary dark:bg-dark-primary text-white font-semibold px-4 py-2 rounded-md
                 hover:opacity-90 transition-opacity"
        >
          Create a Tenant
        </nuxt-link>
      </div>

      <!-- Tenant list -->
      <div
        v-else
        class="space-y-4"
      >
        <p class="text-light-secondary dark:text-dark-secondary">
          Please select one of your existing tenants:
        </p>
        <ul class="space-y-2">
          <li
            v-for="tenant in tenants"
            :key="tenant.id"
            class="flex items-center justify-between
                   bg-light-background dark:bg-dark-background
                   rounded-md p-3 transition-colors
                   hover:opacity-90"
          >
            <div>
              <p class="font-medium text-light-textbase dark:text-dark-textbase">
                {{ tenant.name }}
              </p>
              <p class="text-sm text-light-secondary dark:text-dark-secondary">
                {{ tenant.description }}
              </p>
            </div>
            <button
              class="bg-light-primary dark:bg-dark-primary text-white px-3 py-2 rounded-md
                     hover:opacity-80 transition-opacity"
              @click="selectTenant(tenant.id)"
            >
              Select
            </button>
          </li>
        </ul>

        <div class="flex justify-end">
          <nuxt-link
            to="/tenant/create"
            class="bg-light-primary dark:bg-dark-primary text-white font-semibold px-4 py-2 rounded-md
                   hover:opacity-90 transition-opacity"
          >
            Create New Tenant
          </nuxt-link>
        </div>
      </div>
    </div>
  </div>
</template>
