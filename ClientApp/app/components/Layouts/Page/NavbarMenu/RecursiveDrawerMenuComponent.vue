<script lang="ts" setup>
const { parseMenuRoute, parseMenuTitle } = useNavbarParser()

const props = defineProps({
  menu: {
    type: Object as () => LayoutPageNavbarMenuDropdownItem | LayoutPageNavbarMenu,
    required: true,
  },
  index: {
    type: Number,
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
const closeDrawer = inject<() => void>('closeDrawer')
</script>

<template>
  <div class="text-lg md:text-base">
    <div v-if="menu?.type === 'link'">
      <NuxtLink
        :key="index"
        :to="parseMenuRoute(menu.to)"
        #="{ isActive }"
        class="w-full py-2 underline"
        @click="closeDrawer && closeDrawer()"
      >
        <span
          :class="{
            'text-light-textbase dark:text-dark-textbase font-bold': isActive,
            'text-light-secondary dark:text-dark-secondary': !isActive,
          }"
        >{{ parseMenuTitle(menu?.title) }}</span>
      </NuxtLink>
    </div>
    <div v-if="menu?.type === 'button'">
      <BLButton
        :key="index"
        :text="parseMenuTitle(menu?.title)"
        size="sm"
        :to="parseMenuRoute(menu.to)"
        class="w-full underline"
      />
    </div>
    <div v-if="menu?.type === 'dropdown'">
      <div
        :key="index"
        class="w-full"
      >
        <HeadlessDisclosure v-slot="{ open }">
          <HeadlessDisclosureButton
            :key="index"
            :class="['text-light-secondary dark:text-dark-secondary w-full py-2 flex items-center justify-center duration-300 transition-all', open ? 'font-bold' : '']"
          >
            <span>{{ parseMenuTitle(menu?.title) }}</span>
            <Icon
              name="carbon:chevron-right"
              class="ml-1"
              :class="[open ? 'duration-300 transition-all transform rotate-90' : 'rotate-0']"
            />
          </HeadlessDisclosureButton>
          <Transition
            enter-active-class="transition duration-100 ease-out"
            enter-from-class="transform scale-95 opacity-0"
            enter-to-class="transform scale-100 opacity-100"
            leave-active-class="transition duration-75 ease-out"
            leave-from-class="transform scale-100 opacity-100"
            leave-to-class="transform scale-95 opacity-0"
          >
            <HeadlessDisclosurePanel class="text-light-secondary dark:text-dark-secondary pb-2">
              <template
                v-for="(child, j) in menu?.children || []"
                :key="j"
              >
                <LayoutPageNavbarMenuRecursiveDrawerMenuComponent
                  :menu="child"
                  :index="j"
                  @click="showDrawer = false"
                />
              </template>
            </HeadlessDisclosurePanel>
          </Transition>
        </HeadlessDisclosure>
      </div>
    </div>
  </div>
</template>
