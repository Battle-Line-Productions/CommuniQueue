<script lang="ts" setup>
import type { RouteLocationRaw } from 'vue-router'

const { parseMenuRoute, parseMenuTitle } = useNavbarParser()

interface LayoutPageNavbarMenu {
  type: 'link' | 'button'
  to?: RouteLocationRaw | ((nuxt: any) => RouteLocationRaw)
  title: string
}

interface LayoutPageNavbarMenuDropdownItem extends LayoutPageNavbarMenu { }

const props = defineProps({
  menu: {
    type: Object as () => LayoutPageNavbarMenu | LayoutPageNavbarMenuDropdownItem,
    required: true,
  },
  isDropdown: {
    type: Boolean,
    default: false,
  },
})

const closeDropdown = inject<() => void>('closeDropdown')
</script>

<template>
  <template v-if="menu?.type === 'link' && isDropdown">
    <NuxtLink
      :to="parseMenuRoute(menu?.to)"
      #="{ isActive }"
      @click="closeDropdown && closeDropdown()"
    >
      <div
        :class="[
          'transition-all duration-300 hover:bg-light-surface dark:hover:bg-dark-surface px-4 py-2 rounded-lg w-full',
          isActive ? 'text-light-textbase dark:text-dark-textbase font-bold' : 'text-light-secondary dark:text-dark-secondary',
        ]"
      >
        {{ parseMenuTitle(menu?.title) }}
      </div>
    </NuxtLink>
  </template>
  <template v-else-if="menu?.type === 'link'">
    <NuxtLink
      :to="parseMenuRoute(menu?.to)"
      #="{ isActive }"
    >
      <span
        :class="{ 'text-light-textbase dark:text-dark-textbase font-bold': isActive, 'text-light-secondary dark:text-dark-secondary': !isActive }"
      >{{
        parseMenuTitle(menu?.title)
      }}</span>
    </NuxtLink>
  </template>
  <template v-else-if="menu?.type === 'button'">
    <BLButton
      :text="parseMenuTitle(menu?.title)"
      size="xs"
      :to="parseMenuRoute(menu.to)"
    />
  </template>
</template>
