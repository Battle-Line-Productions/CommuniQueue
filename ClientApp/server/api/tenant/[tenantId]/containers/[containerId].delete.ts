import { defineEventHandler } from 'h3'
import type { IApiResponse } from '../../../../../app/types'
import { withAuthCheck } from '../../../../utils/withAuthCheck'

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()
    const { tenantId, containerId } = event.context.params as {
        tenantId: string
        containerId: string
    }

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<boolean>>(
            `${config.apiBaseUrl}/api/v1/tenant/${tenantId}/containers/${containerId}`,
            {
                method: 'DELETE',
                headers,
            }
        )
        return res
    })
})
