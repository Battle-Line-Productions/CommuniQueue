import { defineEventHandler } from 'h3'
import type { IApiResponse, IStage } from '../../../../../../app/types'
import { withAuthCheck } from '../../../../../utils/withAuthCheck'

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()
    const { tenantId, projectId } = event.context.params as {
        tenantId: string
        projectId: string
    }

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<IStage[]>>(
            `${config.apiBaseUrl}/api/v1/tenant/${tenantId}/projects/${projectId}/stages`,
            {
                method: 'GET',
                headers,
            }
        )
        return res
    })
})
