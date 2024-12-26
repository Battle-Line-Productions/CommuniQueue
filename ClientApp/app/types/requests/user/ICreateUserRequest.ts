import type { GlobalRoleType } from '~/types'

export type ICreateUserRequest = {
  email: string
  ssoId: string
  isActive: boolean
  globalRole: GlobalRoleType
  firstName: string
  lastName: string
}
