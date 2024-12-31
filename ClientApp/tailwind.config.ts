import type { Config } from 'tailwindcss'
import colors from 'tailwindcss/colors'
import defaultTheme from 'tailwindcss/defaultTheme'

const MyTheme = {
  colors: {
    green: {
      DEFAULT: '#3BA676',
      50: '#B4E4CF',
      100: '#A5DFC5',
      200: '#87D4B2',
      300: '#69CA9E',
      400: '#4BBF8B',
      500: '#3BA676',
      600: '#2C7D59',
      700: '#1E533B',
      800: '#0F2A1E',
      900: '#000000',
    },
    blue: {
      DEFAULT: '#0096FF',
      50: '#B8E2FF',
      100: '#A3D9FF',
      200: '#7AC8FF',
      300: '#52B8FF',
      400: '#29A7FF',
      500: '#0096FF',
      600: '#0075C7',
      700: '#00548F',
      800: '#003357',
      900: '#00121F',
    },
    red: {
      DEFAULT: '#FF6464',
      50: '#FFFFFF',
      100: '#FFFFFF',
      200: '#FFDEDE',
      300: '#FFB6B6',
      400: '#FF8D8D',
      500: '#FF6464',
      600: '#FF2C2C',
      700: '#F30000',
      800: '#BB0000',
      900: '#830000',
    },
  },
}

export default <Partial<Config>>{
  content: [
    './app/**/*.{vue,js,ts,jsx,tsx}',
    './components/**/*.{vue,js,ts,jsx,tsx}',
    './layouts/**/*.{vue,js,ts,jsx,tsx}',
    './pages/**/*.{vue,js,ts,jsx,tsx}',
    './plugins/**/*.{js,ts}',
    './app.vue',
    './Error.{js,ts,vue}',
    './error.{js,jsx,ts,tsx,vue}',
    './nuxt.config.{js,ts}',
  ],
  darkMode: 'class',
  theme: {
    extend: {
      maxWidth: {
        '8xl': '90rem',
      },
      colors: {
        primary: MyTheme.colors.green,
        green: MyTheme.colors.green,
        blue: MyTheme.colors.blue,
        red: MyTheme.colors.red,
        slate: colors.slate,
        // Light theme colors
        light: {
          textbase: '#333333', // Dark gray for main text
          background: '#f8fafc', // Very light blue-gray for background
          surface: '#ffffff', // White for surface elements
          primary: '#3b82f6', // Bright blue for primary actions
          secondary: '#64748b', // Slate blue for secondary elements
          accent: '#06b6d4', // Cyan for accents and highlights
          error: '#ef4444', // Red for errors
          info: '#0ea5e9', // Sky blue for informational messages
          success: '#22c55e', // Green for success messages
          warning: '#f59e0b', // Amber for warnings
        },
        // Dark theme colors
        dark: {
          textbase: '#e2e8f0', // Light gray for main text
          background: '#0f172a', // Very dark blue for background
          surface: '#1e293b', // Dark blue-gray for surface elements
          primary: '#60a5fa', // Lighter blue for primary actions
          secondary: '#94a3b8', // Light slate blue for secondary elements
          accent: '#22d3ee', // Bright cyan for accents
          error: '#f87171', // Light red for errors
          info: '#38bdf8', // Bright sky blue for info
          success: '#4ade80', // Bright green for success
          warning: '#fbbf24', // Bright amber for warnings
        },
      },
      fontFamily: {
        sans: ['Nunito', ...defaultTheme.fontFamily.sans],
      },
    },
  },
}
