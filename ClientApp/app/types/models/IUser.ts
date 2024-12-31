import type { IPermission } from '~/types'

export interface IUser {
  id: string // Guid in C#
  createdDateTime: Date
  updatedDateTime: Date
  email: string
  ssoId: string
  firstName: string
  lastName: string
  isActive: boolean
  permissions?: IPermission[]
}
