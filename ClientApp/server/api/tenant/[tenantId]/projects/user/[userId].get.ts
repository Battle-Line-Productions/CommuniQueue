import { defineEventHandler } from 'h3'
import type { IApiResponse, IProject } from '../../../../../../app/types'
import { withAuthCheck } from '../../../../../utils/withAuthCheck'

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()
    const { tenantId, userId } = event.context.params as {
        tenantId: string
        userId: string
    }

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<IProject[]>>(
            `${config.apiBaseUrl}/api/v1/tenant/${tenantId}/projects/user/${userId}`,
            {
                method: 'GET',
                headers,
            }
        )
        return res
    })
})
