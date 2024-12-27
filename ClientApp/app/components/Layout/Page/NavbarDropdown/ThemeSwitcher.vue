<script lang="ts" setup>
const props = defineProps({
  type: {
    type: String,
    default: 'dropdown-right-top',
  },
})
const currentStyle = toRef(props, 'type')

const availableThemes = [
  {
    key: 'light',
    text: 'Light',
  },
  {
    key: 'dark',
    text: 'Dark',
  },
  {
    key: 'system',
    text: 'System',
  },
]
</script>

<template>
  <div class="flex items-center">
    <!-- Headless UI Dropdown for theme selection -->
    <HeadlessListbox
      v-if="currentStyle === 'dropdown-right-top'"
      v-model="$colorMode.preference"
      as="div"
      class="relative flex items-center"
    >
      <HeadlessListboxLabel class="sr-only">
        Theme Selection
      </HeadlessListboxLabel>
      <HeadlessListboxUButton type="template">
        <BLLink class="text-light-secondary dark:text-dark-secondary">
          <!-- Display sun icon for light theme, moon for dark -->
          <span class="flex justify-center items-center dark:hidden">
            <Icon name="uil:sun" />
          </span>
          <span class="flex justify-center items-center dark:flex">
            <Icon name="uil:moon" />
          </span>
        </BLLink>
      </HeadlessListboxUButton>
      <HeadlessListboxOptions
        class="p-1 absolute z-50 origin-top-right top-full right-0 outline-none bg-light-background dark:bg-dark-background rounded-lg ring-1 ring-light-secondary/10 shadow-lg overflow-hidden w-36 py-1 text-sm text-light-textbase dark:text-dark-textbase font-semibold"
      >
        <!-- Options for changing themes -->
        <HeadlessListboxOption
          v-for="theme in availableThemes"
          :key="theme.key"
          :value="theme.key"
          :class="{
            'py-2 px-2 flex items-center cursor-pointer': true,
            'text-light-primary bg-light-surface dark:bg-dark-surface/30': $colorMode.preference === theme.key,
            'hover:bg-light-surface dark:hover:bg-dark-surface/30': $colorMode.preference !== theme.key,
          }"
        >
          <!-- Icons corresponding to the theme options -->
          <span class="flex items-center mr-2">
            <Icon
              v-if="theme.key === 'light'"
              name="uil:sun"
            />
            <Icon
              v-else-if="theme.key === 'dark'"
              name="uil:moon"
            />
            <Icon
              v-else
              name="uil:laptop"
            />
            <!-- Icon for system default -->
          </span>
          {{ theme.text }}
        </HeadlessListboxOption>
      </HeadlessListboxOptions>
    </HeadlessListbox>
    <!-- Standard select box for non-dropdown style -->
    <select
      v-if="currentStyle === 'select-box'"
      v-model="$colorMode.preference"
      class="w-full px-2 py-1 pr-3 outline-none rounded border bg-light-background dark:bg-dark-background text-light-textbase dark:text-dark-textbase border-light-secondary/10 dark:border-dark-secondary/20"
    >
      <option
        v-for="theme in availableThemes"
        :key="theme.key"
        :value="theme.key"
        class="bg-light-background dark:bg-dark-background text-light-textbase dark:text-dark-textbase"
      >
        {{ theme.text }}
      </option>
    </select>
  </div>
</template>
