import { createConfigForNuxt } from '@nuxt/eslint-config/flat'

export default createConfigForNuxt({
  features: {
    stylistic: true,
  },
  rules: {
    semi: ['error', 'always'],
    'vue/multi-word-component-names': 'off',
  },
})
