<script setup lang="ts">
import { useTenant } from '~/composables/use-tenant'
import { useTenantService } from '~/composables/use-tenant-service'

const router = useRouter()
const isLoading = ref(true)
const tenants = ref([])

const { setTenant } = useTenant()
const { getTenantsForUser } = useTenantService()

onMounted(async () => {
  // Fetch user’s tenants
  const res = await getTenantsForUser()
  tenants.value = res
  isLoading.value = false
})

function selectTenant(tenantId: string) {
  setTenant(tenantId)
  router.push(`/tenant/${tenantId}/dashboard`)
}
</script>

<template>
  <div
    class="min-h-screen flex items-center justify-center bg-light-background dark:bg-dark-background text-light-textbase dark:text-dark-textbase"
  >
    <!-- Card-like container -->
    <div class="bg-light-surface dark:bg-dark-surface shadow-md rounded-lg p-6 w-full max-w-md">
      <h1 class="text-2xl font-semibold mb-4">
        Select a Tenant
      </h1>

      <div
        v-if="isLoading"
        class="flex items-center space-x-2"
      >
        <LoadingSpinner />
        <span>Loading tenants...</span>
      </div>

      <div
        v-else-if="tenants.length === 0"
        class="space-y-4"
      >
        <p class="text-light-secondary dark:text-dark-secondary">
          You have no tenants yet.
        </p>
        <nuxt-link
          to="/tenant/create"
          class="bg-light-primary dark:bg-dark-primary text-white font-semibold px-4 py-2 rounded-md hover:opacity-90 transition-opacity"
        >
          Create a Tenant
        </nuxt-link>
      </div>

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
            class="flex items-center justify-between bg-light-background dark:bg-dark-background rounded-md p-3 transition-colors hover:opacity-90"
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
              class="bg-light-secondary dark:bg-dark-secondary text-white px-3 py-2 rounded-md hover:opacity-80 transition-opacity"
              @click="selectTenant(tenant.id)"
            >
              Select
            </button>
          </li>
        </ul>

        <div class="flex justify-end">
          <nuxt-link
            to="/tenant/create"
            class="bg-light-primary dark:bg-dark-primary text-white font-semibold px-4 py-2 rounded-md hover:opacity-90 transition-opacity"
          >
            Create New Tenant
          </nuxt-link>
        </div>
      </div>
    </div>
  </div>
</template>
