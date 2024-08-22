import router from '@/router';
import axios from 'axios'
import { ElMessage } from 'element-plus';
axios.defaults.baseURL = 'https://localhost:7186';
axios.defaults.headers['X-Request-With'] = "XMLHttpRequest";
axios.defaults.headers.post['Content-Type'] = 'application/json';
axios.interceptors.response.use(
    function (response) {
        // 如果响应成功，直接返回响应
        return response;
    },
    function (error) {
        // 如果响应的状态码是 401
        if (error.response && error.response.status === 401) {
            // 执行自定义的处理，例如重定向到登录页面
            ElMessage.error("请登录");
            router.push('/login');
            return ;
            // 例如：window.location.href = '/login';
        }
        // 将错误传递到下一个拦截器
        return Promise.reject(error);
    }
);

export default axios;