import { ref, reactive, onMounted, onUnmounted, type Ref } from 'vue'

/**
 * A type representing valid screen size labels.
 */
export type ScreenSize = 'sm' | 'md' | 'lg' | 'xl'

/**
 * Maps each screen size label to a numeric pixel width threshold.
 * (When screen width is below that threshold, we consider it that size.)
 */
export const defaultScreenConfig: { [key in ScreenSize]: number } = {
  sm: 640,
  md: 768,
  lg: 1024,
  xl: 1280,
}

/**
 * A composable for tracking screen dimensions (width/height)
 * and determining the current screen size category (e.g., 'sm', 'md').
 */
export const useScreen = () => {
  /**
   * A reactive state object holding the browser window's current width and height.
   */
  const screenSize = reactive({
    width: 0,
    height: 0,
  })

  /**
   * A ref for storing the current screen size category ('sm', 'md', 'lg', or 'xl').
   */
  const current: Ref<ScreenSize> = ref('sm')

  /**
   * Determines the screen size category based on a provided (or the current) width.
   *
   * @param width - Optional width in pixels to use for checking size; if not provided,
   *                uses `screenSize.width`.
   * @returns The determined `ScreenSize` category.
   */
  const getSize = (width?: number): ScreenSize => {
    if (width === undefined) {
      width = screenSize.width
    }
    const { sm, md, lg, xl } = defaultScreenConfig
    if (width < sm) return 'sm'
    if (width < md) return 'md'
    if (width < lg) return 'lg'
    if (width < xl) return 'xl'
    return 'xl'
  }

  /**
   * Handler that updates the reactive `screenSize` with the current
   * browser window dimensions, then re-evaluates `current` screen size.
   */
  const onWindowResize = (): void => {
    const { innerWidth, innerHeight } = window
    screenSize.width = innerWidth
    screenSize.height = innerHeight
    current.value = getSize()
  }

  /**
   * Checks if the provided screen size (`size`) is less than or equal to
   * the current or optionally specified `defScreenSize`.
   *
   * @param size - A screen size label to compare against.
   * @param defScreenSize - (Optional) A screen size label to compare to instead of `current.value`.
   * @returns True if the `current` or given size threshold is >= `size`; otherwise false.
   */
  const higherThan = (size: ScreenSize, defScreenSize?: ScreenSize): boolean => {
    const { sm, md, lg, xl } = defaultScreenConfig
    const width = defaultScreenConfig[defScreenSize || current.value]

    if (size === 'sm') return width >= sm
    if (size === 'md') return width >= md
    if (size === 'lg') return width >= lg
    if (size === 'xl') return width >= xl
    return false
  }

  /**
   * Registers event listeners when the component is mounted.
   * Updates `current` with the initial screen size.
   */
  onMounted(() => {
    if (typeof window === 'undefined') return
    window.addEventListener('resize', onWindowResize)
    current.value = getSize(window.innerWidth)
  })

  /**
   * Cleans up event listeners when the component is unmounted.
   */
  onUnmounted(() => {
    if (typeof window === 'undefined') return
    window.removeEventListener('resize', onWindowResize)
  })

  return {
    /**
     * A function that returns a `ScreenSize` category based on
     * an optional width or the current `screenSize.width`.
     */
    getSize,
    /**
     * Reactive object containing the current `width` and `height` of the browser window.
     */
    screenSize,
    /**
     * A ref of type `ScreenSize`, representing the current screen category.
     */
    current,
    /**
     * A function that checks whether a screen size is >= a given threshold.
     */
    higherThan,
  }
}
