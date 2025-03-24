import { defineEventHandler, readBody } from 'h3'
import type { IApiResponse, IAssignVersionToStageRequest } from '../../../../../../../app/types'
import { withAuthCheck } from '../../../../../../utils/withAuthCheck'

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()
    const { tenantId, templateVersionId } = event.context.params as {
        tenantId: string
        templateVersionId: string
    }

    const body = await readBody<IAssignVersionToStageRequest>(event)

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<boolean>>(
            `${config.apiBaseUrl}/api/v1/tenant/${tenantId}/templates/versions/${templateVersionId}/assign`,
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
