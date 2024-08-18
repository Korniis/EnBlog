import axios from "axios";
import { defineStore } from "pinia";

const useArticles = defineStore("articles", {
    state: () => ({
        articles: []
    }),
    actions: {
        getArticles() {
            //配置请求头携带Jwt令牌
            axios.interceptors.request.use(config => {
                //获取Token
                const token = localStorage.getItem("Elog_jwtToken")

                if (token) {
                    config.headers.Authorization = `Bearer ${token}`
                }
                return config
            })
            axios.get('https://localhost:7239/api/Article/GetArticles/Articles').then(res => {
                this.articles = res.data.data
            })
        },
        // CreateArticle(articleData) {
        //     //配置请求头携带Jwt令牌
        //     axios.interceptors.request.use(config => {
        //         //获取Token
        //         const token = localStorage.getItem("Elog_jwtToken")

        //         if (token) {
        //             config.headers.Authorization = `Bearer ${token}`
        //         }
        //         return config
        //     })
        //     axios.get('https://localhost:7239/api/Article/GetArticles/Articles').then(res => {
        //         this.articles = res.data.data
        //     })

        // }
    }
})

export default useArticles