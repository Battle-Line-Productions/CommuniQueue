// ~/server/api/tenants/[tenantId].put.ts
import { defineEventHandler, readBody } from 'h3'
import type { IUpdateTenantRequest, IAppTenantInfo, IApiResponse } from '../../../app/types'
import { withAuthCheck } from '../../utils/withAuthCheck'

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()
    const { tenantId } = event.context.params as { tenantId: string }
    const body = await readBody<IUpdateTenantRequest>(event)

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<IAppTenantInfo>>(`${config.apiBaseUrl}/api/v1/tenants/${tenantId}`, {
            method: 'PUT',
            headers,
            body,
            credentials: 'include',
        })
        return res
    })
})
