import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      '/api': 'http://localhost:5011'
    }
  },
  build: {
    outDir: 'dist',
    minify: false, // Wyłącza minifikację całkowicie
    terserOptions: {
      mangle: false // Wyłącza mangling, czyli zmianę nazw zmiennych
    }
  }
})
