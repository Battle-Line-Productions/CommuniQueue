#!/usr/bin/env node

import fs from 'fs'
import path from 'path'

/**
 * The text you wish to prepend (at the top) of the output file.
 */
const prefixText = `
I'm making an application called communiqueue. its a email and texting template tool where users will come and create templates that take in data on my website and then use my API to send email and text messages by passing me data and I'll built the content.

Users log in and create projects. Each project contains containers. a project contains a default container. the container can contain other containers like a windows folder structure. Inside of these contains are templates. Templates are the things the user will update and manage and these can be stored at any level of containers the user creates for organizational purposes.

My site is built using nuxt 3 Typescript composition api built as SSG and tailwind css as my UI library. Below is my tailwind config and theme:
import type { Config } from 'tailwindcss'

export default <Partial<Config>>{
  content: [],
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

i've also included my nuxt config and package.json so you know what packages i have available to me.
// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({

  modules: [
    '@nuxt/eslint',
    '@nuxt/ui',
    '@nuxt/icon',
    '@nuxt/fonts',
    '@logto/nuxt',
    '@nuxtjs/color-mode',
    'nuxt-headlessui',
    '@hebilicious/vue-query-nuxt',
    '@logto/nuxt',
    '@nuxt/scripts',
    '@nuxtjs/seo',
    '@vueuse/nuxt',
  ],

  devtools: {
    enabled: true,
    timeline: {
      enabled: true,
    },
  },

  app: {
    pageTransition: { name: 'page', mode: 'out-in' },
    layoutTransition: { name: 'layout', mode: 'out-in' },
    head: {
      titleTemplate: '%s | CommuniQueue',
      meta: [
        { name: 'viewport', content: 'width=device-width, initial-scale=1' },
      ],
    },
  },

  css: ['~/assets/css/main.css'],

  site: {
    url: process.env.NUXT_SITE_URL,
    name: process.env.NUXT_SITE_NAME,
    description:
      'Streamline communication with template management and messaging API. Create and send personalized emails. Custom organization for businesses and individuals.',
    defaultLocale: 'en',
    debug: process.env.NUXT_SITE_ENV !== 'production',
  },

  colorMode: {
    classSuffix: '',
  },

  runtimeConfig: {
    public: {
      scripts: {
        cloudflareWebAnalytics: {
          // NUXT_PUBLIC_SCRIPTS_CLOUDFLARE_WEB_ANALYTICS_TOKEN=<your-token>
          token: '',
        },
        googleAnalytics: {
          // NUXT_PUBLIC_SCRIPTS_GOOGLE_ANALYTICS_ID=<your-id>
          id: '',
        },
        clarity: {
          // NUXT_PUBLIC_SCRIPTS_CLARITY_ID=<your-id>
          id: '',
        },
      },
    },
    logto: {
      endpoint: 'https://LocalAuth.battlelineproductions.com/',
      appId: '',
      appSecret: '',
      cookieEncryptionKey: '',
    },
  },

  future: {
    compatibilityVersion: 4,
  },

  compatibilityDate: '2024-11-01',

  postcss: {
    plugins: {
      '@tailwindcss/postcss': {},
    },
  },

  eslint: {
    config: {
      stylistic: true,
    },
  },

  headlessui: {
    prefix: 'Headless',
  },

  ogImage: {
    enabled: false,
  },

  schemaOrg: {
    identity: {
      type: 'Organization',
      name: 'Battleline Productions',
      url: process.env.NUXT_SITE_URL,
      logo: '/battle_logo_small.png',
    },
  },

  scripts: {
    registry: {
      googleAnalytics: true,
      clarity: true,
    },
  },

  // for productioin only scripts
  // $production: {
  //   scripts: {
  //     registry: {
  //       cloudflareWebAnalytics: true,
  //       googleAnalytics: true,
  //       clarity: true,
  //     },
  //   },
  // },
})
{
  "name": "communiqueue",
  "private": true,
  "type": "module",
  "scripts": {
    "build": "nuxt build",
    "dev": "nuxt dev",
    "generate": "nuxt generate",
    "preview": "nuxt preview",
    "postinstall": "nuxt prepare",
    "lint": "eslint .",
    "lint:fix": "eslint --fix ."
  },
  "dependencies": {
    "@headlessui/tailwindcss": "^0.2.1",
    "@headlessui/vue": "^1.7.23",
    "@hebilicious/vue-query-nuxt": "0.3.0",
    "@logto/nuxt": "^1.1.5",
    "@nuxt/eslint": "0.7.4",
    "@nuxt/fonts": "0.10.3",
    "@nuxt/icon": "1.10.3",
    "@nuxt/scripts": "0.9.5",
    "@nuxt/ui": "3.0.0-alpha.10",
    "@nuxtjs/color-mode": "3.5.2",
    "@nuxtjs/seo": "2.0.2",
    "@tailwindcss/postcss": "4.0.0-beta.8",
    "@tanstack/vue-query": "^5.62.9",
    "@vueuse/nuxt": "12.2.0",
    "nuxt": "^3.15.0",
    "tailwindcss": "4.0.0-beta.8",
    "vue": "latest"
  },
  "packageManager": "pnpm@9.15.1",
  "devDependencies": {
    "@nuxt/eslint-config": "^0.7.4",
    "eslint": "^9.17.0",
    "nuxt-headlessui": "^1.2.0",
    "sass": "^1.83.0",
    "sass-loader": "^16.0.4",
    "serverless": "^4.4.18",
    "serverless-cf-invalidate-proxy": "^1.0.1",
    "serverless-domain-manager": "^8.0.0",
    "serverless-plugin-common-excludes": "^4.0.0",
    "serverless-s3-sync": "^3.4.0",
    "typescript": "^5.7.2"
  }
}
`.trim() + '\n\n' // Trim extra whitespace, then add two line breaks

