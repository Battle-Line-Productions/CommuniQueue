<script lang="ts" setup>
const { parseMenuRoute, parseMenuTitle } = useNavbarParser()

const props = defineProps({
  menu: {
    type: Object as () => LayoutPageNavbarMenuDropdownItem | LayoutPageNavbarMenu,
    required: true,
  },
})

const route = useRoute()

const isActive = computed(() => {
  if (!(props.menu as any)?.children) return false
  const childs = ((props.menu as any)?.children || []) as LayoutPageNavbarMenu[]
  for (const child of childs) {
    const to = parseMenuRoute(child.to)
    if (typeof to === 'string' && to === route.path) return true
    if (route && route.name && route.name.toString().includes((to as any)?.name?.toString())) return true
  }
  return false
})

provide('closeDropdown', close)
</script>

<template>
  <template v-if="menu.type === 'button' || menu.type === 'link'">
    <LayoutPageNavbarMenuItem :menu="menu" />
  </template>
  <template v-else-if="menu.children && menu.children.length > 0">
    <HeadlessPopover v-slot="{ open }">
      <HeadlessPopoverButton
        class="flex items-center transition-all duration-300 rounded-md px-2 py-1 text-light-textbase dark:text-dark-textbase hover:bg-light-secondary/50 dark:hover:bg-dark-secondary/50"
      >
        <span :class="`font-medium ${isActive ? 'font-bold' : ''}`">{{ parseMenuTitle(menu.title) }}</span>
        <Icon
          name="carbon:chevron-down"
          class="ml-1 w-4 h-4"
          :class="{ 'transform rotate-180': open }"
        />
      </HeadlessPopoverButton>
      <Transition
        enter-active-class="transition ease-out duration-200"
        enter-from-class="opacity-0 scale-95"
        enter-to-class="opacity-100 scale-100"
        leave-active-class="transition ease-in duration-150"
        leave-from-class="opacity-100 scale-100"
        leave-to-class="opacity-0 scale-95"
      >
        <HeadlessPopoverPanel
          v-slot="{ close }"
          class="absolute z-10 mt-1 rounded-md shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none bg-white dark:bg-gray-700"
        >
          <div class="py-1">
            <template
              v-for="(child, j) in menu.children"
              :key="j"
            >
              <LayoutPageNavbarMenuRecursiveMenuComponent
                :menu="child"
                is-dropdown
                class="block px-4 py-2 text-sm text-gray-700 dark:text-gray-200 hover:bg-gray-100 dark:hover:bg-gray-600"
              />
            </template>
          </div>
        </HeadlessPopoverPanel>
      </Transition>
    </HeadlessPopover>
  </template>
</template>
