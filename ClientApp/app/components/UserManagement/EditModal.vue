<template>
  <div class="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
    <div class="bg-light-surface dark:bg-dark-surface p-6 rounded-lg w-full max-w-md">
      <h2 class="text-lg font-semibold mb-4 text-light-textbase dark:text-dark-textbase">
        Edit User
      </h2>

      <form @submit.prevent="submit">
        <FormField
          id="editUserEmail"
          v-model="localUser.email"
          label="Email"
          required
        />

        <!-- Add more fields here as desired (firstName, lastName, etc.) -->

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
            {{ isPending ? 'Saving...' : 'Save Changes' }}
          </UButton>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, watchEffect } from 'vue'
import { useMutation, useQueryClient } from '@tanstack/vue-query'
import { useToast } from '~/composables/use-toast-service'
import useUsers from '~/composables/use-user-service'
import type { IUser } from '~/types'

const props = defineProps<{
  user: IUser
}>()

const emit = defineEmits(['close', 'updated'])

// Copy the user prop into local state
const localUser = ref({ ...props.user })

watchEffect(() => {
  localUser.value = { ...props.user }
})

const { updateUser } = useUsers()
const { showToast } = useToast()
const queryClient = useQueryClient()

const { mutate, isPending } = useMutation({
  mutationFn: async () => {
    // Pass the entire user object to your update method
    return await updateUser(localUser.value.id, localUser.value)
  },
  onSuccess: (response) => {
    if (response.isSuccess && response.data) {
      showToast({
        type: 'success',
        title: 'User Updated',
        message: 'User details have been saved.',
      })
      queryClient.invalidateQueries({ queryKey: ['users'] })
      emit('updated')
    }
    else {
      showToast({
        type: 'error',
        title: 'Update Failed',
        message: response.message || 'Could not update user.',
      })
    }
  },
  onError: (error: Error) => {
    showToast({
      type: 'error',
      title: 'Update Failed',
      message: error.message || 'Could not update user.',
    })
  },
})

function submit() {
  mutate()
}
</script>
