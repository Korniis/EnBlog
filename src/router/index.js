import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/home/HomeView.vue'


const routes =[
  {
    path: '/',
    name: 'home',
    component: HomeView
  },
  {
    path: '/shop',
    name: 'shop',
    component: () => import('../views/shop/ShopView.vue')
  },


]




const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
