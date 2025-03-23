import Tailwind from '@tailwindcss/vite'

// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({

  modules: [
    '@logto/nuxt',
    '@nuxt/eslint',
    '@nuxt/icon',
    '@nuxt/fonts',
    '@nuxtjs/color-mode',
    '@hebilicious/vue-query-nuxt',
    '@nuxt/scripts',
    '@nuxtjs/seo',
    '@vueuse/nuxt',
    '@nuxt/ui-pro',
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

  css: [
    './app/assets/css/tailwind.css',
  ],

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
    preference: 'system',
    fallback: 'light',
    dataValue: 'theme',
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
    // NUXT_API_BASE_URL=<your-url>
    apiBaseUrl: '',
    logto: {
      endpoint: 'https://t7eamt.logto.app',
      appId: '', // NUXT_LOGTO_APP_ID=<your-id>
      appSecret: '', // NUXT_LOGTO_APP_SECRET=<your-secret
      cookieEncryptionKey: '', // NUXT_LOGTO_COOKIE_ENCRYPTION_KEY
      fetchUserInfo: true,
      postLogoutRedirectUri: '/',
      postCallbackRedirectUri: '/auth/logging-in',
      scopes: ['openid', 'profile', 'email', 'organizations', 'custom_data', 'phone'],
    },
  },

  future: {
    compatibilityVersion: 4,
  },

  experimental: {
    componentIslands: true,
  },

  compatibilityDate: '2024-11-01',

  vite: {
    plugins: [
      Tailwind(),
    ],
  },

  typescript: {
    shim: false,
    strict: true,
  },

  eslint: {
    config: {
      stylistic: true,
    },
  },

  logto: {
    postCallbackRedirectUri: '/auth/logging-in',
    postLogoutRedirectUri: '/',
    fetchUserInfo: true,
    resources: ['https://dev-tenant.battlelineproductions.com', "http://localhost:5000"],
    scopes: ['openid', 'profile', 'email', 'organizations', 'custom_data', 'phone', 'write:resource'],
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

  uiPro: {
    license: process.env.NUXT_UI_PRO_LICENSE,
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
