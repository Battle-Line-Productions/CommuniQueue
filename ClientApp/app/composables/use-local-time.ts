export default function useLocalTime() {
  const localTime = ref<string>('');

  const toLocalTime = (isoUtcDatetime: string | Date | undefined): string => {
    if (!isoUtcDatetime) return '';

    const date = new Date(isoUtcDatetime);

    const options: Intl.DateTimeFormatOptions = {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
      second: '2-digit',
      hour12: true,
      timeZoneName: 'short'
    };

    localTime.value = new Intl.DateTimeFormat(undefined, options).format(date);

    return localTime.value;
  };

  return {
    localTime,
    toLocalTime
  };
}
