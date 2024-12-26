import type { IProject } from '~/types'

export interface IApiKey {
  id: string // Guid in C#
  keyHash: string
  projectId: string // Guid in C#
  project?: IProject
  createdDateTime: Date
  updatedDateTime: Date
  startDate: Date
  endDate: Date
  isExpired: boolean
  scopes: string[]
}
