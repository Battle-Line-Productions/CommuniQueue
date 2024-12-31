import type { IAppTenantInfo, IApiResponse } from '~/types'

export default function useTenantUserManagement() {
  const config = useRuntimeConfig()
  const { handleApiCall, createFetchOptions } = useApiUtils()

  const baseUrl = `${config.public.apiBaseUrl}/api/v1/tenant`

  /**
   * Add a user to a tenant
   * POST: /api/v1/tenant/{tenantId}/users/{userId}
   */
  const addUserToTenant = (tenantId: string, userId: string) => {
    return handleApiCall(
      $fetch<IApiResponse<IAppTenantInfo>>(`${baseUrl}/${tenantId}/users/${userId}`, {
        ...createFetchOptions<IApiResponse<IAppTenantInfo>>(),
        method: 'POST',
      }),
      'addUserToTenant',
    )
  }

  /**
   * Remove a user from a tenant
   * DELETE: /api/v1/tenant/{tenantId}/users/{userId}
   */
  const removeUserFromTenant = (tenantId: string, userId: string) => {
    return handleApiCall(
      $fetch<IApiResponse<IAppTenantInfo>>(`${baseUrl}/${tenantId}/users/${userId}`, {
        ...createFetchOptions<IApiResponse<IAppTenantInfo>>(),
        method: 'DELETE',
      }),
      'removeUserFromTenant',
    )
  }

  return {
    addUserToTenant,
    removeUserFromTenant,
  }
}
