// ~/server/api/tenants/index.post.ts
import { defineEventHandler, readBody } from 'h3'
import type { ICreateTenantRequest, IAppTenantInfo, IApiResponse } from '../../../app/types'
import { withAuthCheck } from '../../utils/withAuthCheck'

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()

    const body = await readBody<ICreateTenantRequest>(event)

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<IAppTenantInfo>>(`${config.apiBaseUrl}/api/v1/tenants`, {
            method: 'POST',
            headers,
            body,
        })
        return res
    })
})
