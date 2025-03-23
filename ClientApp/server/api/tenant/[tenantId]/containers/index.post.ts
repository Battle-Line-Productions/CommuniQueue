import { defineEventHandler, readBody } from 'h3'
import type {
    IApiResponse,
    IContainer,
    ICreateContainerRequest
} from '../../../../../app/types'
import { withAuthCheck } from '../../../../utils/withAuthCheck'

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()
    const { tenantId } = event.context.params as { tenantId: string }

    const body = await readBody<ICreateContainerRequest>(event)

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<IContainer>>(
            `${config.apiBaseUrl}/api/v1/tenant/${tenantId}/containers`,
            {
                method: 'POST',
                headers,
                body,
            }
        )
        return res
    })
})
