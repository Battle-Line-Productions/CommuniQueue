export default defineNuxtRouteMiddleware(async (to, from) => {
  if (!import.meta.server) return;

  const client = useLogtoClient();

  if (!client) {
    console.warn('No Logto client found. Skipping auth middleware.');
    return;
  }

  try {
    // Fetch token if required for protected routes
    const token = await client.getAccessToken('http://localhost:5000');
    if (!token) {
      console.warn('No token found.');
      return;
    }

    console.debug('Logto token:', token);

    const { setToken } = useAuthToken();
    setToken(token);
  } catch (error) {
    console.warn('No token found.');
    return;
  }
});
