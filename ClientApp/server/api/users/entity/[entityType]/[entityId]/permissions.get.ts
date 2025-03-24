import { defineEventHandler } from 'h3'
import type { IApiResponse, IUser } from '../../../../../../app/types'
import { withAuthCheck } from '../../../../../utils/withAuthCheck'

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()
    const { entityType, entityId } = event.context.params as { entityType: string, entityId: string }

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<IUser[]>>(
            `${config.apiBaseUrl}/api/v1/users/entity/${entityType}/${entityId}/permissions`,
            {
                method: 'GET',
                headers,
                credentials: 'include',
            }
        )
        return res
    })
})
