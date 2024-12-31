import type { IUserTenantMembership } from '~/types'

export interface IAppTenantInfo {
  ownerUserId: string // Guid in C#
  id: string // string in C#
  identifier: string
  name: string
  description?: string | null
  createdDateTime: Date
  updatedDateTime: Date
  userTenantMemberships: IUserTenantMembership[]
}
