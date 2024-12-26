import { ref } from 'vue';

interface ToastOptions {
  type: 'success' | 'error' | 'info' | 'warning';
  title: string;
  message?: string;
}

export function useToast() {
  const toastRef = ref();

  const showToast = (options: ToastOptions) => {
    if (toastRef.value) {
      toastRef.value.addToast(options);
    }
  };

  return {
    toastRef,
    showToast
  };
}
