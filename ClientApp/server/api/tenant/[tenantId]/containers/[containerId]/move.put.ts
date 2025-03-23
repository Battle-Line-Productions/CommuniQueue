import { defineEventHandler, readBody } from 'h3'
import type { IApiResponse, IMoveContainerRequest } from '../../../../../../app/types'
import { withAuthCheck } from '../../../../../utils/withAuthCheck'

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()
    const { tenantId, containerId } = event.context.params as {
        tenantId: string
        containerId: string
    }

    const body = await readBody<IMoveContainerRequest>(event)

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<boolean>>(
            `${config.apiBaseUrl}/api/v1/tenant/${tenantId}/containers/${containerId}/move`,
            {
                method: 'PUT',
                headers,
                body,
            }
        )
        return res
    })
})
