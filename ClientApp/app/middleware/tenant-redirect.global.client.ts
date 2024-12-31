import type { UserInfoResponse } from '@logto/nuxt'

export default defineNuxtRouteMiddleware((to, _) => {
  const user = useLogtoUser() as UserInfoResponse | null
  const { currentTenantId, loadTenantFromCookie } = useTenant()

  // Make sure we update our composable from local storage
  loadTenantFromCookie()

  // If the user is logged in but no tenant is selected,
  // and we're not already going to /auth/logging-in,
  // redirect to /auth/logging-in
  if (user && !currentTenantId.value && to.path !== '/auth/logging-in') {
    return navigateTo('/auth/logging-in')
  }
})
