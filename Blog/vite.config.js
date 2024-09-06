import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [
    vue(),
  ],

  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  },
  server: {
    // 代理
    proxy: {
      '/api': {
        target: 'https://localhost:7186/api', // 代理后台服务器地址
        changeOrigin: true, // 允许跨域
        secure: false,
        rewrite: path => path.replace(/^\/api/, ''), // 将请求地址中的 /api 替换成空
        bypass(req, res, options) {
          const realUrl = options.target + (options.rewrite ? options.rewrite(req.url) : '');
          console.log(realUrl); // 在终端显示
          res.setHeader('A-Real-Url', realUrl);
        }
      },

    }

  }
})
