<template>
  <select
    :value="modelValue"
    :disabled="disabled"
    class="bg-light-surface dark:bg-dark-surface border border-light-secondary/20 dark:border-dark-secondary/20 rounded-md px-2 py-1 text-sm disabled:opacity-50 disabled:cursor-not-allowed"
    @change="$emit('update:modelValue', ($event.target as HTMLSelectElement).value as PermissionLevel)"
  >
    <option v-for="level in availableLevels" :key="level" :value="level">
      {{ formatPermissionLevel(level) }}
    </option>
  </select>
</template>

<script setup lang="ts">
import { PermissionLevel } from '~/types';

withDefaults(
  defineProps<{
    modelValue: PermissionLevel;
    disabled?: boolean;
  }>(),
  {
    disabled: false
  }
);

defineEmits<{
  'update:modelValue': [value: PermissionLevel];
}>();

const availableLevels: PermissionLevel[] = [
  PermissionLevel.ReadOnly,
  PermissionLevel.Contributor,
  PermissionLevel.Admin,
  PermissionLevel.SuperAdmin
];

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
