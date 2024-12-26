import type { IContainer, IProject, ITemplateVersion } from '~/types'

export interface ITemplate {
  id: string // Guid in C#
  name: string
  projectId: string // Guid in C#
  project?: IProject
  containerId: string // Guid in C#
  container?: IContainer
  versions: ITemplateVersion[]
  createdDateTime: Date
  updatedDateTime: Date
}
