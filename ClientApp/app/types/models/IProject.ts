import type { IContainer, IPermission, IStage, ITemplate } from '~/types'

export interface IProject {
  id: string // Guid in C#
  name: string
  description: string | null
  customerId: string | null
  rootContainerId: string // Guid in C#
  rootContainer: IContainer
  stages: IStage[]
  containers: IContainer[]
  templates: ITemplate[]
  permissions: IPermission[]
  createdDateTime: Date
  updatedDateTime: Date
}
