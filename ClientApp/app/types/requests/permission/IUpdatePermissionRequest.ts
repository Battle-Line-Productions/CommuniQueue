import type { EntityType, PermissionLevel } from '~/types'

export interface IUpdatePermissionRequest {
  userId: string
  entityId: string
  entityType: EntityType
  newPermissionLevel: PermissionLevel
}
