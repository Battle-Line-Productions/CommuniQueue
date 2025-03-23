import { defineEventHandler, readBody } from 'h3'
import type { IApiResponse, ITemplate, ICreateTemplateRequest } from '../../../../../app/types'
import { withAuthCheck } from '../../../../utils/withAuthCheck'

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()
    const { tenantId } = event.context.params as { tenantId: string }

    const body = await readBody<ICreateTemplateRequest>(event)

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<ITemplate>>(
            `${config.apiBaseUrl}/api/v1/tenant/${tenantId}/templates`,
            {
                method: 'POST',
                headers,
                body,
            }
        )
        return res
    })
})
