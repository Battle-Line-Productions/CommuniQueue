/**
 * Composable to manage an auth token that is shared between SSR and the client.
 * If you keep httpOnly: false, the client can read it. If httpOnly: true, the client won't see it.
 */
export function useAuthToken() {
  // We'll assume for demonstration we turn off httpOnly so the client can read it.
  // If you keep httpOnly = true, SSR can store the value in Nuxt state, but the
  // client won't be able to read the cookie directly. The state hydration trick
  // would also embed the token in the HTML, effectively exposing it anyway.
  const tokenCookie = useCookie<string | null>('auth_token', {
    path: '/',
    maxAge: 60 * 60 * 24 * 7,
    secure: process.env.NODE_ENV === 'production',
    sameSite: 'strict',
    httpOnly: false, // If you want the browser to read the token
  })

  // A reactive state that can be hydrated in SSR => client
  // (the key 'authToken' is just an example)
  const authToken = useState<string | null>('authToken', () => tokenCookie.value)

  // On SSR, we read from the cookie and populate the state
  if (import.meta.server && tokenCookie.value) {
    authToken.value = tokenCookie.value
  }

  function setToken(token: string) {
    tokenCookie.value = token
    authToken.value = token
  }

  function clearToken() {
    tokenCookie.value = null
    authToken.value = null
  }

  function getToken() {
    return authToken.value ?? tokenCookie.value
  }

  return {
    authToken, // reactive
    getToken,
    setToken,
    clearToken,
  }
}
