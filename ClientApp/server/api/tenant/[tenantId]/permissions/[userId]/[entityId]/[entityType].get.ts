import { defineEventHandler } from 'h3'
import type { IApiResponse, IPermission } from '../../../../../../../app/types'
import { withAuthCheck } from '../../../../../../utils/withAuthCheck'

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()
    const { tenantId, userId, entityId, entityType } = event.context.params as {
        tenantId: string
        userId: string
        entityId: string
        entityType: string
    }

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<IPermission>>(
            `${config.apiBaseUrl}/api/v1/tenant/${tenantId}/permissions/${userId}/${entityId}/${entityType}`,
            {
                method: 'GET',
                headers,
                credentials: 'include',
            }
        )
        return res
    })
})
