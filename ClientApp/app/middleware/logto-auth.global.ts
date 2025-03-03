export default defineNuxtRouteMiddleware(async (to, from) => {
  if (import.meta.server) return;
  
  const client = useLogtoClient();
  
  if (!client) {
    console.warn('No Logto client found. Skipping auth middleware.');
    return;
  }

  // Check if the user is authenticated before calling getAccessToken
  if (!(await client.isAuthenticated())) {
    console.warn('User is not authenticated. Redirecting to login...');
    return navigateTo('/login'); // Redirect instead of throwing error
  }

  // // We only want auth on certain pages, exclude public pages here:
  // const publicRoutes = ['/', '/features', '/get-started'];
  // if (publicRoutes.includes(to.path)) return;

  try {
    // Fetch token if required for protected routes
    const token = await client.getAccessToken('https://dev-tenant.battlelineproductions.com');
    if (!token) {
      console.warn('No token found, but user is authenticated.');
      return;
    }

    console.debug('Logto token:', token);

    // Store token in cookie or state
    const { setToken } = useAuthToken();
    setToken(token);
  } catch (error) {
    console.error('Error retrieving access token:', error);
    return navigateTo('/login');
  }
});
