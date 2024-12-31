export function useTenant() {
  // Use a cookie instead of localStorage
  const tenantCookie = useCookie<string | null>('selectedTenantId', {
    maxAge: 30 * 24 * 60 * 60, // 30 days
    path: '/',
    secure: process.env.NODE_ENV === 'production',
    sameSite: 'strict',
  })

  // Use Nuxt's useState for reactive state
  const currentTenantId = useState<string | null>('currentTenantId', () => tenantCookie.value)

  function setTenant(tenantId: string) {
    // Update both the reactive state and the cookie
    currentTenantId.value = tenantId
    tenantCookie.value = tenantId
  }

  function loadTenantFromCookie() {
    // This becomes unnecessary as the cookie is automatically loaded
    // But kept for potential additional logic
    if (tenantCookie.value) {
      currentTenantId.value = tenantCookie.value
    }
  }

  // Clear tenant method
  function clearTenant() {
    currentTenantId.value = null
    tenantCookie.value = null
  }

  return {
    currentTenantId,
    setTenant,
    loadTenantFromCookie,
    clearTenant,
  }
}
