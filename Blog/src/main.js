
import * as ElementPlusIconsVue from '@element-plus/icons-vue'

import axios from 'axios'

import { createApp } from 'vue'
import { createPinia } from 'pinia'
import App from './App.vue'
import router from './router'
import piniaPluginPersist from 'pinia-plugin-persist'

import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
const app = createApp(App)

const pinia=createPinia().use(piniaPluginPersist)

app.use(router)
app.use(pinia)
app.use(ElementPlus)

app.mount('#app')
