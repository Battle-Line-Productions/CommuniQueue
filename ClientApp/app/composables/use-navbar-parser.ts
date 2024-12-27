import type { RouteLocationRaw } from 'vue-router'
import type { NuxtApp } from '#app' // NuxtApp type
import { useNuxtApp } from '#app'

/**
 * Provides utility functions for parsing navigation bar routes and titles.
 */
export function useNavbarParser() {
  /**
   * Dynamically resolves a route location based on the provided `to` value.
   * If `to` is a function, it will be called with the current `NuxtApp` instance.
   * Otherwise, `to` is used directly if it's already a `RouteLocationRaw`.
   *
   * @param to - An optional `RouteLocationRaw` or function that takes `NuxtApp` and returns a `RouteLocationRaw`.
   * @returns A `RouteLocationRaw`, or `undefined` if none is provided.
   */
  const parseMenuRoute = (
    to?: RouteLocationRaw | ((nuxt: NuxtApp) => RouteLocationRaw),
  ): RouteLocationRaw | undefined => {
    const nuxtApp = useNuxtApp()
    return typeof to === 'function' ? to(nuxtApp) : to
  }

  /**
   * Processes the menu title. Currently just returns the same string.
   *
   * @param title - The menu title string.
   * @returns The unmodified `title`.
   */
  const parseMenuTitle = (title: string): string => {
    return title
  }

  return { parseMenuRoute, parseMenuTitle }
}
