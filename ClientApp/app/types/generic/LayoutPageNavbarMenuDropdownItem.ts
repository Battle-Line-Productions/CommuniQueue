import type { NuxtApp } from 'nuxt/schema'
import type { RouteLocationRaw } from '#vue-router'

export interface LayoutPageNavbarMenuDropdownItem {
  type?: 'link'
  title?: string | ((nuxt: NuxtApp) => string)
  shouldRequireAuth?: boolean
  to?: RouteLocationRaw | ((nuxt: NuxtApp) => RouteLocationRaw)
  children?: LayoutPageNavbarMenuDropdownItem[]
}
