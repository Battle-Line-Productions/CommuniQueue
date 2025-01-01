<template>
  <div class="space-y-6">
    <div class="bg-light-surface dark:bg-dark-surface p-6 rounded-lg shadow-xs">
      <div class="flex justify-between items-center mb-4">
        <h2 class="text-xl font-semibold text-light-textbase dark:text-dark-textbase">
          Team Members
        </h2>
        <UButton
          v-if="canManageUsers"
          class="bg-light-primary dark:bg-dark-primary text-white px-4 py-2 rounded-md hover:opacity-90 transition-opacity"
          @click="showInviteModal = true"
        >
          Invite Member
        </UButton>
      </div>

      <div
        v-if="isLoading"
        class="py-8 text-center text-light-secondary dark:text-dark-secondary"
      >
        <LoadingSpinner />
      </div>

      <div
        v-else-if="projectUsers?.length === 0"
        class="text-center py-8"
      >
        <Icon
          name="mdi:account-group-outline"
          class="w-16 h-16 mx-auto text-light-secondary dark:text-dark-secondary mb-2"
        />
        <p class="text-light-secondary dark:text-dark-secondary">
          No team members yet
        </p>
      </div>

      <div
        v-else
        class="space-y-4"
      >
        <DashboardAccessPermissionCard
          v-for="user in projectUsers"
          :key="user.id"
          :user="user"
          :project-id="projectId"
          :can-manage="canManageUsers"
          :is-current-user="isCurrentUser(user.id)"
          @update="handlePermissionUpdate"
          @remove="handleUserRemove"
        />
      </div>
    </div>

    <DashboardAccessInviteMemberModal
      v-if="showInviteModal"
      :project-id="projectId"
      @close="showInviteModal = false"
      @invited="handleInvited"
    />

    <ConfirmationModal
      v-if="showRemoveConfirmation"
      title="Remove Team Member"
      :message="removeConfirmationMessage"
      confirm-text="Remove"
      :is-processing="isRemoving"
      processing-text="Removing..."
      @confirm="confirmRemoveUser"
      @cancel="cancelRemoveUser"
    />
  </div>
</template>

<script setup lang="ts">
import { useQuery, useMutation, useQueryClient } from '@tanstack/vue-query'
import type { IUser, PermissionLevel } from '~/types'
import { EntityType } from '~/types'
import useUsers from '~/composables/use-user-service'
import usePermissions from '~/composables/use-permissions-service'

const route = useRoute()
const projectId = computed(() => route.params.id as string)

const { getUsersWithEntityPermissions } = useUsers()
const { updatePermission: updatePermissionApi, deletePermission: deletePermissionApi } = usePermissions()
const queryClient = useQueryClient()
const { add } = useToast()

// State management
const showInviteModal = ref(false)
const showRemoveConfirmation = ref(false)
const userToRemove = ref<IUser | null>(null)

// Query for project users with their permissions
const { data: projectUsersData, isLoading } = useQuery({
  queryKey: ['entityUsers', projectId.value, EntityType.Project],
  queryFn: () => getUsersWithEntityPermissions(projectId.value, EntityType.Project),
})

const projectUsers = computed(() => projectUsersData.value?.data || [])

// Computed properties for user management
const canManageUsers = computed(() => {
  // From the screenshot, we can see projectUsers.value[0] has the user data
  const currentUser = projectUsers.value[0]
  // Access the first permission in the permissions array
  const currentUserPermission = currentUser?.permissions?.[0]?.permissionLevel

  return currentUserPermission === 'SuperAdmin' || currentUserPermission === 'Admin'
})

// User-related methods
const isCurrentUser = (userId: string) => {
  // Replace with actual authentication check
  return userId === '7F58AFB9-CFEB-4117-998A-A6658C3BCC90'
}

// Mutations for updating and removing permissions
const { mutate: updatePermission } = useMutation({
  mutationFn: async ({ user, newLevel }: { user: IUser, newLevel: PermissionLevel }) => {
    // Find the project-specific permission
    const projectPermission = user.permissions?.find(p => p.entityId === projectId.value && p.entityType === EntityType.Project)

    if (!projectPermission) {
      throw new Error('No project permission found')
    }

    return await updatePermissionApi({
      userId: user.id,
      entityId: projectId.value,
      entityType: EntityType.Project,
      newPermissionLevel: newLevel,
    })
  },
  onSuccess: () => {
    // Invalidate and refetch project users
    queryClient.invalidateQueries({
      queryKey: ['entityUsers', projectId.value, EntityType.Project],
    })
    add({
      color: 'success',
      title: 'Permission Updated',
      description: 'User permission has been updated successfully.',
    })
  },
  onError: (error: Error) => {
    add({
      color: 'error',
      title: 'Update Failed',
      description: error.message || 'Failed to update user permission.',
    })
  },
})

// Remove user from project mutation
const { mutate: removeUser, isPending: isRemoving } = useMutation({
  mutationFn: async (user: IUser) => {
    return await deletePermissionApi(user.id, projectId.value, EntityType.Project)
  },
  onSuccess: () => {
    // Invalidate and refetch project users
    queryClient.invalidateQueries({
      queryKey: ['entityUsers', projectId.value, EntityType.Project],
    })
    add({
      color: 'success',
      title: 'User Removed',
      description: 'Team member has been removed from the project.',
    })
    showRemoveConfirmation.value = false
    userToRemove.value = null
  },
  onError: (error: Error) => {
    add({
      color: 'error',
      title: 'Removal Failed',
      description: error.message || 'Failed to remove team member.',
    })
    showRemoveConfirmation.value = false
    userToRemove.value = null
  },
})

// Event handlers
const handlePermissionUpdate = (user: IUser, newLevel: PermissionLevel) => {
  updatePermission({ user, newLevel })
}

const handleUserRemove = (user: IUser) => {
  userToRemove.value = user
  showRemoveConfirmation.value = true
}

const confirmRemoveUser = () => {
  if (userToRemove.value) {
    removeUser(userToRemove.value)
  }
}

const cancelRemoveUser = () => {
  showRemoveConfirmation.value = false
  userToRemove.value = null
}

const handleInvited = () => {
  showInviteModal.value = false
  // Invalidate and refetch project users
  queryClient.invalidateQueries({
    queryKey: ['entityUsers', projectId.value, EntityType.Project],
  })
}
</script>
