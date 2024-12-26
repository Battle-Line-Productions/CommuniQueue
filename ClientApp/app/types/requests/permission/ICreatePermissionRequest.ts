import type { EntityType, PermissionLevel } from '~/types'

export interface ICreatePermissionRequest {
  userId: string
  entityId: string
  entityType: EntityType
  permissionLevel: PermissionLevel
}
