import type { GlobalRoleType } from '~/types'

export type IUpdateUserRequest = {
  email: string
  isActive: boolean
  globalRole: GlobalRoleType
  firstName: string
  lastName: string
}
