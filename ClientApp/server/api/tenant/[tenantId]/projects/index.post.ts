import { defineEventHandler, readBody } from 'h3'
import type {
    IApiResponse,
    IProject,
    ICreateProjectRequest
} from '../../../../../app/types'
import { withAuthCheck } from '../../../../utils/withAuthCheck' // adjust path as needed

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()
    const { tenantId } = event.context.params as { tenantId: string }
    const body = await readBody<ICreateProjectRequest>(event)

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<IProject>>(
            `${config.apiBaseUrl}/api/v1/tenant/${tenantId}/projects`,
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
