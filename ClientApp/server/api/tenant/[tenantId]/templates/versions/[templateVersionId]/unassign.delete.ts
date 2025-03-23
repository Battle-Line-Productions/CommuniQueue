import { defineEventHandler, readBody } from 'h3'
import type { IApiResponse, IRemoveVersionFromStageRequest } from '../../../../../../../app/types'
import { withAuthCheck } from '../../../../../../utils/withAuthCheck'

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()
    const { tenantId, templateVersionId } = event.context.params as {
        tenantId: string
        templateVersionId: string
    }

    const body = await readBody<IRemoveVersionFromStageRequest>(event)

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<boolean>>(
            `${config.apiBaseUrl}/api/v1/tenant/${tenantId}/templates/versions/${templateVersionId}/unassign`,
            {
                method: 'DELETE',
                headers,
                body,
            }
        )
        return res
    })
})
