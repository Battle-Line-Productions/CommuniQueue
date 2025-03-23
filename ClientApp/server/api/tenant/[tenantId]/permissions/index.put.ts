import { defineEventHandler, readBody } from 'h3'
import type { IApiResponse, IPermission, IUpdatePermissionRequest } from '../../../../../app/types'
import { withAuthCheck } from '../../../../utils/withAuthCheck'

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()
    const { tenantId } = event.context.params as { tenantId: string }
    const body = await readBody<IUpdatePermissionRequest>(event)

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<IPermission>>(
            `${config.apiBaseUrl}/api/v1/tenant/${tenantId}/permissions`,
            {
                method: 'PUT',
                headers,
                body,
            }
        )
        return res
    })
})
