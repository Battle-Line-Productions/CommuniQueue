<script setup lang="ts">
defineProps<{
  label?: string
  fieldId?: string
  name?: string
  type?: string
  placeholder?: string
  inputClass?: string
  modelValue?: string
  required?: boolean
  error?: string
}>()

defineEmits(['update:modelValue'])
</script>

<template>
  <div class="mb-6">
    <label
      :for="fieldId"
      class="block mb-2 text-sm font-medium text-light-textbase dark:text-dark-textbase"
    >{{ label }}</label>
    <FormTextInput
      v-if="type !== 'textarea'"
      :id="fieldId"
      :type="type"
      :name="name"
      :placeholder="placeholder"
      :required="required"
      :model-value="modelValue"
      class="bg-light-background dark:bg-dark-surface border-light-secondary dark:border-dark-secondary text-light-textbase dark:text-dark-textbase rounded-lg focus:ring-light-primary dark:focus:ring-dark-primary focus:border-light-primary dark:focus:border-dark-primary block w-full"
      :class="[inputClass, { 'border-red-500': error }]"
      @input="$emit('update:modelValue', $event.target.value)"
    />
    <FormTextInput
      v-else
      :id="fieldId"
      :name="name"
      type="textarea"
      :placeholder="placeholder"
      :model-value="modelValue"
      :rows="4"
      class="bg-light-background dark:bg-dark-background border-light-secondary dark:border-dark-secondary text-light-textbase dark:text-dark-textbase rounded-lg focus:ring-light-primary dark:focus:ring-dark-primary focus:border-light-primary dark:focus:border-dark-primary block w-full"
      :class="[inputClass, { 'border-red-500': error }]"
      @input="$emit('update:modelValue', $event.target.value)"
    />
    <p
      v-if="error"
      class="text-red-500 text-sm mt-1"
    >
      {{ error }}
    </p>
  </div>
</template>
