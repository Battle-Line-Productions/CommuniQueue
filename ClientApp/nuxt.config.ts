// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({

  modules: [
    '@nuxt/eslint',
    '@nuxt/ui',
    '@nuxt/icon',
    '@nuxt/fonts',
    '@logto/nuxt',
    '@nuxtjs/color-mode',
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
