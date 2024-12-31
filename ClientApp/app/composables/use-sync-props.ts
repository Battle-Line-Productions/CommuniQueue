import type { WritableComputedRef } from 'vue'

/**
 * Syncs a prop with local state, emitting the standard Vue "update:<propName>" event on set.
 *
 * @template T  The type of the entire props object.
 * @template K  A key of T, so the computed ref will be the correct type T[K].
 *
 * @param props The props object.
 * @param key   The name of the prop to sync.
 * @param emit  The emit function, typically from setup()'s context.emit.
 * @returns     A two-way computed ref that reads from props[key] and emits on write.
 */
export function useSyncProps<
  T extends Record<string, unknown>,
  K extends keyof T,
>(
  props: T,
  key: K,
  emit: (event: `update:${string}`, value: T[K]) => void,
): WritableComputedRef<T[K]> {
  return computed({
    get() {
      return props[key]
    },
    set(value) {
      emit(`update:${String(key)}`, value)
    },
  })
}
