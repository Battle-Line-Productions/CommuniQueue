export interface IGenerateApiKeyRequest {
  projectId: string
  startDate: string
  endDate: string
  scopes: string[]
}
