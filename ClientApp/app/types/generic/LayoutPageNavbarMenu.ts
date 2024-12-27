import type { NuxtApp } from 'nuxt/schema'
import type { RouteLocationRaw } from '#vue-router'
import type { LayoutPageNavbarMenuDropdownItem } from '~/types'

export interface LayoutPageNavbarMenu {
  type?: 'link' | 'button' | 'dropdown'
  shouldRequireAuth?: boolean
  title?: string | ((nuxt: NuxtApp) => string)
  to?: RouteLocationRaw | ((nuxt: NuxtApp) => RouteLocationRaw)
  children?: LayoutPageNavbarMenuDropdownItem[]
}
