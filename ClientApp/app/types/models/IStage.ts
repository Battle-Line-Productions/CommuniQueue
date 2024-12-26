import type { IProject } from '~/types'

export interface IStage {
  id: string // Guid in C#
  name: string
  order: number
  projectId: string // Guid in C#
  project?: IProject
  createdDateTime: Date
  updatedDateTime: Date
}
