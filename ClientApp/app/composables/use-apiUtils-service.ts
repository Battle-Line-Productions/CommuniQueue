import type { FetchContext } from 'ofetch';
import type { NitroFetchOptions, NitroFetchRequest } from 'nitropack';

export function useApiUtils() {
  return {
    handleApiCall: async <T>(apiCall: Promise<T>, actionName: string): Promise<T> => {
      try {
        console.log(`Starting ${actionName}`);

        // For development, temporarily disable SSL verification
        if (process.env.NODE_ENV === 'development') {
          process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0';
        }

        const result = await apiCall;
        console.log(`Completed ${actionName}`);
        return result;
      } catch (error) {
        console.error(`Error in ${actionName}:`, error);
        throw error;
      } finally {
        // Reset SSL verification for other calls
        if (process.env.NODE_ENV === 'development') {
          process.env.NODE_TLS_REJECT_UNAUTHORIZED = '1';
        }
      }
    },
    createFetchOptions: <ResponseType>(): NitroFetchOptions<NitroFetchRequest, 'post' | 'get' | 'head' | 'patch' | 'put' | 'delete' | 'connect' | 'options' | 'trace'> => {
      return {
        headers: {
          'Content-Type': 'application/json'
        },
        onResponseError(context: FetchContext<ResponseType>): void {
          const { request, response } = context;
          const url = request instanceof Request ? request.url : String(request);
          console.error(`API Error: ${response?.status}`, { url });
        },
        onRequestError(context: FetchContext<ResponseType>): void {
          const { request, error } = context;
          const url = request instanceof Request ? request.url : String(request);
          console.error(`Request Error: ${error?.message}`, { url });
        }
      } as NitroFetchOptions<NitroFetchRequest, 'post' | 'get' | 'head' | 'patch' | 'put' | 'delete' | 'connect' | 'options' | 'trace'>;
    }
  };
}
