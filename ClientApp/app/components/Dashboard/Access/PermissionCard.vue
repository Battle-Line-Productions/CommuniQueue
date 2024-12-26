<template>
  <div class="flex items-center justify-between p-4 bg-light-background dark:bg-dark-background rounded-lg">
    <div class="flex items-center space-x-4">
      <div class="relative">
        <Icon name="mdi:account-circle" class="w-10 h-10 text-light-secondary dark:text-dark-secondary" />
        <div
          v-if="isCurrentUser"
          class="absolute -top-1 -right-1 w-4 h-4 bg-light-success dark:bg-dark-success rounded-full border-2 border-light-background dark:border-dark-background"
          title="Current User"
        />
      </div>
      <div>
        <div class="font-medium text-light-textbase dark:text-dark-textbase">
          {{ user.email }}
        </div>
        <div class="flex items-center space-x-2 text-sm text-light-secondary dark:text-dark-secondary">
          <span> Added {{ localTime.toLocalTime(projectPermission?.createdDateTime || new Date()) }} </span>
          <span v-if="isCurrentUser" class="px-2 py-0.5 bg-light-success/10 dark:bg-dark-success/10 text-light-success dark:text-dark-success rounded-full text-xs"> You </span>
        </div>
      </div>
    </div>

    <div class="flex items-center space-x-4">
      <DashboardAccessPermissionSelect
        v-if="canManage"
        :model-value="projectPermission?.permissionLevel || PermissionLevel.ReadOnly"
        :disabled="isCurrentUser || !canManage"
        @update:model-value="(newLevel: PermissionLevel) => $emit('update', user, newLevel)"
      />
      <div v-else class="px-2 py-1 text-sm rounded-md bg-light-secondary/10 dark:bg-dark-secondary/10">
        {{ formatPermissionLevel(projectPermission?.permissionLevel || PermissionLevel.ReadOnly) }}
      </div>

      <button v-if="canManage && !isCurrentUser" class="text-light-error dark:text-dark-error hover:opacity-80 transition-opacity" @click="$emit('remove', user)">
        <Icon name="mdi:trash-can-outline" class="w-5 h-5" />
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { IUser } from '~/types';
import { EntityType, PermissionLevel } from '~/types';

const props = defineProps<{
  user: IUser;
  projectId: string;
  canManage: boolean;
  isCurrentUser: boolean;
}>();

defineEmits<{
  update: [user: IUser, newLevel: PermissionLevel];
  remove: [user: IUser];
}>();

const localTime = useLocalTime();

const projectPermission = computed(() => props.user.permissions?.find((p) => p.entityId === props.projectId && p.entityType === EntityType.Project));

const formatPermissionLevel = (level: PermissionLevel): string => {
  return (
    {
      ReadOnly: 'Read Only',
      Contributor: 'Contributor',
      Admin: 'Admin',
      SuperAdmin: 'Super Admin'
    }[level] || level
  );
};
</script>
