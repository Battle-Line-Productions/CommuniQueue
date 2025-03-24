import { defineEventHandler } from 'h3'
import type { IApiResponse, ITemplateVersion } from '../../../../../../app/types'
import { withAuthCheck } from '../../../../../utils/withAuthCheck'

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()
    const { tenantId, templateId } = event.context.params as { tenantId: string; templateId: string }

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<ITemplateVersion[]>>(
            `${config.apiBaseUrl}/api/v1/tenant/${tenantId}/templates/${templateId}/versions`,
            {
                method: 'GET',
                headers,
                credentials: 'include',
            }
        )
        return res
    })
})
