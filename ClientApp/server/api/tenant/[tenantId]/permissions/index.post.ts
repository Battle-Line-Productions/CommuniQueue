import { defineEventHandler, readBody } from 'h3'
import type { IApiResponse, IPermission, ICreatePermissionRequest } from '../../../../../app/types'
import { withAuthCheck } from '../../../../utils/withAuthCheck'

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()
    const { tenantId } = event.context.params as { tenantId: string }

    const body = await readBody<ICreatePermissionRequest>(event)

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<IPermission>>(
            `${config.apiBaseUrl}/api/v1/tenant/${tenantId}/permissions`,
            {
                method: 'POST',
                headers,
                body,
            }
        )
        return res
    })
})
