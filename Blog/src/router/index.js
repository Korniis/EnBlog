import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import { ElMessage } from 'element-plus'
import useUser from '@/stores/useUser'


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
    path: '/EditPageView/:pid',
    name: 'EditPageView',
    component: () => import('../views/EditPageView.vue'),
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
  {
    path: '/EmailPasswordReset',
    name: 'EmailPasswordReset',
    component: () => import('../views/AuthPage/EmailPasswordReset.vue')
  },
  {
    path: '/Account',
    name: 'Account',
    component: () => import('../views/MyAccount.vue'),
    redirect: '/Account/ArticleManage',
    children: [
      {
        path: 'InfoPage', // 去掉前面的斜杠
        name: 'InfoPage',
        component: () => import('@/views/MyBack/InfoContent.vue'),
      },
      {
        path: 'AvatarCheck', // 去掉前面的斜杠
        name: 'AvatarCheck',
        component: () => import('@/views/MyBack/AvatarCheck.vue'),
      },
      {
        path: 'PasswordChange', // 去掉前面的斜杠
        name: 'PasswordChange',
        component: () => import('@/views/MyBack/PasswordChange.vue'),
      },
      {
        path: 'ArticleManage', // 去掉前面的斜杠
        name: 'ArticleManage',
        component: () => import('@/views/MyBack/ArticleManage.vue'),
      },
      {
        path: 'MySetting', // 去掉前面的斜杠
        name: 'MySetting',
        component: () => import('@/views/MyBack/MySetting.vue'),
      },
      {
        path: 'EmailChange', // 去掉前面的斜杠
        name: 'EmailChange',
        component: () => import('@/views/MyBack/EmailChange.vue'),
      },
    ]
  },
]


const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach((to, from) => {
  const token = localStorage.getItem("Elog_jwtToken");
  const toPath = to.path.toLowerCase();
  const userStore = useUser();
  // 允许访问的路径数组
  const allowedPaths = ['/', '/login', '/register', '/emailpasswordreset'];

  // 允许访问的路径的正则表达式，包含带参数的路径
  const allowedPathWithParams = /^\/ContentPage\/\d+$/;

  // 检查是否匹配允许访问的路径
  if (!userStore.isLogin && !allowedPaths.includes(toPath) && !allowedPathWithParams.test(toPath)) {
    ElMessage.error("请登录");
    return "/Login";
  }
  if (token && (toPath == "/login" || toPath == "/Register")) {
    return "/";
  }
})

export default router
