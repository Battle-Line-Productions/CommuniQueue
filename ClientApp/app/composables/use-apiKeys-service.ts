import type { IApiKey, IApiResponse, IGenerateApiKeyRequest, IValidateApiKeyRequest } from '~/types'

export default function useApiKeys() {
  const config = useRuntimeConfig()
  const tenant = useTenant()

  const baseUrl = `${config.public.apiBaseUrl}/api/v1/tenant/${tenant.currentTenantId.value}apikeys`
  const { handleApiCall, createFetchOptions } = useApiUtils()

  const generateApiKey = (request: IGenerateApiKeyRequest) => {
    return handleApiCall(
      $fetch<IApiResponse<{ apiKey: IApiKey, apiKeyString: string }>>(`${baseUrl}/generate`, {
        ...createFetchOptions<IApiResponse<{ apiKey: IApiKey, apiKeyString: string }>>(),
        method: 'POST',
        body: request,
      }),
      'generateApiKey',
    )
  }

  const getApiKeyById = (apiKeyId: string) => {
    return handleApiCall($fetch<IApiResponse<IApiKey>>(`${baseUrl}/${apiKeyId}`, createFetchOptions<IApiResponse<IApiKey>>()), 'getApiKeyById')
  }

  const getApiKeysByProjectId = (projectId: string) => {
    return handleApiCall($fetch<IApiResponse<IApiKey[]>>(`${baseUrl}/project/${projectId}`, createFetchOptions<IApiResponse<IApiKey[]>>()), 'getApiKeysByProjectId')
  }

  const validateApiKey = (request: IValidateApiKeyRequest) => {
    return handleApiCall(
      $fetch<IApiResponse<boolean>>(`${baseUrl}/validate`, {
        ...createFetchOptions<IApiResponse<boolean>>(),
        method: 'POST',
        body: request,
      }),
      'validateApiKey',
    )
  }

  const expireApiKey = (apiKeyId: string) => {
    return handleApiCall(
      $fetch<IApiResponse<boolean>>(`${baseUrl}/expire/${apiKeyId}`, {
        ...createFetchOptions<IApiResponse<boolean>>(),
        method: 'POST',
      }),
      'expireApiKey',
    )
  }

  const hasValidApiKey = (projectId: string) => {
    return handleApiCall($fetch<IApiResponse<boolean>>(`${baseUrl}/valid/${projectId}`, createFetchOptions<IApiResponse<boolean>>()), 'hasValidApiKey')
  }

  return {
    generateApiKey,
    getApiKeyById,
    getApiKeysByProjectId,
    validateApiKey,
    expireApiKey,
    hasValidApiKey,
  }
}
