import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'


const routes =[
  {
    path: '/',
    name: 'home',
    component: HomeView
  },
  {
    path: '/ContentPage',
    name: 'ContentPage',
    component: () => import('../views/ContentPageView.vue')
   },


]




const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
