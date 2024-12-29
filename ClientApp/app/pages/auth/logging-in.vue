<script setup lang="ts">
import { useTenant } from '~/composables/useTenant'
import { useTenantService } from '~/composables/use-tenant-service'
import LoadingSpinner from '~/components/LoadingSpinner.vue'

async function handlePostLogin() {
  const { getTenantsForUser } = useTenantService()
  const { setTenant } = useTenant()

  const tenants = await getTenantsForUser()

  if (tenants.length === 0) {
    // No tenant exists => redirect user to create tenant
    return navigateTo('/tenant/create')
  }
  else if (tenants.length === 1) {
    // Exactly one => auto-select it
    setTenant(tenants[0].id)
    // Then navigate to that tenant’s dashboard
    return navigateTo(`/tenant/${tenants[0].id}/dashboard`)
  }
  else {
    // Multiple => let them pick in /tenant
    return navigateTo('/tenant')
  }
}

await handlePostLogin()
</script>

<template>
  <div
    class="min-h-screen flex flex-col items-center justify-center bg-light-background dark:bg-dark-background text-light-textbase dark:text-dark-textbase"
  >
    <div class="flex flex-col items-center space-y-4">
      <p class="text-lg font-semibold">
        Redirecting, please wait...
      </p>
      <LoadingSpinner />
    </div>
  </div>
</template>
