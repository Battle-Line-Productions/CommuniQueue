import { useNuxtApp } from '#app';
import type { RouteLocationRaw } from 'vue-router';

export function useNavbarParser() {
  const parseMenuRoute = (to?: RouteLocationRaw | ((nuxt: any) => RouteLocationRaw)) => {
    const nuxtApp = useNuxtApp();
    return typeof to === 'function' ? to(nuxtApp) : to;
  };

  const parseMenuTitle = (title: string) => title;

  return { parseMenuRoute, parseMenuTitle };
}
