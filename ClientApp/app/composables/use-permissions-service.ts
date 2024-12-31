import type { IPermission, IApiResponse, ICreatePermissionRequest, IUpdatePermissionRequest, EntityType } from '~/types'

export default function usePermissions() {
  const config = useRuntimeConfig()
  const tenant = useTenant()

  const baseUrl = `${config.public.apiBaseUrl}/api/v1/tenant/${tenant.currentTenantId.value}permissions`
  const { handleApiCall, createFetchOptions } = useApiUtils()

  const createPermission = (request: ICreatePermissionRequest) => {
    return handleApiCall(
      $fetch<IApiResponse<IPermission>>(`${baseUrl}`, {
        ...createFetchOptions<IApiResponse<IPermission>>(),
        method: 'POST',
        body: request,
      }),
      'createPermission',
    )
  }

  const getPermission = (userId: string, entityId: string, entityType: EntityType) => {
    return handleApiCall($fetch<IApiResponse<IPermission>>(`${baseUrl}/${userId}/${entityId}/${entityType}`, createFetchOptions<IApiResponse<IPermission>>()), 'getPermission')
  }

  const getPermissionsByEntity = (entityId: string, entityType: EntityType) => {
    console.log('getPermissionsByEntity', entityId, entityType)
    return handleApiCall(
      $fetch<IApiResponse<IPermission[]>>(`${baseUrl}/entity/${entityId}/${entityType}`, createFetchOptions<IApiResponse<IPermission[]>>()),
      'getPermissionsByEntity',
    )
  }

  const updatePermission = (request: IUpdatePermissionRequest) => {
    return handleApiCall(
      $fetch<IApiResponse<IPermission>>(`${baseUrl}`, {
        ...createFetchOptions<IApiResponse<IPermission>>(),
        method: 'PUT',
        body: request,
      }),
      'updatePermission',
    )
  }

  const deletePermission = (userId: string, entityId: string, entityType: EntityType) => {
    return handleApiCall(
      $fetch<IApiResponse<boolean>>(`${baseUrl}/${userId}/${entityId}/${entityType}`, {
        ...createFetchOptions<IApiResponse<boolean>>(),
        method: 'DELETE',
      }),
      'deletePermission',
    )
  }

  return {
    createPermission,
    getPermission,
    getPermissionsByEntity,
    updatePermission,
    deletePermission,
  }
}
