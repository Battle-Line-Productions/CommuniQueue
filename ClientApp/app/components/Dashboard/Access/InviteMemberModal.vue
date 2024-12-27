<template>
  <div
    class="fixed inset-0 z-50 flex items-center justify-center bg-black/50 backdrop-blur-sm"
    @click.self="$emit('close')"
  >
    <div
      class="w-full max-w-md mx-4 bg-light-surface dark:bg-dark-surface rounded-lg shadow-xl transform transition-all duration-300 ease-in-out"
      role="dialog"
      aria-modal="true"
    >
      <!-- Header -->
      <div
        class="flex justify-between items-center p-6 border-b border-light-secondary/10 dark:border-dark-secondary/10"
      >
        <h2 class="text-xl font-semibold text-light-textbase dark:text-dark-textbase">
          Invite Team Member
        </h2>
        <UButton
          class="text-light-secondary dark:text-dark-secondary hover:text-light-textbase dark:hover:text-dark-textbase"
          @click="$emit('close')"
        >
          <Icon
            name="mdi:close"
            class="w-6 h-6"
          />
        </UButton>
      </div>

      <!-- Content -->
      <div class="p-6 space-y-6">
        <!-- User Search Section (previously shown) -->
        <div>
          <label class="block text-sm font-medium text-light-textbase dark:text-dark-textbase mb-1"> Search User
          </label>
          <div class="relative">
            <FormField
              v-model="searchTerm"
              label="Email or Name"
              type="text"
              placeholder="Search by email or name"
              :disabled="isPending"
            />

            <!-- Loading State -->
            <div
              v-if="isSearching"
              class="absolute z-10 w-full mt-1 flex items-center justify-center py-2 text-light-secondary dark:text-dark-secondary"
            >
              <LoadingSpinner class="h-5 w-5" />
              <span class="ml-2">Searching...</span>
            </div>

            <!-- Error State -->
            <div
              v-else-if="searchError"
              class="absolute z-10 w-full mt-1 bg-light-error/10 dark:bg-dark-error/10 text-light-error dark:text-dark-error p-2 rounded-md"
            >
              <p class="text-sm">
                <Icon
                  name="mdi:alert-circle"
                  class="inline-block mr-2 w-5 h-5"
                />
                {{ searchError.message || 'An error occurred while searching' }}
              </p>
            </div>

            <!-- Search Results Dropdown -->
            <div
              v-else-if="(searchResults.value?.data ?? []).length > 0"
              class="absolute z-10 w-full mt-1 bg-light-surface dark:bg-dark-surface border rounded-md shadow-lg max-h-60 overflow-y-auto"
            >
              <div
                v-for="user in searchResults.value?.data"
                :key="user.id"
                class="px-4 py-2 hover:bg-light-background dark:hover:bg-dark-background cursor-pointer flex items-center justify-between"
                @click="selectUser(user)"
              >
                <div>
                  <span class="font-medium">{{ user.email }}</span>
                </div>
                <Icon
                  v-if="selectedUser?.id === user.id"
                  name="mdi:check-circle"
                  class="w-5 h-5 text-light-success dark:text-dark-success"
                />
              </div>
            </div>

            <!-- No Results State -->
            <div
              v-else-if="searchTerm.length >= 2"
              class="absolute z-10 w-full mt-1 px-4 py-2 text-light-secondary dark:text-dark-secondary"
            >
              No users found
            </div>
          </div>
        </div>

        <!-- Selected User Display -->
        <div
          v-if="selectedUser"
          class="bg-light-background dark:bg-dark-background p-4 rounded-lg flex items-center justify-between"
        >
          <div class="flex items-center space-x-3">
            <Icon
              name="mdi:account-circle"
              class="w-10 h-10 text-light-secondary dark:text-dark-secondary"
            />
            <div>
              <div class="font-medium">
                {{ selectedUser.email }}
              </div>
            </div>
          </div>
          <UButton
            type="UButton"
            class="text-light-error dark:text-dark-error hover:opacity-80"
            @click="clearSelectedUser"
          >
            <Icon
              name="mdi:close-circle"
              class="w-6 h-6"
            />
          </UButton>
        </div>

        <!-- Permission Level Selection -->
        <div>
          <label class="block text-sm font-medium text-light-textbase dark:text-dark-textbase mb-1">
            Permission Level
            <span class="text-light-error dark:text-dark-error">*</span>
          </label>
          <select
            v-model="selectedPermissionLevelValue"
            class="w-full px-3 py-2 border rounded-md text-light-textbase dark:text-dark-textbase bg-light-background dark:bg-dark-background border-light-secondary/20 dark:border-dark-secondary/20"
            :disabled="isPending"
          >
            <option
              v-for="level in availablePermissionLevels"
              :key="level.value"
              :value="level.value"
            >
              {{ level.label }}
            </option>
          </select>

          <!-- Permission Description -->
          <div class="mt-4 p-4 bg-light-background dark:bg-dark-background rounded-lg">
            <h4 class="font-medium text-light-textbase dark:text-dark-textbase mb-2">
              {{ currentPermissionLevel.label }} Access
            </h4>
            <p class="text-sm text-light-secondary dark:text-dark-secondary">
              {{ currentPermissionLevel.description }}
            </p>
            <ul class="mt-2 space-y-1">
              <li
                v-for="(ability, index) in currentPermissionLevel.abilities"
                :key="index"
                class="text-sm flex items-center space-x-2 text-light-textbase dark:text-dark-textbase"
              >
                <Icon
                  name="mdi:check-circle"
                  class="w-4 h-4 text-light-success dark:text-dark-success"
                />
                <span>{{ ability }}</span>
              </li>
            </ul>
          </div>
        </div>

        <!-- Action UButtons -->
        <div class="flex justify-end space-x-4 pt-4">
          <UButton
            type="UButton"
            class="text-light-secondary dark:text-dark-secondary hover:underline px-4 py-2"
            :disabled="isPending"
            @click="$emit('close')"
          >
            Cancel
          </UButton>
          <UButton
            type="UButton"
            :disabled="isPending || !isFormValid"
            class="bg-light-primary dark:bg-dark-primary text-white px-4 py-2 rounded-md hover:opacity-90 transition-opacity disabled:opacity-50"
            @click="handleSubmit"
          >
            {{ isPending ? 'Sending Invitation...' : 'Send Invitation' }}
          </UButton>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useMutation, useQuery } from '@tanstack/vue-query'
