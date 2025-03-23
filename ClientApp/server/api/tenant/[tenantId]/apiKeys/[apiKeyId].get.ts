import { defineEventHandler } from 'h3'
import type { IApiResponse, IApiKey } from '../../../../../app/types'
import { withAuthCheck } from '../../../../utils/withAuthCheck'

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()
    const { tenantId, apiKeyId } = event.context.params as {
        tenantId: string
        apiKeyId: string
    }

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<IApiKey>>(
            `${config.apiBaseUrl}/api/v1/tenant/${tenantId}/apikeys/${apiKeyId}`,
            {
                method: 'GET',
                headers,
            }
        )
        return res
    })
})
