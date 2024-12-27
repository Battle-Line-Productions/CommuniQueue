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
