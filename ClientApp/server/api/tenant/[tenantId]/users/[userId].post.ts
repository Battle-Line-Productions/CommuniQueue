import { defineEventHandler } from 'h3'
import type { IApiResponse, IAppTenantInfo } from '../../../../../app/types'
import { withAuthCheck } from '../../../../utils/withAuthCheck'

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()

    const { tenantId, userId } = event.context.params as { tenantId: string; userId: string }

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<IAppTenantInfo>>(
            `${config.apiBaseUrl}/api/v1/tenant/${tenantId}/users/${userId}`,
            {
                method: 'POST',
                headers,
            }
        )

        return res
    })
})
