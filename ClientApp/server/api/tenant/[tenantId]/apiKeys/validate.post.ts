import { defineEventHandler, readBody } from 'h3'
import type { IApiResponse, IValidateApiKeyRequest } from '../../../../../app/types'
import { withAuthCheck } from '../../../../utils/withAuthCheck'

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()
    const { tenantId } = event.context.params as { tenantId: string }
    const body = await readBody<IValidateApiKeyRequest>(event)

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<boolean>>(
            `${config.apiBaseUrl}/api/v1/tenant/${tenantId}/apikeys/validate`,
            {
                method: 'POST',
                headers,
                body,
            }
        )
        return res
    })
})
