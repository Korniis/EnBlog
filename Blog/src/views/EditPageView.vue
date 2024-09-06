<template>
    <div class="writecontent">
        <div class="writeheader">
            <h2>Editcontent</h2>
            <h2>标题: <el-input v-model="articleData.title" style="width: 600px" placeholder="请输入标题" /></h2>
            <h2>类型 : <el-input v-model="articleData.typeName" disabled style="width: 600px" placeholder="请输入标题" /> </h2>
            <el-button style=" margin: auto; width: 70px; " type="primary"
                @click="CreateArticle(articleData)">提交</el-button>
        </div>
        <MdEditor v-model="articleData.content" @onUploadImg="onUploadImg" :toolbars="toolbars" placeholder="请输入内容" />

    </div>

</template>
<script setup>
import { onMounted, reactive, ref } from 'vue';
import { MdEditor } from 'md-editor-v3';
import 'md-editor-v3/lib/style.css';
import router from '@/router';
import useArticles from '@/stores/useArticles';
import useWriter from '@/stores/useWriter'
import { ElButton, ElMessage } from 'element-plus';
import axios from '@/api/index';

const toolbars = [
    'bold',
    'underline',
    'italic',
    '-',
    'strikeThrough',
    'title',
    'sub',
    'sup',
    'quote',
    'unorderedList',
    'orderedList',
    'task', // ^2.4.0
    '-',
    'codeRow',
    'code',
    'link',
    'image',
    'table',
    'mermaid',
    'katex',
    '-',
    'revoke',
    'next',
    'save',
    '=',
    'preview',
    'previewOnly',
    'htmlPreview',
    'catalog',

]
let propsData = defineProps(["pid"])
const textType = ref([


]);

const articleData = reactive({
    id: '',
    title: '',
    content: '',
    typeId: '',
    imgSrc: '',
    typeName: '',

});
const writeStore = useWriter();

const CreateArticle = (article) => {

    writeStore.UpdateArticle(article);



}
const onUploadImg = async (files, callback) => {
    const token = localStorage.getItem("Elog_jwtToken");
    const res = await Promise.all(
        files.map((file) => {
            return new Promise((rev, rej) => {
                const formData = new FormData();
                formData.append('formData', file);

                axios.post('api/Article/UploadImg', formData, {
                    headers: {
                        'Content-Type': 'multipart/form-data',
                        "Authorization": `Bearer ${token}`,
                    }
                })
                    .then((res) => rev(res))
                    .catch((error) => rej(error));
            });
        })
    );

    callback(res.map((item) => {
        if (!articleData.imgSrc) {

            articleData.imgSrc = item.data.data.fileName;
        }
        return axios.defaults.baseURL + "/images/" + item.data.data.fileName
    }));
    // for (let i in files) {
    //     if (files[i].size > 1024 * 1024 * 5) {
    //         ElMessage.error("图片请小于5MB")
    //         return;

    //     }
    //     const formData = new FormData();
    //     formData.append('file', files[i])
    //     axios.post("https://sm.ms/api/v2/upload", {
    //         Header: {
    //             "Content-Type": "multipart/form-data",
    //             "Authorization": "WodY6dUwybVllRGoso6aobrxijWobdjm",
    //         },
    //         smfile: formData

    //     }).then(
    //         res => {

    //             insertImage({ url: res.data })

    //         }
    //     )

    // }


}
const getType = () => {

    axios.get('/api/ArticleType/GetArticleTypes').then(res => {
        textType.value = res.data.data;
    }).catch(error => {
        ElMessage.error('请求失败');
    });

}


onMounted(() => {
    articleData.id = propsData.pid;
    getType();
    axios.get(`api/Article/GetArticleById/${articleData.id}`


    ).then(res => {
        if (res == null) {
            ElMessage("无信息")
        }
        res = res.data

        articleData.title = res.data.title;
        articleData.content = res.data.content;
        articleData.imgSrc = null;
        articleData.typeName = res.data.typeName

        console.log(articleData);

    }).catch(err => {
        ElMessage.error(err.response);
        console.log(err);
    })


})
</script>
<style>
.writecontent {
    padding: 20px;
    background-color: #f9f9f9;
    border-radius: 8px;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.writeheader {
    display: flex;
    flex-wrap: wrap;
    align-items: center;
    margin-bottom: 20px;
}

.writeheader h2 {
    font-size: 18px;
    font-weight: 500;
    margin: 0;
}

.writeheader h2:first-child {
    flex-basis: 100%;
    margin-bottom: 20px;
    font-size: 24px;
}

.writeheader h2+h2 {
    margin-right: 20px;
    display: flex;
    align-items: center;
}

.writeheader h2>.el-input,
.writeheader h2>.el-select {
    margin-left: 10px;
}

.el-button {
    margin-left: auto;
    background-color: #409eff;
    border-color: #409eff;
    color: white;
    border-radius: 4px;
    transition: background-color 0.3s, border-color 0.3s;
}

.el-button:hover {
    background-color: #66b1ff;
    border-color: #66b1ff;
}

.el-button:active {
    background-color: #3a8ee6;
    border-color: #3a8ee6;
}
</style>