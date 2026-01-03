import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    allowedHosts: ["gutturalized-explanatory-jeff.ngrok-free.dev"],
    proxy: {
      '/api': {
        target: 'http://localhost:5258',
        changeOrigin: true,
        secure: false,
      }
    }
  }
})