/**
 * The text you wish to append (at the end) of the output file.
 */
const suffixText = `
any time you are helping me build code you need to keep in mind that you are acting as the UI Designer as I have no UI/UX experience. Also we want to build code to be as re-usable as possible so small components are preferred. I also have many components already designed like my header and footer. Are you ready to help me build my application?
`.trim() + '\n' // Trim and add a final newline

/**
 * The output file where everything will be appended.
 */
const OUTPUT_FILE = path.join(process.cwd(), 'all_files_output.txt')

/**
 * Recursively walks through a directory, finding all .ts or .vue files
 * and appending their contents to the output file.
 *
 * @param dir - The directory path to walk.
 */
function walkDirectory(dir) {
  const files = fs.readdirSync(dir)

  for (const file of files) {
    const fullPath = path.join(dir, file)
    const stat = fs.statSync(fullPath)

    // If it's a directory, recurse into it
    if (stat.isDirectory()) {
      walkDirectory(fullPath)
    }
    // If it's a file, check if it ends with .ts or .vue
    else if (stat.isFile()) {
      if (file.endsWith('.ts') || file.endsWith('.vue')) {
        const content = fs.readFileSync(fullPath, 'utf8')
        const relativePath = path.relative(process.cwd(), fullPath)

        // Append file header and contents
        fs.appendFileSync(OUTPUT_FILE, `=== ${relativePath} ===\n`)
        fs.appendFileSync(OUTPUT_FILE, content + '\n\n')
      }
    }
  }
}

// 1. Remove existing output file (if any) so we start fresh
if (fs.existsSync(OUTPUT_FILE)) {
  fs.unlinkSync(OUTPUT_FILE)
}

// 2. Write our prefix text
fs.writeFileSync(OUTPUT_FILE, prefixText)

// 3. Walk the current directory and append all .ts / .vue files
walkDirectory(process.cwd())

// 4. Finally, append the suffix text
fs.appendFileSync(OUTPUT_FILE, suffixText)

console.log(`\nCollected all .ts and .vue files into: ${OUTPUT_FILE}`)
