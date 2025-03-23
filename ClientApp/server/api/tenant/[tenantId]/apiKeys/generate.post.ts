import { defineEventHandler, readBody } from 'h3'
import type {
    IApiResponse,
    IGenerateApiKeyRequest,
    IApiKey
} from '../../../../../app/types'
import { withAuthCheck } from '../../../../utils/withAuthCheck'

type GenerateApiKeyResponse = [IApiKey | null, string | null]

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()
    const { tenantId } = event.context.params as { tenantId: string }

    const body = await readBody<IGenerateApiKeyRequest>(event)

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<GenerateApiKeyResponse>>(
            `${config.apiBaseUrl}/api/v1/tenant/${tenantId}/apikeys/generate`,
            {
                method: 'POST',
                headers,
                body,
            }
        )
        return res
    })
})
