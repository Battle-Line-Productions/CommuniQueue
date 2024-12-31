<script setup lang="ts">
import { useMutation } from '@tanstack/vue-query'
import type { UserInfoResponse } from '@logto/nuxt'
import { useTenant } from '~/composables/use-tenant'
import useTenants from '~/composables/use-tenant-service'
import type { IAppTenantInfo, IApiResponse, ICreateTenantRequest } from '~/types'

const user: UserInfoResponse = useLogtoUser()

const currentSsoUserId = user.sub

// Local refs for form fields
const tenantName = ref('')
const tenantDescription = ref('')

// Tenant composables
const { setTenant } = useTenant()
const { createTenant } = useTenants()

/**
 * We type our mutation so:
 *  - TData = IApiResponse<IAppTenantInfo> (what createTenant returns)
 *  - TError = Error (or unknown if you prefer)
 *  - TVariables = ICreateTenantRequest (the shape of the payload)
 */
const {
  mutate,
  isPending,
  isError,
  error,
} = useMutation<IApiResponse<IAppTenantInfo>, Error | null, ICreateTenantRequest>({
  mutationFn: async (payload) => {
    // call your composable function
    return await createTenant(payload)
  },
  onSuccess(data) {
    // Ensure we have a valid tenant object
    if (data.data) {
      // store the tenant
      setTenant(data.data.id)
      // route to the newly created tenant’s dashboard
      navigateTo(`/tenant/${data.data.id}/dashboard/projects`)
    }
  },
  onError(err: Error | null) {
    // handle the error scenario
    console.error('Failed to create tenant:', err)
  },
})

// For a user-friendly error message in the template
const errorMessage = computed(() => {
  if (error.value && error.value.message) {
    return error.value.message
  }
  return 'Failed to create tenant.'
})

// Trigger our mutation from the form
function handleSubmit() {
  mutate({
    tenantName: tenantName.value.trim(),
    tenantDescription: tenantDescription.value.trim(),
    ssoUserId: currentSsoUserId,
  })
}
</script>

<template>
  <div
    class="min-h-screen flex items-center justify-center
           bg-light-background dark:bg-dark-background
           text-light-textbase dark:text-dark-textbase px-4"
  >
    <!-- Container Card -->
    <div
      class="bg-light-surface dark:bg-dark-surface
             shadow-md rounded-lg p-6 w-full max-w-md"
    >
      <!-- Title -->
      <h1 class="text-2xl font-semibold mb-2">
        Create Tenant
      </h1>
      <!-- Subtitle / Instructions -->
      <p class="text-light-secondary dark:text-dark-secondary mb-6">
        Please provide a name and an optional description for your new tenant.
      </p>

      <!-- Error Message -->
      <p
        v-if="isError"
        class="text-red-500 mb-4"
      >
        {{ errorMessage }}
      </p>

      <!-- Form -->
      <form
        class="space-y-4"
        @submit.prevent="handleSubmit"
      >
        <!-- Tenant Name -->
        <UFormGroup
          label="Tenant Name"
          name="tenantName"
          required
          class="block space-y-1"
        >
          <UInput
            v-model="tenantName"
            placeholder="Enter tenant name"
            required
            class="w-full"
          />
        </UFormGroup>

        <!-- Tenant Description -->
        <UFormGroup
          label="Description"
          name="tenantDescription"
          class="block space-y-1"
        >
          <UTextarea
            v-model="tenantDescription"
            placeholder="(optional) short description"
            class="w-full"
          />
        </UFormGroup>

        <!-- Submit Button -->
        <div class="flex justify-end mt-6">
          <UButton
            type="submit"
            :disabled="isPending"
            color="primary"
            variant="solid"
            class="px-4 py-2 rounded-md
         transition-all duration-300
         text-white
         hover:opacity-90
         focus:ring-2 focus:ring-offset-2
         disabled:opacity-50 disabled:cursor-not-allowed"
          >
            {{ isPending ? 'Creating...' : 'Create Tenant' }}
          </UButton>
        </div>
      </form>
    </div>
  </div>
</template>
