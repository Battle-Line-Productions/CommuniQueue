import { defineEventHandler, getQuery } from 'h3'
import type { IApiResponse, IUser } from '../../../app/types'
import { withAuthCheck } from '../../utils/withAuthCheck'

export default defineEventHandler(async (event) => {
    const config = useRuntimeConfig()
    const { searchTerm } = getQuery(event) as { searchTerm?: string }

    return withAuthCheck(async (headers) => {
        const res = await $fetch<IApiResponse<IUser[]>>(`${config.apiBaseUrl}/api/v1/users/search`, {
            method: 'GET',
            headers,
            query: searchTerm ? { searchTerm } : {},
            credentials: 'include',
        })

        return res
    })
})
