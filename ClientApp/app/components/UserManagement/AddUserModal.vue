<template>
  <div class="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
    <div class="bg-light-surface dark:bg-dark-surface p-6 rounded-lg w-full max-w-md">
      <h2 class="text-lg font-semibold mb-4 text-light-textbase dark:text-dark-textbase">
        Add a New User
      </h2>

      <form
        class="space-y-4"
        @submit.prevent="submit"
      >
        <!-- First Name -->
        <FormField
          id="addUserFirstName"
          v-model="firstName"
          label="First Name"
          required
          placeholder="John"
          :error="errors.firstName"
          @input="errors.firstName = ''"
        />

        <!-- Last Name -->
        <FormField
          id="addUserLastName"
          v-model="lastName"
          label="Last Name"
          required
          placeholder="Doe"
          :error="errors.lastName"
          @input="errors.lastName = ''"
        />

        <!-- Email -->
        <FormField
          id="addUserEmail"
          v-model="email"
          label="Email"
          required
          placeholder="user@example.com"
          :error="errors.email"
          @input="errors.email = ''"
        />

        <!-- Global Role (Owner not selectable) -->
        <FormField
          id="addUserGlobalRole"
          v-model="globalRole"
          label="Global Role"
          type="select"
          :required="true"
        >
          <!-- We exclude Owner from the dropdown -->
          <option value="SuperAdmin">
            Super Admin
          </option>
          <option value="Contributor">
            Contributor
          </option>
          <option value="ReadOnly">
            Read Only
          </option>
        </FormField>

        <!-- Descriptions of roles for clarity -->
        <div class="mb-4 text-sm text-light-secondary dark:text-dark-secondary space-y-2">
          <p><strong>Owner</strong>: Automatically assigned to the account creator. Not selectable.</p>
          <p><strong>Super Admin</strong>: Full privileges across the entire application.</p>
          <p><strong>Contributor</strong>: Collaborates on tasks but does not have full admin rights.</p>
          <p>
            <strong>Read Only</strong>: Has no system access and only access to projects and templates when
            specifically granted.
          </p>
        </div>

        <!-- IsActive checkbox -->
        <div class="flex items-center gap-2">
          <input
            id="addUserIsActive"
            v-model="isActive"
            type="checkbox"
            class="h-4 w-4 rounded border-light-secondary/20 dark:border-dark-secondary/20"
          >
          <label
            for="addUserIsActive"
            class="text-sm text-light-textbase dark:text-dark-textbase cursor-pointer"
          >
            Active User </label>
        </div>

        <!-- UButtons -->
        <div class="flex justify-end space-x-2 mt-6">
          <UButton
            type="button"
            class="px-4 py-2 text-light-secondary dark:text-dark-secondary hover:underline"
            @click="$emit('close')"
          >
            Cancel
          </UButton>
          <UButton
            :disabled="isPending"
            class="bg-light-primary dark:bg-dark-primary text-white px-4 py-2 rounded-md hover:opacity-90 transition-opacity disabled:opacity-50"
          >
            {{ isPending ? 'Adding...' : 'Add User' }}
          </UButton>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useMutation, useQueryClient } from '@tanstack/vue-query'
import useUsers from '~/composables/use-user-service'
import { type ICreateUserRequest, GlobalRoleType } from '~/types'

const emit = defineEmits<{
  (event: 'close' | 'added'): void
}>()

// Form fields
const firstName = ref('')
const lastName = ref('')
const email = ref('')
const globalRole = ref<GlobalRoleType>(GlobalRoleType.Contributor) // Default
const isActive = ref(true) // or false if you prefer they not be active by default

// Basic client-side errors for each required field
const errors = ref({
  firstName: '',
  lastName: '',
  email: '',
})

const { createUser } = useUsers()
const { add } = useToast()
const queryClient = useQueryClient()

// The create user mutation
const { mutate, isPending } = useMutation({
  mutationFn: async () => {
    // Build request
    const payload: ICreateUserRequest = {
      email: email.value,
      firstName: firstName.value,
      lastName: lastName.value,
      isActive: isActive.value,
      // Even if your backend requires a string, ensure you define
      // these fields in your ICreateUserRequest
      globalRole: globalRole.value,
      ssoId: '', // or some relevant property if needed
    }
    return await createUser(payload)
  },
  onSuccess: (response) => {
    if (response.isSuccess && response.data) {
      add({
        color: 'success',
        title: 'User Created',
        description: 'New user has been successfully created.',
      })
      queryClient.invalidateQueries({ queryKey: ['users'] })
      emit('added')
    }
    else {
      add({
        color: 'error',
        title: 'Creation Failed',
        description: response.message || 'Could not create user.',
      })
    }
  },
  onError: (error: Error) => {
    add({
      color: 'error',
      title: 'Creation Failed',
      description: error.message || 'Could not create user.',
    })
  },
})

// Validate required fields before calling mutate
function submit() {
  let valid = true

  if (!firstName.value.trim()) {
    errors.value.firstName = 'First name is required'
    valid = false
  }
  if (!lastName.value.trim()) {
    errors.value.lastName = 'Last name is required'
    valid = false
  }
  if (!email.value.trim()) {
    errors.value.email = 'Email is required'
    valid = false
  }
  else if (!isValidEmail(email.value)) {
    errors.value.email = 'Please enter a valid email'
    valid = false
  }

  if (!valid) return

  // if all validations pass
  mutate()
}

// Example email check
function isValidEmail(value: string): boolean {
  // Basic email check, can be replaced with something more robust
  return /\S+@\S+\.\S+/.test(value)
}
</script>

<style scoped>
/* Basic styling for your modal, adapt as you see fit */
</style>
