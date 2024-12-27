<template>
  <div class="relative isolate overflow-hidden bg-light-background dark:bg-dark-background">
    <!-- Background pattern -->
    <svg
      aria-hidden="true"
      class="absolute inset-0 -z-10 h-full w-full stroke-light-primary/10 dark:stroke-dark-primary/10 [mask-image:radial-gradient(100%_100%_at_top_right,white,transparent)]"
    >
      <defs>
        <pattern
          id="983e3e4c-de6d-4c3f-8d64-b9761d1534cc"
          width="200"
          height="200"
          x="50%"
          y="-1"
          patternUnits="userSpaceOnUse"
        >
          <path
            d="M.5 200V.5H200"
            fill="none"
          />
        </pattern>
      </defs>
      <rect
        width="100%"
        height="100%"
        stroke-width="0"
        fill="url(#983e3e4c-de6d-4c3f-8d64-b9761d1534cc)"
      />
    </svg>

    <!-- Gradient blob -->
    <div
      aria-hidden="true"
      class="absolute left-[calc(50%-4rem)] top-10 -z-10 transform-gpu blur-3xl sm:left-[calc(50%-18rem)] lg:left-48 lg:top-[calc(50%-30rem)] xl:left-[calc(50%-24rem)]"
    >
      <div
        class="aspect-[1108/632] w-[69.25rem] bg-gradient-to-r from-light-primary to-light-accent dark:from-dark-primary dark:to-dark-accent opacity-20"
        style="
          clip-path: polygon(
            73.6% 51.7%,
            91.7% 11.8%,
            100% 46.4%,
            97.4% 82.2%,
            92.5% 84.9%,
            75.7% 64%,
            55.3% 47.5%,
            46.5% 49.4%,
            45% 62.9%,
            50.3% 87.2%,
            21.3% 64.1%,
            0.1% 100%,
            5.4% 51.1%,
            21.4% 63.9%,
            58.9% 0.2%,
            73.6% 51.7%
          );
        "
      />
    </div>

    <div class="mx-auto max-w-7xl px-6 py-24 sm:py-32 lg:flex lg:items-center lg:gap-x-10 lg:px-8 lg:py-40">
      <div class="mx-auto max-w-2xl lg:mx-0 lg:flex-auto">
        <h1 class="mt-10 max-w-lg text-4xl font-bold tracking-tight text-light-textbase dark:text-dark-textbase sm:text-6xl">
          Streamline Your Communication with CommuniQueue
        </h1>
        <p class="mt-6 text-lg leading-8 text-light-secondary dark:text-dark-secondary">
          Create, organize, and send emails and texts effortlessly using templates
        </p>
        <div class="mt-10 flex items-center gap-x-6">
          <NuxtLink
            to="/get-started"
            class="rounded-md bg-light-primary dark:bg-dark-primary px-3.5 py-2.5 text-sm font-semibold text-white shadow-sm hover:bg-light-accent dark:hover:bg-dark-accent focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-light-primary dark:focus-visible:outline-dark-primary transition duration-300"
          >
            Get Started
          </NuxtLink>
          <NuxtLink
            to="/features"
            class="text-sm font-semibold leading-6 text-light-textbase dark:text-dark-textbase"
          >
            Learn more <span aria-hidden="true">→</span>
          </NuxtLink>
        </div>
      </div>
      <div class="mt-16 sm:mt-24 lg:mt-0 lg:flex-shrink-0 lg:flex-grow">
        <img
          alt="CommuniQueue App Screenshot"
          src="/assets/images/communiqueueLogo.png"
          class="mx-auto w-full max-w-[48rem] rounded-md shadow-2xl ring-1 ring-white/10"
        >
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
// You can add any necessary imports or logic here
</script>
