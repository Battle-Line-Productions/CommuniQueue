import type {
  IApiResponse,
  IProject,
  IStage,
  ICreateProjectRequest,
  IUpdateProjectRequest,
  IAddUserToProjectRequest,
  IUpdateUserPermissionRequest,
  IAddStageToProjectRequest,
  IUpdateProjectStageRequest,
  IProjectKpis,
} from '~/types'

export default function useProjects() {
  const config = useRuntimeConfig()
  const { handleApiCall, createFetchOptions } = useApiUtils()

  const tenant = useTenant()

  const baseUrl = `${config.public.apiBaseUrl}/api/v1/tenant/${tenant.currentTenantId.value}/projects`

  const createProject = (request: ICreateProjectRequest) => {
    return handleApiCall(
      $fetch<IApiResponse<IProject>>(`${baseUrl}`, {
        ...createFetchOptions<IApiResponse<IProject>>(),
        method: 'POST',
        body: request,
      }),
      'createProject',
    )
  }

  const getProjectKpis = (projectId: string) => {
    return handleApiCall($fetch<IApiResponse<IProjectKpis>>(`${baseUrl}/kpis?${projectId}`, createFetchOptions<IApiResponse<IProjectKpis>>()), 'getProjectKpis')
  }

  const getProjectById = (projectId: string) => {
    return handleApiCall($fetch<IApiResponse<IProject>>(`${baseUrl}/${projectId}`, createFetchOptions<IApiResponse<IProject>>()), 'getProjectById')
  }

  const getProjectsByUserId = (userId: string) => {
    return handleApiCall($fetch<IApiResponse<IProject[]>>(`${baseUrl}/user/${userId}`, createFetchOptions<IApiResponse<IProject[]>>()), 'getProjectsByUserId')
  }

  const updateProject = (projectId: string, request: IUpdateProjectRequest) => {
    return handleApiCall(
      $fetch<IApiResponse<IProject>>(`${baseUrl}/${projectId}`, {
        ...createFetchOptions<IApiResponse<IProject>>(),
        method: 'PUT',
        body: request,
      }),
      'updateProject',
    )
  }

  const deleteProject = (projectId: string) => {
    return handleApiCall(
      $fetch<IApiResponse<boolean>>(`${baseUrl}/${projectId}`, {
        ...createFetchOptions<IApiResponse<boolean>>(),
        method: 'DELETE',
      }),
      'deleteProject',
    )
  }

  const addUserToProject = (projectId: string, request: IAddUserToProjectRequest) => {
    return handleApiCall(
      $fetch<IApiResponse<boolean>>(`${baseUrl}/${projectId}/users`, {
        ...createFetchOptions<IApiResponse<boolean>>(),
        method: 'POST',
        body: request,
      }),
      'addUserToProject',
    )
  }

  const removeUserFromProject = (projectId: string, userId: string) => {
    return handleApiCall(
      $fetch<IApiResponse<boolean>>(`${baseUrl}/${projectId}/users/${userId}`, {
        ...createFetchOptions<IApiResponse<boolean>>(),
        method: 'DELETE',
      }),
      'removeUserFromProject',
    )
  }

  const updateUserPermissionInProject = (projectId: string, userId: string, request: IUpdateUserPermissionRequest) => {
    return handleApiCall(
      $fetch<IApiResponse<boolean>>(`${baseUrl}/${projectId}/users/${userId}/permission`, {
        ...createFetchOptions<IApiResponse<boolean>>(),
        method: 'PUT',
        body: request,
      }),
      'updateUserPermissionInProject',
    )
  }

  const getProjectStages = (projectId: string) => {
    return handleApiCall($fetch<IApiResponse<IStage[]>>(`${baseUrl}/${projectId}/stages`, createFetchOptions<IApiResponse<IStage[]>>()), 'getProjectStages')
  }

  const addStageToProject = (projectId: string, request: IAddStageToProjectRequest) => {
    return handleApiCall(
      $fetch<IApiResponse<IStage>>(`${baseUrl}/${projectId}/stages`, {
        ...createFetchOptions<IApiResponse<IStage>>(),
        method: 'POST',
        body: request,
      }),
      'addStageToProject',
    )
  }

  const removeStageFromProject = (projectId: string, stageId: string) => {
    return handleApiCall(
      $fetch<IApiResponse<boolean>>(`${baseUrl}/${projectId}/stages/${stageId}`, {
        ...createFetchOptions<IApiResponse<boolean>>(),
        method: 'DELETE',
      }),
      'removeStageFromProject',
    )
  }

  const updateProjectStage = (projectId: string, stageId: string, request: IUpdateProjectStageRequest) => {
    return handleApiCall(
      $fetch<IApiResponse<IStage>>(`${baseUrl}/${projectId}/stages/${stageId}`, {
        ...createFetchOptions<IApiResponse<IStage>>(),
        method: 'PUT',
        body: request,
      }),
      'updateProjectStage',
    )
  }

  return {
    createProject,
    getProjectKpis,
    getProjectById,
    getProjectsByUserId,
    updateProject,
    deleteProject,
    addUserToProject,
    removeUserFromProject,
    updateUserPermissionInProject,
    getProjectStages,
    addStageToProject,
    removeStageFromProject,
    updateProjectStage,
  }
}
