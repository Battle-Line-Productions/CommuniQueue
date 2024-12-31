import type {
  IApiResponse,
  ITemplate,
  ITemplateVersion,
  ICreateTemplateRequest,
  ICreateVersionRequest,
  IAssignVersionToStageRequest,
  IRemoveVersionFromStageRequest,
} from '~/types'

export default function useTemplates() {
  const config = useRuntimeConfig()
  const tenant = useTenant()

  const baseUrl = `${config.public.apiBaseUrl}/api/v1/tenant/${tenant.currentTenantId.value}templates`
  const { handleApiCall, createFetchOptions } = useApiUtils()

  const createTemplate = (request: ICreateTemplateRequest) => {
    return handleApiCall(
      $fetch<IApiResponse<ITemplate>>(`${baseUrl}`, {
        ...createFetchOptions<IApiResponse<ITemplate>>(),
        method: 'POST',
        body: request,
      }),
      'createTemplate',
    )
  }

  const getTemplateById = (templateId: string) => {
    return handleApiCall($fetch<IApiResponse<ITemplate>>(`${baseUrl}/${templateId}`, createFetchOptions<IApiResponse<ITemplate>>()), 'getTemplateById')
  }

  const getTemplatesByProjectId = (projectId: string) => {
    return handleApiCall($fetch<IApiResponse<ITemplate[]>>(`${baseUrl}/project/${projectId}`, createFetchOptions<IApiResponse<ITemplate[]>>()), 'getTemplatesByProjectId')
  }

  const getTemplatesByContainerId = (containerId: string) => {
    return handleApiCall($fetch<IApiResponse<ITemplate[]>>(`${baseUrl}/container/${containerId}`, createFetchOptions<IApiResponse<ITemplate[]>>()), 'getTemplatesByContainerId')
  }

  const getLatestVersion = (templateId: string) => {
    return handleApiCall(
      $fetch<IApiResponse<ITemplateVersion>>(`${baseUrl}/${templateId}/latest-version`, createFetchOptions<IApiResponse<ITemplateVersion>>()),
      'getLatestVersion',
    )
  }

  const getAllVersions = (templateId: string) => {
    return handleApiCall($fetch<IApiResponse<ITemplateVersion[]>>(`${baseUrl}/${templateId}/versions`, createFetchOptions<IApiResponse<ITemplateVersion[]>>()), 'getAllVersions')
  }

  const createNewVersion = (templateId: string, request: ICreateVersionRequest) => {
    return handleApiCall(
      $fetch<IApiResponse<ITemplateVersion>>(`${baseUrl}/${templateId}/versions`, {
        ...createFetchOptions<IApiResponse<ITemplateVersion>>(),
        method: 'POST',
        body: request,
      }),
      'createNewVersion',
    )
  }

  const assignVersionToStage = (templateVersionId: string, request: IAssignVersionToStageRequest) => {
    return handleApiCall(
      $fetch<IApiResponse<boolean>>(`${baseUrl}/versions/${templateVersionId}/assign`, {
        ...createFetchOptions<IApiResponse<boolean>>(),
        method: 'POST',
        body: request,
      }),
      'assignVersionToStage',
    )
  }

  const removeVersionFromStage = (templateVersionId: string, request: IRemoveVersionFromStageRequest) => {
    return handleApiCall(
      $fetch<IApiResponse<boolean>>(`${baseUrl}/versions/${templateVersionId}/unassign`, {
        ...createFetchOptions<IApiResponse<boolean>>(),
        method: 'DELETE',
        body: request,
      }),
      'removeVersionFromStage',
    )
  }

  const getVersionAssignedToStage = (templateId: string, stageId: string) => {
    return handleApiCall(
      $fetch<IApiResponse<ITemplateVersion>>(`${baseUrl}/${templateId}/stage/${stageId}`, createFetchOptions<IApiResponse<ITemplateVersion>>()),
      'getVersionAssignedToStage',
    )
  }

  return {
    createTemplate,
    getTemplateById,
    getTemplatesByProjectId,
    getTemplatesByContainerId,
    getLatestVersion,
    getAllVersions,
    createNewVersion,
    assignVersionToStage,
    removeVersionFromStage,
    getVersionAssignedToStage,
  }
}
