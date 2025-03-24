import { defineEventHandler } from 'h3'
import type { IApiResponse, IUser } from '../../../app/types'
import { withAuthCheck } from '../../utils/withAuthCheck'

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<IUser[]>>(`${config.apiBaseUrl}/api/v1/users`, {
            method: 'GET',
            headers,
            credentials: 'include',
        })

        return res
    })
})
