import type { IStage, ITemplateVersion } from '~/types'

export interface ITemplateStageAssignment {
  id: string // Guid in C#
  templateVersionId: string // Guid in C#
  templateVersion?: ITemplateVersion
  stageId: string // Guid in C#
  stage?: IStage
  createdDateTime: Date
  updatedDateTime: Date
}
