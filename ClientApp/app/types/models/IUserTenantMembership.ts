import type { IAppTenantInfo, IUser, GlobalRoleType } from '~/types'

export interface IUserTenantMembership {
  id: string // Guid in C#
  userId: string // Guid in C#
  tenantId: string
  createdDateTime: Date
  updatedDateTime: Date
  user?: IUser
  globalRole: GlobalRoleType
  tenant?: IAppTenantInfo
}
