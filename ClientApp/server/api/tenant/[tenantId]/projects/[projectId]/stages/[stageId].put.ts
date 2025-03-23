import { defineEventHandler, readBody } from 'h3'
import type {
    IApiResponse,
    IStage,
    IUpdateProjectStageRequest
} from '../../../../../../../app/types'
import { withAuthCheck } from '../../../../../../utils/withAuthCheck'

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()
    const { tenantId, projectId, stageId } = event.context.params as {
        tenantId: string
        projectId: string
        stageId: string
    }

    const body = await readBody<IUpdateProjectStageRequest>(event)

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<IStage>>(
            `${config.apiBaseUrl}/api/v1/tenant/${tenantId}/projects/${projectId}/stages/${stageId}`,
            {
                method: 'PUT',
                headers,
                body,
            }
        )
        return res
    })
})
