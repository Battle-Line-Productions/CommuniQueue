import { defineEventHandler } from 'h3'
import type { IApiResponse } from '../../../../../../../app/types'
import { withAuthCheck } from '../../../../../../utils/withAuthCheck'

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()
    const { tenantId, projectId, userId } = event.context.params as {
        tenantId: string
        projectId: string
        userId: string
    }

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<boolean>>(
            `${config.apiBaseUrl}/api/v1/tenant/${tenantId}/projects/${projectId}/users/${userId}`,
            {
                method: 'DELETE',
                headers,
            }
        )
        return res
    })
})
