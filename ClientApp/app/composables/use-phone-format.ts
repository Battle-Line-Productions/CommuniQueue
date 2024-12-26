// usePhoneFormat.ts
import { ref, watch } from 'vue';

export default function usePhoneFormat(value: string) {
  const formattedPhoneValue = ref(value);

  const formatPhoneNumber = (value: string) => {
    let cleaned = value.replace(/\D/g, '');
    cleaned = cleaned.replace(/(\d{1})(\d{3})(\d{3})(\d{4})/, '$1 ($2) $3-$4');
    formattedPhoneValue.value = cleaned.trim().substring(0, 16);
  };

  watch(
    () => value,
    (newValue) => {
      formatPhoneNumber(newValue);
    }
  );

  return {
    formattedPhoneValue
  };
}
