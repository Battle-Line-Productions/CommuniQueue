import { ref, type Ref } from 'vue'

/**
 * Represents the supported options for displaying a toast.
 */
export interface ToastOptions {
  type: 'success' | 'error' | 'info' | 'warning'
  title: string
  message?: string
}

/**
 * Defines the shape of the toast component or object
 * that exposes an `addToast` method.
 */
export interface ToastRef {
  /**
   * Displays a new toast message based on the given options.
   */
  addToast(options: ToastOptions): void
}

/**
 * A composable to manage toast notifications via a `toastRef`.
 */
export function useToast() {
  /**
   * A reference to the toast component or object that can
   * display new toast messages.
   */
  const toastRef: Ref<ToastRef | undefined> = ref()

  /**
   * Displays a toast using the provided options. If `toastRef`
   * is set, it calls `addToast` on the underlying instance.
   *
   * @param options - An object containing information about the toast
   */
  const showToast = (options: ToastOptions): void => {
    if (toastRef.value) {
      toastRef.value.addToast(options)
    }
  }

  return {
    toastRef,
    showToast,
  }
}
