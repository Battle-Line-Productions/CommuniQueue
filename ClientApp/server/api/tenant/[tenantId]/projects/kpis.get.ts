import { defineEventHandler, getQuery } from 'h3'
import type { IApiResponse, IProjectKpis } from '../../../../../app/types'
import { withAuthCheck } from '../../../../utils/withAuthCheck'

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()
    const { tenantId } = event.context.params as { tenantId: string }
    const query = getQuery(event)

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<IProjectKpis>>(
            `${config.apiBaseUrl}/api/v1/tenant/${tenantId}/projects/kpis`,
            {
                method: 'GET',
                headers,
                query,
                credentials: 'include',
            }
        )
        return res
    })
})
