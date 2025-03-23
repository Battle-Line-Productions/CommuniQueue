// ~/server/api/tenants/user/[ssoUserId].get.ts
import { defineEventHandler } from 'h3'
import type { IAppTenantInfo, IApiResponse } from '../../../../app/types'
import { withAuthCheck } from '../../../utils/withAuthCheck'

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()
    const { ssoUserId } = event.context.params as { ssoUserId: string }

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<IAppTenantInfo[]>>(
            `${config.apiBaseUrl}/api/v1/tenants/user/${ssoUserId}`,
            {
                method: 'GET',
                headers,
            }
        )
        return res
    })
})
