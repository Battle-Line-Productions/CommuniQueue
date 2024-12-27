<template>
  <div class="p-4 sm:p-6 lg:p-8 space-y-6">
    <!-- Card Container -->
    <div class="bg-light-surface dark:bg-dark-surface p-6 rounded-lg shadow-sm">
      <!-- Header Section -->
      <div class="flex justify-between items-center mb-4">
        <h2 class="text-xl font-semibold text-light-textbase dark:text-dark-textbase">
          User Management
        </h2>

        <UButton
          v-if="canManageUsers"
          class="bg-light-primary dark:bg-dark-primary text-white px-4 py-2 rounded-md hover:opacity-90 transition-opacity"
          @click="showAddUserModal = true"
        >
          Add User
        </UButton>
      </div>

      <!-- Loading State -->
      <div
        v-if="isLoading"
        class="py-8 text-center text-light-secondary dark:text-dark-secondary"
      >
        <LoadingSpinner />
      </div>

      <!-- Empty State -->
      <div
        v-else-if="users.length === 0"
        class="text-center py-8"
      >
        <Icon
          name="mdi:account-group-outline"
          class="w-16 h-16 mx-auto text-light-secondary dark:text-dark-secondary mb-2"
        />
        <p class="text-light-secondary dark:text-dark-secondary">
          No users found
        </p>
      </div>

      <!-- User List -->
      <div
        v-else
        class="space-y-4"
      >
        <UserManagementCard
          v-for="user in users"
          :key="user.id"
          :user="user"
          :can-manage="canManageUsers"
          :is-current-user="isCurrentUser(user.id)"
          @remove="handleUserRemove"
        />
      </div>
    </div>

    <!-- Add User Modal -->
    <UserManagementAddUserModal
      v-if="showAddUserModal"
      @close="showAddUserModal = false"
      @added="handleUserAdded"
    />

    <!-- Confirmation Modal -->
    <ConfirmationModal
      v-if="showRemoveConfirmation"
      title="Remove User"
      :message="removeConfirmationMessage"
      confirm-text="Remove"
      :is-processing="isRemoving"
      processing-text="Removing..."
      variant="danger"
      @confirm="confirmRemoveUser"
      @cancel="cancelRemoveUser"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useQuery, useMutation, useQueryClient } from '@tanstack/vue-query'
import { useToast } from '~/composables/use-toast-service'

import type { IUser } from '~/types'
import useUsers from '~/composables/use-user-service'

// Composition
const { getAllUsers, deleteUser } = useUsers()
const { showToast } = useToast()
const queryClient = useQueryClient()

// Reactive state
const showAddUserModal = ref(false)
const showRemoveConfirmation = ref(false)
const userToRemove = ref<IUser | null>(null)

// Query to fetch users
const { data: usersData, isLoading } = useQuery({
  queryKey: ['users'],
  queryFn: () => getAllUsers(),
})

// Derived data
const users = computed(() => usersData.value?.data || [])
const canManageUsers = computed(() => true) // Replace with real permission logic

// Confirmation message
const removeConfirmationMessage = computed(() => {
  if (!userToRemove.value) return ''
  return `Are you sure you want to remove ${userToRemove.value.email}? This action cannot be undone.`
})

// Mutation to remove user
const { mutate: removeUser, isPending: isRemoving } = useMutation({
  mutationFn: async (userId: string) => {
    return await deleteUser(userId)
  },
  onSuccess: () => {
    queryClient.invalidateQueries({ queryKey: ['users'] })
    showToast({
      type: 'success',
      title: 'User Removed',
      message: 'User has been removed successfully.',
    })
    showRemoveConfirmation.value = false
    userToRemove.value = null
  },
  onError: (error: Error) => {
    showToast({
      type: 'error',
      title: 'Removal Failed',
      message: error.message || 'Failed to remove user.',
    })
  },
})

// Methods
function isCurrentUser(userId: string) {
  // Replace with actual auth check or store logic
  return userId === 'current-user-id'
}

function handleUserRemove(user: IUser) {
  userToRemove.value = user
  showRemoveConfirmation.value = true
}

function confirmRemoveUser() {
  if (userToRemove.value) {
    removeUser(userToRemove.value.id)
  }
}

function cancelRemoveUser() {
  showRemoveConfirmation.value = false
  userToRemove.value = null
}

function handleUserAdded() {
  showAddUserModal.value = false
  // Re-fetch user list
  queryClient.invalidateQueries({ queryKey: ['users'] })
}
</script>

<style scoped>
/* Optional local styling */
</style>
