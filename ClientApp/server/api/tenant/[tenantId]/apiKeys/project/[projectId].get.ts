import { defineEventHandler } from 'h3'
import type { IApiResponse, IApiKey } from '../../../../../../app/types'
import { withAuthCheck } from '../../../../../utils/withAuthCheck'

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()
    const { tenantId, projectId } = event.context.params as {
        tenantId: string
        projectId: string
    }

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<IApiKey[]>>(
            `${config.apiBaseUrl}/api/v1/tenant/${tenantId}/apikeys/project/${projectId}`,
            {
                method: 'GET',
                headers,
            }
        )
        return res
    })
})
