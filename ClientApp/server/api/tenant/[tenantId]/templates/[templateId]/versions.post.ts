import { defineEventHandler, readBody } from 'h3'
import type { IApiResponse, ITemplateVersion, ICreateVersionRequest } from '../../../../../../app/types'
import { withAuthCheck } from '../../../../../utils/withAuthCheck'

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()
    const { tenantId, templateId } = event.context.params as { tenantId: string; templateId: string }

    const body = await readBody<ICreateVersionRequest>(event)

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<ITemplateVersion>>(
            `${config.apiBaseUrl}/api/v1/tenant/${tenantId}/templates/${templateId}/versions`,
            {
                method: 'POST',
                headers,
                body,
                credentials: 'include',
            }
        )
        return res
    })
})
