// usePhoneFormat.ts
import { ref, watch, type Ref } from 'vue'

/**
 * A composable that formats a phone number string in the format:
 *   x (xxx) xxx-xxxx
 *
 * @param value - The initial phone number to format.
 * @returns An object containing `formattedPhoneValue`, which is a ref of the formatted phone number.
 */
export default function usePhoneFormat(value: string): { formattedPhoneValue: Ref<string> } {
  /**
   * A ref to store the formatted phone number.
   */
  const formattedPhoneValue = ref(value)

  /**
   * Takes a phone number string, removes any non-digit characters,
   * and formats it as x (xxx) xxx-xxxx.
   *
   * @param value - The raw phone number string to be formatted.
   */
  const formatPhoneNumber = (value: string): void => {
    // Remove all non-digit characters.
    let cleaned = value.replace(/\D/g, '')

    // Insert the formatting pattern if we have enough digits (1 + 3 + 3 + 4).
    cleaned = cleaned.replace(/(\d{1})(\d{3})(\d{3})(\d{4})/, '$1 ($2) $3-$4')

    // Limit to a maximum length of 16 characters (e.g., "9 (999) 999-9999").
    formattedPhoneValue.value = cleaned.trim().substring(0, 16)
  }

  /**
   * Watch for changes in `value`. Whenever `value` changes, reformat it.
   */
  watch(
    () => value,
    (newValue) => {
      formatPhoneNumber(newValue)
    },
  )

  // Initially format the phone number.
  formatPhoneNumber(value)

  return {
    /**
     * The reactive, formatted phone number string.
     */
    formattedPhoneValue,
  }
}