import { useToast } from '~/composables/use-toast-service'
import useUsers from '~/composables/use-user-service'
import useProjects from '~/composables/use-projects-service'
import { PermissionLevel, type IUser } from '~/types'

const props = defineProps<{
  projectId: string
}>()

const emit = defineEmits<{
  close: []
  invited: []
}>()

const { searchUsers } = useUsers()
const { addUserToProject } = useProjects()
const { showToast } = useToast()

// State management
const searchTerm = ref('')
const selectedUser = ref<IUser | null>(null)
const selectedPermissionLevelValue = ref<PermissionLevel>(PermissionLevel.ReadOnly)

// Search query using Vue Query
const searchQuery = useQuery({
  queryKey: ['userSearch', searchTerm],
  queryFn: () => searchUsers(searchTerm.value),
  enabled: computed(() => searchTerm.value.length >= 2),
  placeholderData: previousData => previousData,
  staleTime: 10000,
})

// Computed properties for search results and states
const searchResults = computed(() => searchQuery.data ?? [])
const isSearching = computed(() => searchQuery.isLoading.value)
const searchError = computed(() => searchQuery.error.value)

// Permission levels configuration
const permissionLevels = {
  [PermissionLevel.ReadOnly]: {
    value: PermissionLevel.ReadOnly,
    label: 'Read Only',
    description: 'Can view project content but cannot make changes.',
    abilities: ['View all project content', 'View templates and containers', 'Download template versions'],
  },
  [PermissionLevel.Contributor]: {
    value: PermissionLevel.Contributor,
    label: 'Contributor',
    description: 'Can create and modify content within the project.',
    abilities: ['All Read Only permissions', 'Create and edit templates', 'Create and manage containers', 'Manage template versions'],
  },
  [PermissionLevel.Admin]: {
    value: PermissionLevel.Admin,
    label: 'Admin',
    description: 'Full project management capabilities except ownership transfer.',
    abilities: ['All Contributor permissions', 'Manage team members', 'Configure project settings', 'Manage API keys'],
  },
  [PermissionLevel.SuperAdmin]: {
    value: PermissionLevel.SuperAdmin,
    label: 'Super Admin',
    description: 'Full system-wide administrative access.',
    abilities: ['All Admin permissions', 'System-wide configuration', 'User management across all projects'],
  },
}

const availablePermissionLevels = Object.values(permissionLevels)

const currentPermissionLevel = computed(() => {
  return permissionLevels[selectedPermissionLevelValue.value] || permissionLevels[PermissionLevel.ReadOnly]
})

// Form validation
const isFormValid = computed(() => {
  return !!selectedUser.value && !!selectedPermissionLevelValue.value
})

// User selection method
const selectUser = (user: IUser) => {
  selectedUser.value = user
  searchTerm.value = '' // Clear search term
}

const clearSelectedUser = () => {
  selectedUser.value = null
}

// Invitation mutation
const { mutate, isPending } = useMutation({
  mutationFn: async () => {
    if (!selectedUser.value) {
      throw new Error('No user selected')
    }

    return await addUserToProject(props.projectId, {
      userId: selectedUser.value.id,
      permissionLevel: selectedPermissionLevelValue.value,
    })
  },
  onSuccess: () => {
    showToast({
      type: 'success',
      title: 'Invitation Sent',
      message: `An invitation has been sent to ${selectedUser.value?.email}`,
    })
    emit('invited')
  },
  onError: (error: Error) => {
    showToast({
      type: 'error',
      title: 'Invitation Failed',
      message: error.message || 'Failed to send invitation',
    })
  },
})

// Form submission handler
const handleSubmit = () => {
  // Validate form
  if (!isFormValid.value) {
    showToast({
      type: 'error',
      title: 'Incomplete Form',
      message: 'Please select a user and permission level',
    })
    return
  }

  // Trigger mutation
  mutate()
}

// Watch for search term changes
watch(searchTerm, (newTerm) => {
  // Reset search results if term is too short
  if (newTerm.length < 2) {
    searchQuery.refetch()
  }
})

// Cleanup on component unmount
onUnmounted(() => {
  selectedUser.value = null
})
</script>

<style scoped>
/* Custom scrollbar for search results */
.max-h-60::-webkit-scrollbar {
  width: 6px;
}

.max-h-60::-webkit-scrollbar-track {
  background: rgba(0, 0, 0, 0.1);
}

.max-h-60::-webkit-scrollbar-thumb {
  background: rgba(0, 0, 0, 0.2);
  border-radius: 3px;
}
</style>
