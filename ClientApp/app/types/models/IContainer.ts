import type { IProject, ITemplate } from '~/types'

export interface IContainer {
  id: string // Guid in C#
  name: string
  description: string | null
  projectId: string // Guid in C#
  parentId: string | null // Guid? in C#
  parent?: IContainer
  children: IContainer[]
  project?: IProject
  templates: ITemplate[]
  isRoot: boolean
  createdDateTime: Date
  updatedDateTime: Date
}
