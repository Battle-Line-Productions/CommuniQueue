<script setup lang="ts">
import { useTenant } from '~/composables/use-tenant'
import { useTenantService } from '~/composables/use-tenant-service'

const router = useRouter()
const isSubmitting = ref(false)
const tenantName = ref('')
const tenantDescription = ref('')

const { setTenant } = useTenant()
const { createTenant } = useTenantService()

async function handleSubmit() {
  isSubmitting.value = true
  try {
    const newTenant = await createTenant({
      name: tenantName.value.trim(),
      description: tenantDescription.value.trim(),
    })
    setTenant(newTenant.id)
    router.push(`/tenant/${newTenant.id}/dashboard`)
  }
  catch (err) {
    console.error('Failed to create tenant', err)
    isSubmitting.value = false
  }
}
</script>

<template>
  <div
    class="min-h-screen flex items-center justify-center bg-light-background dark:bg-dark-background text-light-textbase dark:text-dark-textbase"
  >
    <div class="bg-light-surface dark:bg-dark-surface shadow-md rounded-lg p-6 w-full max-w-md">
      <h1 class="text-2xl font-semibold mb-2">
        Create Tenant
      </h1>
      <p class="text-light-secondary dark:text-dark-secondary mb-6">
        Please provide a name and an optional description for your new tenant.
      </p>

      <form
        class="space-y-4"
        @submit.prevent="handleSubmit"
      >
        <FormField
          v-model="tenantName"
          label="Tenant Name"
          placeholder="Enter tenant name"
          required
        />
        <FormField
          v-model="tenantDescription"
          label="Description"
          placeholder="(optional) short description"
          type="textarea"
        />
        <div class="flex justify-end">
          <UButton :disabled="isSubmitting">
            {{ isSubmitting ? 'Creating...' : 'Create Tenant' }}
          </UButton>
        </div>
      </form>
    </div>
  </div>
</template>
