import axios from 'axios'
axios.defaults.baseURL = 'http://localhost:5173';
axios.defaults.headers['X-Request-With'] = "XMLHttpRequest";
axios.defaults.headers.post['Content-Type'] = 'application/json';
export default axios;