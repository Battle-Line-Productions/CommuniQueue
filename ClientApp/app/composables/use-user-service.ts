import type { EntityType, IApiResponse, ICreateUserRequest, IPermission, IUpdateUserRequest, IUser } from '~/types'

export default function useUsers() {
  const config = useRuntimeConfig()
  const { handleApiCall, createFetchOptions } = useApiUtils()

  const baseUrl = `${config.public.apiBaseUrl}/api/v1/users`

  const getOrCreateUser = (request: ICreateUserRequest) => {
    return handleApiCall(
      $fetch<IApiResponse<IUser>>(`${baseUrl}/initial`, {
        ...createFetchOptions<IApiResponse<IUser>>(),
        method: 'POST',
        body: request,
      }),
      'getorCreateUser',
    )
  }

  const createUser = (request: ICreateUserRequest) => {
    return handleApiCall(
      $fetch<IApiResponse<IUser>>(`${baseUrl}`, {
        ...createFetchOptions<IApiResponse<IUser>>(),
        method: 'POST',
        body: request,
      }),
      'createUser',
    )
  }

  const getUserById = (userId: string) => {
    return handleApiCall($fetch<IApiResponse<IUser>>(`${baseUrl}/${userId}`, createFetchOptions<IApiResponse<IUser>>()), 'getUserById')
  }

  const getUserBySsoId = (ssoId: string) => {
    return handleApiCall($fetch<IApiResponse<IUser>>(`${baseUrl}/sso/${ssoId}`, createFetchOptions<IApiResponse<IUser>>()), 'getUserBySsoId')
  }

  const getUserByEmail = (email: string) => {
    return handleApiCall($fetch<IApiResponse<IUser>>(`${baseUrl}/email/${email}`, createFetchOptions<IApiResponse<IUser>>()), 'getUserByEmail')
  }

  const getAllUsers = () => {
    return handleApiCall($fetch<IApiResponse<IUser[]>>(`${baseUrl}`, createFetchOptions<IApiResponse<IUser[]>>()), 'getAllUsers')
  }

  const updateUser = (userId: string, request: IUpdateUserRequest) => {
    return handleApiCall(
      $fetch<IApiResponse<IUser>>(`${baseUrl}/${userId}`, {
        ...createFetchOptions<IApiResponse<IUser>>(),
        method: 'PUT',
        body: request,
      }),
      'updateUser',
    )
  }

  const deleteUser = (userId: string) => {
    return handleApiCall(
      $fetch<IApiResponse<boolean>>(`${baseUrl}/${userId}`, {
        ...createFetchOptions<IApiResponse<boolean>>(),
        method: 'DELETE',
      }),
      'deleteUser',
    )
  }

  const getUserPermissions = (userId: string) => {
    return handleApiCall($fetch<IApiResponse<IPermission[]>>(`${baseUrl}/${userId}/permissions`, createFetchOptions<IApiResponse<IPermission[]>>()), 'getUserPermissions')
  }

  const searchUsers = (searchTerm: string) => {
    return handleApiCall(
      $fetch<IApiResponse<IUser[]>>(`${baseUrl}/search?searchTerm=${encodeURIComponent(searchTerm)}`, createFetchOptions<IApiResponse<IUser[]>>()),
      'searchUsers',
    )
  }

  const getUsersWithEntityPermissions = (entityId: string, entityType: EntityType) => {
    return handleApiCall(
      $fetch<IApiResponse<IUser[]>>(`${baseUrl}/entity/${entityType}/${entityId}/permissions`, createFetchOptions<IApiResponse<IUser[]>>()),
      'getUsersWithEntityPermissions',
    )
  }

  return {
    getOrCreateUser,
    createUser,
    getUserById,
    getUserBySsoId,
    getUserByEmail,
    getAllUsers,
    updateUser,
    deleteUser,
    getUserPermissions,
    searchUsers,
    getUsersWithEntityPermissions,
  }
}
