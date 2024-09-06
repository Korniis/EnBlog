import router from '@/router';
import useUser from '@/stores/useUser';
import axios from 'axios'
import { ElMessage } from 'element-plus';
axios.defaults.baseURL = 'https://localhost:7186';

axios.defaults.headers.post['Content-Type'] = 'application/json';
axios.interceptors.response.use(
    function (response) {
        // 如果响应成功，直接返回响应
        return response;
    },
    function (error) {
        const userStore = useUser();

        // 如果响应的状态码是 401
        if (error.response && error.response.status === 401) {
            // 执行自定义的处理，例如重定向到登录页面

            if (userStore.isLogin == true) {
                ElMessage.error("信息修改或在其他位置登录")
            }
            userStore.isLogin = false;
            localStorage.removeItem("Elog_jwtToken");
            router.push('/login');
            return;
            // 例如：window.location.href = '/login';
        }
        // 将错误传递到下一个拦截器
        return Promise.reject(error);
    }
);

export default axios;