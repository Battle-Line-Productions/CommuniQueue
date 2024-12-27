<script setup lang="ts">
import { ref, computed } from 'vue'

const props = withDefaults(
  defineProps<{
    modelValue: string
    label: string
    type?: string
    placeholder?: string
    helperText?: string
    error?: string
    required?: boolean
    disabled?: boolean
    rows?: number
    maxLength?: number
    showCharCount?: boolean
    id?: string
  }>(),
  {
    type: 'text',
    placeholder: '',
    helperText: '',
    error: '',
    maxLength: 1000,
    id: '',
    required: false,
    disabled: false,
    rows: 3,
    showCharCount: false,
  },
)

const emit = defineEmits<{
  'update:modelValue': [value: string]
}>()

// Generate a unique ID if not provided
// const uniqueId = computed(() => props.id || `field-${Math.random().toString(36).substring(2, 9)}`);

// Password visibility handling
const showPassword = ref(false)
const isPasswordType = computed(() => props.type === 'password')
// const effectiveType = computed(() => {
//   if (isPasswordType.value && showPassword.value) {
//     return 'text';
//   }
//   return props.type;
// });

const togglePasswordVisibility = () => {
  showPassword.value = !showPassword.value
}

const handleInput = (event: Event) => {
  const target = event.target as HTMLInputElement
  let value = target.value

  // Handle maxLength if specified
  if (props.maxLength && value.length > props.maxLength) {
    value = value.slice(0, props.maxLength)
  }

  emit('update:modelValue', value)
}
</script>

<template>
  <div class="form-field">
    <label
      :for="id"
      class="block text-sm font-medium text-light-textbase dark:text-dark-textbase mb-1"
    >
      {{ label }}
      <span
        v-if="required"
        class="text-light-error dark:text-dark-error"
      >*</span>
    </label>

    <template v-if="type === 'textarea'">
      <textarea
        :id="id"
        :value="modelValue"
        :placeholder="placeholder"
        :rows="rows"
        :disabled="disabled"
        :required="required"
        class="w-full px-3 py-2 border rounded-md text-light-textbase dark:text-dark-textbase bg-light-background dark:bg-dark-background disabled:opacity-50 disabled:cursor-not-allowed"
        :class="[
          error
            ? 'border-light-error dark:border-dark-error focus:ring-light-error dark:focus:ring-dark-error'
            : 'border-light-secondary/20 dark:border-dark-secondary/20 focus:border-light-primary dark:focus:border-dark-primary focus:ring-light-primary dark:focus:ring-dark-primary',
        ]"
        v-bind="$attrs"
        @input="$emit('update:modelValue', ($event.target as HTMLTextAreaElement).value)"
      />
    </template>

    <template v-else-if="type === 'select'">
      <select
        :id="id"
        :value="modelValue"
        :disabled="disabled"
        :required="required"
        class="w-full px-3 py-2 border rounded-md text-light-textbase dark:text-dark-textbase bg-light-background dark:bg-dark-background disabled:opacity-50 disabled:cursor-not-allowed"
        :class="[
          error
            ? 'border-light-error dark:border-dark-error focus:ring-light-error dark:focus:ring-dark-error'
            : 'border-light-secondary/20 dark:border-dark-secondary/20 focus:border-light-primary dark:focus:border-dark-primary focus:ring-light-primary dark:focus:ring-dark-primary',
        ]"
        v-bind="$attrs"
        @change="$emit('update:modelValue', ($event.target as HTMLSelectElement).value)"
      >
        <option
          v-if="placeholder"
          value=""
        >
          {{ placeholder }}
        </option>
        <slot />
      </select>
    </template>

    <template v-else>
      <div class="relative">
        <input
          :id="id"
          :type="type"
          :value="modelValue"
          :placeholder="placeholder"
          :disabled="disabled"
          :required="required"
          :maxlength="maxLength"
          class="w-full px-3 py-2 border rounded-md text-light-textbase dark:text-dark-textbase bg-light-background dark:bg-dark-background disabled:opacity-50 disabled:cursor-not-allowed"
          :class="[
            error
              ? 'border-light-error dark:border-dark-error focus:ring-light-error dark:focus:ring-dark-error'
              : 'border-light-secondary/20 dark:border-dark-secondary/20 focus:border-light-primary dark:focus:border-dark-primary focus:ring-light-primary dark:focus:ring-dark-primary',
            { 'pr-10': isPasswordType || showCharCount },
          ]"
          v-bind="$attrs"
          @input="handleInput"
        >

        <!-- Password Toggle UButton -->
        <UButton
          v-if="isPasswordType"
          type="button"
          class="absolute inset-y-0 right-0 flex items-center pr-3 text-light-secondary dark:text-dark-secondary hover:text-light-primary dark:hover:text-dark-primary"
          @click="togglePasswordVisibility"
        >
          <Icon
            :name="showPassword ? 'mdi:eye-off' : 'mdi:eye'"
            class="w-5 h-5"
          />
        </UButton>

        <!-- Character Count -->
        <div
          v-if="showCharCount"
          class="absolute inset-y-0 right-0 flex items-center pr-3 text-sm text-light-secondary dark:text-dark-secondary"
        >
          {{ modelValue?.length || 0 }}/{{ maxLength }}
        </div>
      </div>
    </template>

    <!-- Helper Text -->
    <div
      v-if="helperText"
      class="mt-1 text-sm text-light-secondary dark:text-dark-secondary"
    >
      {{ helperText }}
    </div>

    <!-- Error Message -->
    <div
      v-if="error"
      class="mt-1 text-sm text-light-error dark:text-dark-error flex items-center space-x-1"
    >
      <Icon
        name="mdi:alert-circle"
        class="w-4 h-4"
      />
      <span>{{ error }}</span>
    </div>
  </div>
</template>

<style scoped>
.form-field {
  position: relative;
}

/* Focus styles */
input:focus,
textarea:focus,
select:focus {
  outline: none;
  box-shadow: 0 0 0 2px rgba(0, 0, 0, 0.5);
}

/* Transition for all interactive states */
input,
textarea,
select,
UButton {
  transition: all 0.2s;
}
</style>
