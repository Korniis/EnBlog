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
//   server: {
//     // 代理
//     proxy: {
//         '/shop': {
//             target: 'https://localhost:7099', // 代理后台服务器地址
//             changeOrigin: true, //允许跨域
//            // ws: true,
//             //secure: true,
//             secure: false,
//             rewrite: path => path.replace(/^\/shop/,'api') // 将请求地址中的 /ok 替换成空
//         }
//     }
// }
})
