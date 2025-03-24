import { defineEventHandler } from 'h3'
import type { IApiResponse } from '../../../../../../../app/types'
import { withAuthCheck } from '../../../../../../utils/withAuthCheck'

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()
    const { tenantId, projectId, stageId } = event.context.params as {
        tenantId: string
        projectId: string
        stageId: string
    }

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<boolean>>(
            `${config.apiBaseUrl}/api/v1/tenant/${tenantId}/projects/${projectId}/stages/${stageId}`,
            {
                method: 'DELETE',
                headers,
                credentials: 'include',
            }
        )
        return res
    })
})
