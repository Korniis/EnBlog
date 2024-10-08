import router from "@/router";
import axios from "axios";
import { ElMessage } from "element-plus";
import { defineStore } from "pinia";
export const useWriter = defineStore("writer", {

    state: () => ({

        writer: ({

            title: '',
            content: '',
            typeId: '',

        })
    }),

    actions: {

        CreateArticle(article) {
            axios.interceptors.request.use(config => {
                //获取Token
                const token = localStorage.getItem("Elog_jwtToken")

                if (token) {
                    config.headers.Authorization = `Bearer ${token}`
                }
                return config
            })
            this.writer = article;
            //debugger
            axios.post('/api/Article/CreateArticle', {

                title: article.title,
                content: article.content,
                tid: article.typeId,
                imgSrc: article.imgSrc
            }).then(res => {

                if (res.data.code = 200) {
                    ElMessage.success("成功");
                    //router.push("/");
                }
                else if (res.data.code == 500) {
                    ElMessage.error(res.data.message)
                }

            }
            ).catch(error => {
                ElMessage.error("提交失败，标题，类型，文章不能为空");
            }
            )

        },


        UpdateArticle(article) {
            axios.interceptors.request.use(config => {
                //获取Token
                const token = localStorage.getItem("Elog_jwtToken")

                if (token) {
                    config.headers.Authorization = `Bearer ${token}`
                }
                return config
            })
            this.writer = article;
            axios.put('/api/Article/UpdateArticle', {
                tid: article.id,
                title: article.title,
                content: article.content,
                imgSrc: article.imgSrc
            }).then(res => {

                if (res.data.code = 200) {
                    ElMessage.success("成功");
                    router.back();
                }
                else if (res.data.code == 500) {
                    ElMessage.error(res.data.message)
                }

            }
            ).catch(error => {
                ElMessage.error("提交失败，标题，类型，文章不能为空");
            }
            )

        }
    }

})
export default useWriter