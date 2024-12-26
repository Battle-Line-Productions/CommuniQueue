import type { ITemplate, ITemplateStageAssignment } from '~/types'

export interface ITemplateVersion {
  id: string // Guid in C#
  versionNumber: number
  subject: string
  body: string
  templateId: string // Guid in C#
  template?: ITemplate
  stageAssignments: ITemplateStageAssignment[]
  createdDateTime: Date
  updatedDateTime: Date
}
