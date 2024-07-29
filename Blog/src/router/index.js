import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'


const routes =[
  {
    path: '/',
    name: 'home',
    component: HomeView
  },
  {
    path: '/ContentPage/:pid',
    name: 'ContentPage',
    component: () => import('../views/ContentPageView.vue'),
    props: true
   },
   {
    path: '/Login',
    name: 'Login',
    component:() => import('../views/AuthPage/LoginView.vue')
  },
  {
    path: '/Register',
    name: 'Register',
    component:() => import('../views/AuthPage/RegisterView.vue')
  },

]




const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
