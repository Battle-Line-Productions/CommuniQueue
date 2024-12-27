<!-- components/UserManagementCard.vue -->
<template>
  <div class="flex items-center justify-between p-4 bg-light-background dark:bg-dark-background rounded-lg">
    <!-- Left: avatar + user info -->
    <div class="flex items-center space-x-4">
      <!-- Avatar -->
      <div class="relative">
        <Icon
          name="mdi:account-circle"
          class="w-10 h-10 text-light-secondary dark:text-dark-secondary"
        />
        <!-- Current User Dot -->
        <div
          v-if="isCurrentUser"
          class="absolute -top-1 -right-1 w-4 h-4 bg-light-success dark:bg-dark-success rounded-full border-2 border-light-background dark:border-dark-background"
          title="Current User"
        />
      </div>

      <!-- User Info -->
      <div>
        <div class="font-medium text-light-textbase dark:text-dark-textbase">
          {{ userCopy.email }}
        </div>
        <div class="flex flex-wrap items-center gap-2 text-sm">
          <span class="text-light-secondary dark:text-dark-secondary"> Joined {{ formattedCreatedDate }} </span>

          <!-- Tag to highlight if it’s you -->
          <span
            v-if="isCurrentUser"
            class="px-2 py-0.5 bg-light-success/10 dark:bg-dark-success/10 text-light-success dark:text-dark-success rounded-full text-xs"
          > You </span>

          <!-- If you store a role in your user object, show a small role tag -->
          <span
            v-if="userCopy.globalRole"
            class="px-2 py-0.5 bg-light-primary/10 dark:bg-dark-primary/10 text-light-primary dark:text-dark-primary rounded-full text-xs"
          >
            {{ formatUserRole(userCopy.globalRole) }}
          </span>
        </div>
      </div>
    </div>

    <!-- Right: actions -->
    <div class="flex items-center gap-4">
      <!-- Toggle isActive, calls updateUser with the entire user -->
      <div
        v-if="canManage && !isCurrentUser"
        class="flex items-center gap-2"
      >
        <Switch
          v-model="userCopy.isActive"
          :disabled="isUpdating"
          class="relative inline-flex h-6 w-11 items-center rounded-full cursor-pointer"
          :class="userCopy.isActive ? 'bg-light-success dark:bg-dark-success' : 'bg-light-secondary dark:bg-dark-secondary'"
          @update:model-value="handleIsActiveToggle"
        >
          <span
            class="inline-block h-4 w-4 transform bg-white rounded-full transition"
            :class="userCopy.isActive ? 'translate-x-6' : 'translate-x-1'"
          />
        </Switch>
        <span class="text-sm text-light-secondary dark:text-dark-secondary">
          {{ userCopy.isActive ? 'Active' : 'Inactive' }}
        </span>
      </div>

      <!-- Delete Button -->
      <button
        v-if="canManage && !isCurrentUser"
        class="text-light-error dark:text-dark-error hover:opacity-80 transition-opacity"
        title="Remove User"
        @click="$emit('remove', userCopy)"
      >
        <Icon
          name="mdi:trash-can-outline"
          class="w-5 h-5"
        />
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, watchEffect } from 'vue'
import { useMutation, useQueryClient } from '@tanstack/vue-query'
import { useToast } from '~/composables/use-toast-service'
import useUsers from '~/composables/use-user-service'

import type { IUser } from '~/types'

const props = defineProps<{
  user: IUser
  canManage: boolean
  isCurrentUser: boolean
}>()

defineEmits<{
  remove: [user: IUser]
}>()

const { updateUser } = useUsers()
const { showToast } = useToast()
const queryClient = useQueryClient()

// We copy the prop into a local user ref so we can toggle isActive, etc.
const userCopy = ref<IUser>({ ...props.user })

watchEffect(() => {
  // Keep userCopy in sync with incoming prop changes (if any)
  userCopy.value = { ...props.user }
})

// Format date in a user-friendly manner
const formattedCreatedDate = computed(() => {
  return new Date(userCopy.value.createdDateTime).toLocaleDateString()
})

// If your user has a role string, you can transform/capitalize it
function formatUserRole(role: string) {
  return role.charAt(0).toUpperCase() + role.slice(1)
}

// Mutation to update the entire user object
const { mutate: doUpdateUser, isPending: isUpdating } = useMutation({
  // We’ll pass the new user object as the payload
  mutationFn: async (updatedUser: IUser) => {
    return await updateUser(updatedUser.id, updatedUser)
  },
  onSuccess: () => {
    queryClient.invalidateQueries({ queryKey: ['users'] })
    showToast({
      type: 'success',
      title: 'User Updated',
      message: 'User changes have been saved.',
    })
  },
  onError: (error: Error) => {
    showToast({
      type: 'error',
      title: 'Update Failed',
      message: error.message ?? 'Could not update user.',
    })
  },
})

// Called when the Switch toggles isActive
function handleIsActiveToggle(newVal: boolean) {
  userCopy.value.isActive = newVal
  doUpdateUser(userCopy.value)
}
</script>

<style scoped>
/* Optional local styling */
</style>
