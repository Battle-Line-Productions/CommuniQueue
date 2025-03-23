import { defineEventHandler } from 'h3'
import type { IApiResponse, IUser } from '../../../../app/types'
import { withAuthCheck } from '../../../utils/withAuthCheck'

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()
    const { ssoId } = event.context.params as { ssoId: string }

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<IUser>>(`${config.apiBaseUrl}/api/v1/users/sso/${ssoId}`, {
            method: 'GET',
            headers,
        })
        return res
    })
})
