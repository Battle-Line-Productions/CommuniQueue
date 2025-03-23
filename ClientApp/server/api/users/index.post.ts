import { defineEventHandler, readBody } from 'h3'
import type { ICreateUserRequest, IApiResponse, IUser } from '../../../app/types'
import { withAuthCheck } from '../../utils/withAuthCheck'

export default defineEventHandler(async (event) => {
  const config = useRuntimeConfig()
  const body = await readBody<ICreateUserRequest>(event)

  return withAuthCheck(async (headers) => {
    const res = await $fetch<IApiResponse<IUser>>(`${config.apiBaseUrl}/api/v1/users`, {
      method: 'POST',
      headers,
      body,
    })
    return res
  })
})
