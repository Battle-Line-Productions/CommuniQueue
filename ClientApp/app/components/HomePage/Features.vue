<template>
  <div class="bg-light-background dark:bg-dark-background py-24 sm:py-32">
    <div class="mx-auto max-w-7xl px-6 lg:px-8">
      <div class="mx-auto max-w-2xl lg:text-center">
        <h2 class="text-base font-semibold leading-7 text-light-primary dark:text-dark-primary">Streamline Your Communication</h2>
        <p class="mt-2 text-3xl font-bold tracking-tight text-light-textbase dark:text-dark-textbase sm:text-4xl">Everything you need to manage your communications</p>
        <p class="mt-6 text-lg leading-8 text-light-secondary dark:text-dark-secondary"
          >CommuniQueue provides a powerful platform for creating, organizing, and sending templated emails and texts effortlessly. Boost your productivity and streamline your
          communication process.</p
        >
      </div>
      <div class="mx-auto mt-16 max-w-2xl sm:mt-20 lg:mt-24 lg:max-w-none">
        <dl class="grid max-w-xl grid-cols-1 gap-x-8 gap-y-16 lg:max-w-none lg:grid-cols-3">
          <div v-for="feature in features" :key="feature.name" class="flex flex-col">
            <dt class="flex items-center gap-x-3 text-base font-semibold leading-7 text-light-textbase dark:text-dark-textbase">
              <NuxtIcon :name="feature.icon" class="h-5 w-5 flex-none text-light-primary dark:text-dark-primary" aria-hidden="true" />
              {{ feature.name }}
            </dt>
            <dd class="mt-4 flex flex-auto flex-col text-base leading-7 text-light-secondary dark:text-dark-secondary">
              <p class="flex-auto">{{ feature.description }}</p>
              <p class="mt-6">
                <NuxtLink :to="feature.href" class="text-sm font-semibold leading-6 text-light-primary dark:text-dark-primary"
                  >Learn more <span aria-hidden="true">→</span></NuxtLink
                >
              </p>
            </dd>
          </div>
        </dl>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
const features = [
  {
    name: 'Template Management',
    description:
      'Easily create and organize your communication templates. Our intuitive interface allows you to design and categorize templates for various purposes, ensuring you always have the right message ready.',
    href: '/features/template-management',
    icon: 'i-heroicons-document-text'
  },
  {
    name: 'Effortless Sending',
    description:
      'Send emails and texts with just a few clicks. Our streamlined process allows you to select a template, customize it if needed, and send it to your recipients quickly and efficiently.',
    href: '/features/effortless-sending',
    icon: 'i-heroicons-paper-airplane'
  },
  {
    name: 'Secure API',
    description:
      'Integrate our robust API for automated communications. Our secure API allows you to connect CommuniQueue with your existing systems, enabling automated sending of personalized messages at scale.',
    href: '/features/secure-api',
    icon: 'i-heroicons-lock-closed'
  }
];
</script>
