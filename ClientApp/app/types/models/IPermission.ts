import type { EntityType, IProject, IUser, PermissionLevel } from '~/types'

export interface IPermission {
  id: string // Guid in C#
  userId: string // Guid in C#
  entityId: string // Guid in C#
  entityType: EntityType
  permissionLevel: PermissionLevel
  user?: IUser
  project?: IProject | null
  createdDateTime: Date
  updatedDateTime: Date
}
