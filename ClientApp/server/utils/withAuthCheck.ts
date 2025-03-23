// ~/server/utils/withAuthCheck.ts
import { useAuthToken } from '../../app/composables/use-auth-token'
import type { IApiResponse } from '../../app/types'

/**
 * A utility function that checks for an auth token and returns
 * a 401 response if none is found. Otherwise, it injects the
 * Authorization header into a headers object and calls a
 * callback for further processing.
 *
 * @param callback - A function that receives the pre-filled headers and handles the rest.
 */
export async function withAuthCheck<T>(
    callback: (headers: Record<string, string>) => Promise<T>
): Promise<T | { status: number; body: string }> {
    const tokenService = useAuthToken()
    const authorization = tokenService.getToken()

    // If no token, return a 401 object response (or you can throw an error).
    if (!authorization) {
        // Return an object directly:
        return {
            status: 401,
            body: JSON.stringify({
                isSuccess: false,
                status: 401,
                errors: ['Unauthorized'],
                message: 'Unauthorized',
            } as IApiResponse<null>),
        }
    }

    // Build a headers object with Authorization
    const headers: Record<string, string> = {
        Accept: 'application/json',
        'Content-Type': 'application/json',
        Authorization: `Bearer ${authorization}`,
    }

    // Delegate to the caller with our prepared headers
    return callback(headers)
}
