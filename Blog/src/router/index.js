import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import { ElMessage } from 'element-plus'


const routes = [
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
    component: () => import('../views/AuthPage/LoginView.vue')
  },
  {
    path: '/Register',
    name: 'Register',
    component: () => import('../views/AuthPage/RegisterView.vue')
  },
  {
    path: '/Write',
    name: 'Write',
    component: () => import('../views/WritePageView.vue')
  },
]


const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach((to, from) => {
  const token = localStorage.getItem("Elog_jwtToken");

  // 允许访问的路径数组
  const allowedPaths = ['/', '/Login', '/Register'];

  // 允许访问的路径的正则表达式，包含带参数的路径
  const allowedPathWithParams = /^\/ContentPage\/\d+$/;

  // 检查是否匹配允许访问的路径
  if (!token && !allowedPaths.includes(to.path) && !allowedPathWithParams.test(to.path)) {
    ElMessage.error("请登录");
    return "/Login";
  }
})

export default router
