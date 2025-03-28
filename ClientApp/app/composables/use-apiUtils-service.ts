import type { FetchContext } from 'ofetch'
import type { NitroFetchOptions, NitroFetchRequest } from 'nitropack'

/**
 * Provides utilities for handling API calls and generating fetch options.
 */
export function useApiUtils() {
  return {
    /**
     * Wraps an API call in a try/catch/finally block with console logging.
     * When in development mode, it temporarily disables SSL verification before
     * the call and restores it afterward.
     *
     * @typeParam T - The expected type of the data resolved by the API call.
     * @param apiCall - A promise representing the API call.
     * @param actionName - A descriptive name for logging the request/action.
     * @returns A promise that resolves to the result of the `apiCall`.
     * @throws Rethrows any error caught during the API call.
     */
    handleApiCall: async <T>(
      apiCall: Promise<T>,
      actionName: string,
    ): Promise<T> => {
      try {
        console.log(`Starting ${actionName}`)

        // For development, temporarily disable SSL verification.
        if (process.env.NODE_ENV === 'development') {
          process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0'
        }

        const result = await apiCall

        if (process.env.NODE_ENV === 'development') {
          console.log(`Completed ${actionName} result:`, result)
        }

        console.log(`Completed ${actionName}`)
        return result
      }
      catch (error) {
        console.error(`Error in ${actionName}:`, error)
        throw error
      }
      finally {
        // Reset SSL verification for other calls.
        if (process.env.NODE_ENV === 'development') {
          process.env.NODE_TLS_REJECT_UNAUTHORIZED = '1'
        }
      }
    },

    /**
     * Creates a base configuration object for a Nitro fetch call.
     * This includes setting default headers, adding the Bearer token, and providing
     * error logging for both request and response errors.
     *
     * @typeParam ResponseType - The expected response type for the fetch calls.
     * @returns A typed `NitroFetchOptions` object that includes default
     * headers and error handling callbacks.
     */
    createFetchOptions: <ResponseType>():
      NitroFetchOptions<
        NitroFetchRequest,
        | 'post'
        | 'get'
        | 'head'
        | 'patch'
        | 'put'
        | 'delete'
        | 'connect'
        | 'options'
        | 'trace'
      > => {
      const headers: Record<string, string> = {
        'Content-Type': 'application/json',
      }

      return {
        headers,
        credentials: 'include',
        onResponseError(context: FetchContext<ResponseType>): void {
          const { request, response } = context
          const url
            = request instanceof Request ? request.url : String(request)
          console.error(`API Error: ${response?.status}`, { url })
        },
        onRequestError(context: FetchContext<ResponseType>): void {
          const { request, error } = context
          const url
            = request instanceof Request ? request.url : String(request)
          console.error(`Request Error: ${error?.message}`, { url })
        },
      }
    },
  }
}
