import { defineEventHandler, readBody } from 'h3'
import type {
    IApiResponse,
    IProject,
    IUpdateProjectRequest
} from '../../../../../app/types'
import { withAuthCheck } from '../../../../utils/withAuthCheck'

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()
    const { tenantId, projectId } = event.context.params as {
        tenantId: string
        projectId: string
    }

    const body = await readBody<IUpdateProjectRequest>(event)

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<IProject>>(
            `${config.apiBaseUrl}/api/v1/tenant/${tenantId}/projects/${projectId}`,
            {
                method: 'PUT',
                headers,
                body,
                credentials: 'include',
            }
        )
        return res
    })
})
