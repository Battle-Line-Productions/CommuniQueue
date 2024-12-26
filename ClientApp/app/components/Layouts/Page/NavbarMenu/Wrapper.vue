<script lang="ts" setup>
const route = useRoute()
const { parseMenuRoute, parseMenuTitle } = useNavbarParser()

const props = defineProps({
  menu: {
    type: Object as () => LayoutPageNavbarMenuDropdownItem | LayoutPageNavbarMenu,
    required: true,
  },
})
const isActive = computed(() => {
  if (!(props.menu as any)?.children) return false
  const childs = ((props.menu as any)?.children || []) as LayoutPageNavbarMenu[]
  for (const child of childs) {
    const to = parseMenuRoute(child.to)
    if (typeof to === 'string' && to === route.path) return true
    if (route.name?.toString().includes((to as any).name?.toString())) return true
  }
  return false
})
</script>

<template>
  <template v-if="menu.type !== 'dropdown'">
    <LayoutPageNavbarMenuItem :menu="menu" />
  </template>
  <template v-else-if="menu.children && menu.children.length > 0">
    <HeadlessPopover v-slot="{ open }">
      <HeadlessPopoverButton
        class="flex items-center transition-all duration-300 text-light-textbase dark:text-dark-textbase"
      >
        <span :class="[isActive ? 'font-bold' : '']">{{ parseMenuTitle(menu.title) }}</span>
        <Icon
          name="carbon:chevron-down"
          class="ml-1"
          :class="[open ? 'transform rotate-180' : '']"
        />
      </HeadlessPopoverButton>
      <Transition
        enter-active-class="transition duration-200 ease-out"
        enter-from-class="translate-y-1 opacity-0"
        enter-to-class="translate-y-0 opacity-100"
        leave-active-class="transition duration-150 ease-in"
        leave-from-class="translate-y-0 opacity-100"
        leave-to-class="translate-y-1 opacity-0"
      >
        <HeadlessPopoverPanel
          class="absolute z-10 px-2 py-2 rounded-lg min-w-[150px] bg-light-surface dark:bg-dark-surface border-light-secondary dark:border-dark-secondary border"
        >
          <div class="grid grid-cols-1">
            <template
              v-for="(child, j) in menu.children"
              :key="j"
            >
              <LayoutPageNavbarMenuItem
                :menu="child"
                is-dropdown
              />
            </template>
          </div>
        </HeadlessPopoverPanel>
      </Transition>
    </HeadlessPopover>
  </template>
</template>
