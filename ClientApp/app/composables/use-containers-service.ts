import type { IApiResponse, IContainer, ICreateContainerRequest, IUpdateContainerRequest, IMoveContainerRequest } from '~/types'

export default function useContainers() {
  const config = useRuntimeConfig()
  const baseUrl = `${config.public.apiBaseUrl}/api/v1/containers`
  const { handleApiCall, createFetchOptions } = useApiUtils()

  const createContainer = (request: ICreateContainerRequest) => {
    return handleApiCall(
      $fetch<IApiResponse<IContainer>>(`${baseUrl}`, {
        ...createFetchOptions<IApiResponse<IContainer>>(),
        method: 'POST',
        body: request,
      }),
      'createContainer',
    )
  }

  const getContainerById = (containerId: string) => {
    return handleApiCall($fetch<IApiResponse<IContainer>>(`${baseUrl}/${containerId}`, createFetchOptions<IApiResponse<IContainer>>()), 'getContainerById')
  }

  const getContainersByProjectId = (projectId: string) => {
    return handleApiCall($fetch<IApiResponse<IContainer[]>>(`${baseUrl}/project/${projectId}`, createFetchOptions<IApiResponse<IContainer[]>>()), 'getContainersByProjectId')
  }

  const updateContainer = (containerId: string, request: IUpdateContainerRequest) => {
    return handleApiCall(
      $fetch<IApiResponse<IContainer>>(`${baseUrl}/${containerId}`, {
        ...createFetchOptions<IApiResponse<IContainer>>(),
        method: 'PUT',
        body: request,
      }),
      'updateContainer',
    )
  }

  const deleteContainer = (containerId: string) => {
    return handleApiCall(
      $fetch<IApiResponse<boolean>>(`${baseUrl}/${containerId}`, {
        ...createFetchOptions<IApiResponse<boolean>>(),
        method: 'DELETE',
      }),
      'deleteContainer',
    )
  }

  const getChildContainers = (parentContainerId: string) => {
    return handleApiCall($fetch<IApiResponse<IContainer[]>>(`${baseUrl}/${parentContainerId}/children`, createFetchOptions<IApiResponse<IContainer[]>>()), 'getChildContainers')
  }

  const moveContainer = (containerId: string, request: IMoveContainerRequest) => {
    return handleApiCall(
      $fetch<IApiResponse<boolean>>(`${baseUrl}/${containerId}/move`, {
        ...createFetchOptions<IApiResponse<boolean>>(),
        method: 'PUT',
        body: request,
      }),
      'moveContainer',
    )
  }

  return {
    createContainer,
    getContainerById,
    getContainersByProjectId,
    updateContainer,
    deleteContainer,
    getChildContainers,
    moveContainer,
  }
}
