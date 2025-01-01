<script lang="ts" setup>
const config = useRuntimeConfig()

const githubRepoOwner = 'Battle-Line-Productions'
const githubRepoName = 'BattleSites'
const githubToken = config.public.githubPat

// Form data
const formData = ref({
  name: '',
  email: '',
  type: 'feature',
  description: '',
})

// Handle form submission
const handleSubmit = async () => {
  const issueTitle = `${formData.value.type === 'feature' ? 'Feature Request' : 'Bug Report'}: ${formData.value.name}`
  const issueBody = `
    **Description:**
    ${formData.value.description}

    **Submitted by:** ${formData.value.name} (${formData.value.email})
  `

  const githubIssueData = {
    title: issueTitle,
    body: issueBody,
    labels: [formData.value.type === 'feature' ? 'feature request' : 'bug'],
  }

  try {
    const response = await fetch(`https://api.github.com/repos/${githubRepoOwner}/${githubRepoName}/issues`, {
      method: 'POST',
      headers: {
        'Authorization': `token ${githubToken}`,
        'Accept': 'application/vnd.github.v3+json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(githubIssueData),
    })

    if (response.ok) {
      alert('Thank you for your submission! The issue has been created.')
    }
    else {
      console.error('GitHub API error:', await response.json())
      alert('There was an issue creating the GitHub issue.')
    }
  }
  catch (error) {
    console.error('Submission error:', error)
    alert('There was an error submitting the form.')
  }

  // Clear form after submission
  formData.value = {
    name: '',
    email: '',
    type: 'feature',
    description: '',
  }
}
</script>

<template>
  <section class="py-16 bg-light-background dark:bg-dark-background">
    <div class="container mx-auto px-4">
      <!-- Make sure it has the same container and padding -->
      <h2 class="text-3xl font-bold text-center text-light-textbase dark:text-dark-textbase mb-6">
        Request a Feature or Report a Bug
      </h2>
      <form
        class="w-full bg-light-surface dark:bg-dark-surface p-6 rounded-lg shadow-lg"
        @submit.prevent="handleSubmit"
      >
        <!-- w-full to take up full width -->
        <div class="mb-4">
          <label
            for="name"
            class="block text-sm font-medium text-light-textbase dark:text-dark-textbase"
          >Name</label>
          <input
            id="name"
            v-model="formData.name"
            type="text"
            class="mt-1 block w-full border border-gray-300 rounded-md shadow-xs focus:ring-primary focus:border-primary bg-light-background dark:bg-dark-background text-light-textbase dark:text-dark-textbase p-2"
            required
          >
        </div>

        <div class="mb-4">
          <label
            for="email"
            class="block text-sm font-medium text-light-textbase dark:text-dark-textbase"
          >Email</label>
          <input
            id="email"
            v-model="formData.email"
            type="email"
            class="mt-1 block w-full border border-gray-300 rounded-md shadow-xs focus:ring-primary focus:border-primary bg-light-background dark:bg-dark-background text-light-textbase dark:text-dark-textbase p-2"
            required
          >
        </div>

        <div class="mb-4">
          <label
            for="type"
            class="block text-sm font-medium text-light-textbase dark:text-dark-textbase"
          >Request
            Type</label>
          <select
            id="type"
            v-model="formData.type"
            class="mt-1 block w-full border border-gray-300 rounded-md shadow-xs focus:ring-primary focus:border-primary bg-light-background dark:bg-dark-background text-light-textbase dark:text-dark-textbase p-2"
            required
          >
            <option value="feature">
              Feature Request
            </option>
            <option value="bug">
              Bug Report
            </option>
            <option value="bug">
              Just Sayin Hey!
            </option>
          </select>
        </div>

        <div class="mb-4">
          <label
            for="description"
            class="block text-sm font-medium text-light-textbase dark:text-dark-textbase"
          >Description</label>
          <textarea
            id="description"
            v-model="formData.description"
            class="mt-1 block w-full border border-gray-300 rounded-md shadow-xs focus:ring-primary focus:border-primary bg-light-background dark:bg-dark-background text-light-textbase dark:text-dark-textbase p-2"
            rows="4"
            required
          />
        </div>

        <UButton
          type="submit"
          class="w-full bg-light-primary dark:bg-dark-primary text-white font-bold py-2 px-4 rounded-full hover:bg-light-accent dark:hover:bg-dark-accent transition-colors"
        >
          Submit
        </UButton>
      </form>
    </div>
  </section>
</template>
