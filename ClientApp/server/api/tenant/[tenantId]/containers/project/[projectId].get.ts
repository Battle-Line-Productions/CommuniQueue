import { defineEventHandler } from 'h3'
import type { IApiResponse, IContainer } from '../../../../../../app/types'
import { withAuthCheck } from '../../../../../utils/withAuthCheck'

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()
    const { tenantId, projectId } = event.context.params as {
        tenantId: string
        projectId: string
    }

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<IContainer[]>>(
            `${config.apiBaseUrl}/api/v1/tenant/${tenantId}/containers/project/${projectId}`,
            {
                method: 'GET',
                headers,
                credentials: 'include',
            }
        )
        return res
    })
})
