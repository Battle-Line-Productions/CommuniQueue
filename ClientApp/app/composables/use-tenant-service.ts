import type {
  IApiResponse,
  IAppTenantInfo,
  ICreateTenantRequest,
  IUpdateTenantRequest,
} from '~/types'

export default function useTenants() {
  const config = useRuntimeConfig()
  const { handleApiCall, createFetchOptions } = useApiUtils()

  const baseUrl = `${config.public.apiBaseUrl}/api/v1/tenants`

  const createTenant = (request: ICreateTenantRequest) => {
    return handleApiCall(
      $fetch<IApiResponse<IAppTenantInfo>>(`${baseUrl}`, {
        ...createFetchOptions<IApiResponse<IAppTenantInfo>>(),
        method: 'POST',
        body: request,
      }),
      'createTenant',
    )
  }

  const getTenantsByUser = (ssoUserId: string) => {
    return handleApiCall(
      $fetch<IApiResponse<IAppTenantInfo[]>>(`${baseUrl}/user/${ssoUserId}`, createFetchOptions<IApiResponse<IAppTenantInfo[]>>()),
      'getTenantsByUser',
    )
  }

  const updateTenant = (tenantId: string, request: IUpdateTenantRequest) => {
    return handleApiCall(
      $fetch<IApiResponse<IAppTenantInfo>>(`${baseUrl}/${tenantId}`, {
        ...createFetchOptions<IApiResponse<IAppTenantInfo>>(),
        method: 'PUT',
        body: request,
      }),
      'updateTenant',
    )
  }

  return {
    createTenant,
    getTenantsByUser,
    updateTenant,
  }
}
