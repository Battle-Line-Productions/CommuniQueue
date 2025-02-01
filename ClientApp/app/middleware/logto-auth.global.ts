// server/middleware/logto-auth.global.ts
export default defineNuxtRouteMiddleware(async () => {
  // Attempt to get the Logto client (SSR only)
  const client = useLogtoClient()
  if (!client) {
    // Possibly user not logged in or Logto is not yet initialized
    console.warn('No Logto client found')
    return
  }

  // Get the token from Logto
  const token = await client.getAccessToken('https://dev-tenant.battlelineproductions.com')
  if (!token) {
    console.warn('No token found in Logto client')
    return
  }

  console.log('Logto token:', token)

  // Use our composable to store the token in cookie and (optionally) Nuxt state
  const { setToken } = useAuthToken()
  setToken(token)
})
