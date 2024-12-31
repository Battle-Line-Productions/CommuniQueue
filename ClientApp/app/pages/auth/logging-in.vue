<template>
  <div
    class="min-h-screen flex flex-col items-center justify-center
           bg-light-background dark:bg-dark-background
           text-light-textbase dark:text-dark-textbase"
  >
    <div class="flex flex-col items-center space-y-4">
      <!-- Loading State -->
      <p
        v-if="isLoading"
        class="text-lg font-semibold"
      >
        Checking user information...
      </p>
      <!-- Error State -->
      <p
        v-else-if="isError"
        class="text-light-error dark:text-dark-error"
      >
        {{ errorMessage }}
      </p>
      <!-- Show a spinner if loading or error -->
      <LoadingSpinner v-if="isLoading || isError" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { useQuery } from '@tanstack/vue-query'
import type { UserInfoResponse } from '@logto/nuxt'
import type { IUser, IAppTenantInfo } from '~/types'
import useUsers from '~/composables/use-user-service'
import useTenants from '~/composables/use-tenant-service'

// The shape returned by our queryFn
interface IUserTenantsResult {
  user: IUser
  tenants: IAppTenantInfo[]
}

const logtoUserData: UserInfoResponse = useLogtoUser()

const { getOrCreateUser } = useUsers()
const { getTenantsByUser } = useTenants()
const { setTenant } = useTenant()

const {
  data: userTenantsData,
  isLoading,
  isError,
  error,
} = useQuery<IUserTenantsResult, Error>({
  queryKey: ['userTenants', logtoUserData.sub],

  queryFn: async (): Promise<IUserTenantsResult> => {
    // Step 1: Create or get the user in the backend
    const fullNameSplit = logtoUserData.name?.split(' ') || []
    const userResp = await getOrCreateUser({
      email: logtoUserData.email ?? '',
      ssoId: logtoUserData.sub ?? '',
      isActive: true,
      firstName: fullNameSplit[0] ?? '',
      lastName: fullNameSplit[1] ?? '',
    })

    if (!userResp?.data) {
      throw new Error('Failed to create/find user')
    }
    const user = userResp.data

    // Step 2: Fetch tenants for that user by their SSO ID
    const tenantsResp = await getTenantsByUser(user.ssoId)
    if (!tenantsResp?.data) {
      throw new Error('Failed to load tenants')
    }

    return { user, tenants: tenantsResp.data }
  },
})

// Watch for successful query result
watch(userTenantsData, (tenantUserResult) => {
  if (!tenantUserResult) return

  if (!tenantUserResult.tenants || tenantUserResult.tenants.length === 0) {
    navigateTo('/tenant/create')
  }
  else if (tenantUserResult.tenants.length === 1) {
    if (tenantUserResult.tenants[0]?.id) {
      setTenant(tenantUserResult.tenants[0].id)
    }
    navigateTo(`/tenant/${tenantUserResult.tenants[0]?.id}/dashboard/projects`)
  }
  else {
    navigateTo('/tenant')
  }
}, { immediate: false })

const errorMessage = computed(() => {
  if (error.value?.message) {
    return error.value.message
  }
  return 'An unknown error occurred.'
})
</script>
