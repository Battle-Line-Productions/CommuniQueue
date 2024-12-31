import type { Config } from 'tailwindcss'

export default <Partial<Config>>{
  content: [
    './app/**/*.{vue,js,ts,jsx,tsx}',
    // Scan Nuxt specific directories
    './components/**/*.{vue,js,ts,jsx,tsx}',
    './layouts/**/*.{vue,js,ts,jsx,tsx}',
    './pages/**/*.{vue,js,ts,jsx,tsx}',
    './plugins/**/*.{js,ts}',
    './app.vue',
  ],
  darkMode: 'class',
  theme: {
    extend: {
      maxWidth: {
        '8xl': '90rem',
      },
      colors: {
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
    },
  },
}
