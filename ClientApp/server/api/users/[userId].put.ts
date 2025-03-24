import { defineEventHandler, readBody } from 'h3'
import type { IApiResponse, IUser, IUpdateUserRequest } from '../../../app/types'
import { withAuthCheck } from '../../utils/withAuthCheck'

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()
    const { userId } = event.context.params as { userId: string }
    const body = await readBody<IUpdateUserRequest>(event)

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<IUser>>(`${config.apiBaseUrl}/api/v1/users/${userId}`, {
            method: 'PUT',
            headers,
            body,
            credentials: 'include',
        })
        return res
    })
})
