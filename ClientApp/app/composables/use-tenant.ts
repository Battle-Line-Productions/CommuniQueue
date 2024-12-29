export function useTenant() {
  const currentTenantId = useState<string | null>('currentTenantId', () => null)

  function setTenant(tenantId: string) {
    currentTenantId.value = tenantId
    localStorage.setItem('selectedTenantId', tenantId)
  }

  function loadTenantFromStorage() {
    const stored = localStorage.getItem('selectedTenantId')
    if (stored) {
      currentTenantId.value = stored
    }
  }

  return {
    currentTenantId,
    setTenant,
    loadTenantFromStorage,
  }
}
