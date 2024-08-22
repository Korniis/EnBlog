<template>
    <div class="writecontent">
        <div class="writeheader">
            <h2>writecontent</h2>
            <h2>标题: <el-input v-model="articleData.title" style="width: 600px" placeholder="请输入标题" /></h2>
            <h2>类型 : <el-select v-model="articleData.typeId" placeholder="文章类型" size="large" style="width: 240px">
                    <el-option v-for="item in textType" :key="item.value" :label="item.typeName" :value="item.id" />
                </el-select> </h2>
            <el-button style=" margin: auto; width: 70px; " type="primary"
                @click="CreateArticle(articleData)">提交</el-button>
        </div>
        <MdEditor v-model="articleData.content" />
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
import axios from 'axios';



const textType = ref([


]);

const articleData = reactive({
    title: '',
    content: '',
    typeId: '',


});
const writeStore = useWriter();

const CreateArticle = (article) => {

    writeStore.CreateArticle(article);

}

const getType = () => {

    axios.get('/api/ArticleType/GetArticleTypes').then(res => {
        textType.value = res.data.data;
    }).catch(error => {
        ElMessage.error('请求失败');
    });

}


onMounted(() => {

    getType();

})
</script>
<style>
.writeheader {
    display: flex;
    flex-direction: row;
    padding: 2px;

}

.writeheader>* {


    margin-right: 50px;
}
</style>