import type { PermissionLevel } from '~/types'

export interface IAddUserToProjectRequest {
  userId: string
  permissionLevel: PermissionLevel
}
