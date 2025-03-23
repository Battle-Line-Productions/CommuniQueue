import { defineEventHandler } from 'h3'
import type { IApiResponse, IPermission } from '../../../../app/types'
import { withAuthCheck } from '../../../utils/withAuthCheck'

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()
    const { userId } = event.context.params as { userId: string }

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<IPermission[]>>(`${config.apiBaseUrl}/api/v1/users/${userId}/permissions`, {
            method: 'GET',
            headers,
        })
        return res
    })
})
