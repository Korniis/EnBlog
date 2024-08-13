<template>

    <div class="titlecontent">
        <div class="titlemusk">
            <div class="title">
                <span v-html="psgArt.title"></span>
            </div>
            <div id="post-meta">
                <div class="meta-firstline">
                    <span class="post-meta-date">
                        <span class="post-meta-label">发表于</span>
                        <time class="post-meta-date-created" :datetime="psgArt.createdDate"
                            :title="psgArt.createdDate">{{ psgArt.createdDate }}</time>
                        <span class="post-meta-separator">|</span>

                    </span>
                </div>
                <div class="meta-secondline">
                    <span class="post-meta-separator">|</span>
                    <span class="post-meta-wordcount">
                        <span class="post-meta-label">字数总计:</span>
                        <span class="word-count">48</span>
                        <span class="post-meta-separator">|</span>
                        <span class="post-meta-label">阅读时长:</span>
                        <span>1分钟</span>
                    </span>
                    <span class="post-meta-separator">|</span>
                    <span class="post-meta-pv-cv" id="" data-flag-title="">
                        <span class="post-meta-label">阅读量:</span>
                        <span id="busuanzi_value_page_pv">110</span>
                    </span>
                </div>
            </div>

        </div>
    </div>
    <div class="atrticlemain">
        <div class="bodyclearfix">
            <div class="boxL">
                <div class=boxLcontent>
                    <div class=iscenter>
                        <div class=imgcard>
                            <img src="../../public/avator/avator.png">
                        </div>
                        <div class="infoname">
                            EnBoWer
                        </div>
                    </div>
                    <div class="infodata">
                        <a href='/'>
                            <div class="dataname">文章</div>
                            <div class="datanum">{{ psgCoutNum }}</div>
                        </a>
                        <a href='/' style="text-decoration:none">

                            <div class="dataname">标签</div>
                            <div class="datanum">{{ psgTagNum }}</div>
                        </a>
                        <a href='/' style="text-decoration:none">

                            <div class="dataname">分类</div>
                            <div class="datanum">{{ psgSortNum }}</div>
                        </a>
                    </div>

                    <el-button type="primary">
                        <span>Follow Me</span>
                    </el-button>

                </div>
                <div class="boxLcontent" style="margin-top: 20px;">
                    jaasfasf
                </div>


            </div>
            <div class="boxR">
                <div class="boxRcontent">
                    <span v-html="psgArt.content"></span>
                </div>
            </div>
        </div>
    </div>
</template>
<script setup>
import { computed, onMounted, reactive } from 'vue';
import { useUser } from '@/stores/counter';
import axios from '../api/index';
import { storeToRefs } from 'pinia';
import { ElMessage } from 'element-plus';
const user_store = useUser()
// const {

//     psgCoutNum,
//     psgTagNum,
//     psgSortNum,
// } = storeToRefs(user_store)
let pageData = reactive({

    // psgCoutNum: user_store.psgCoutNum,
    // psgTagNum: user_store.psgTagNum,
    // psgSortNum: user_store.psgSortNum,




});
let propsData = defineProps(["pid"])
let psgArt = reactive({
    id: 0,
    title: "",
    content: "",
    createdDate: "",
    readCount: 1
});

onMounted(() => {
    psgArt.id=propsData.pid;
    axios.get(`api/Article/GetArticleById/${psgArt.id}`


    ).then(res => {
        if(res==null)
    {
        ElMessage("无信息")
    }
    res=res.data
        psgArt.id = res.data.id;
        psgArt.title = res.data.title;
        psgArt.content = res.data.content;
        psgArt.createdDate = res.data.createTime;
        psgArt.readCount = res.data.viewCount;
        console.log(psgArt);

    }).catch(err => {
        ElMessage.error(err.response);
        console.log(err);
    })


})

</script>

<style>
@import url(../assets/css/ContentPageView.css);
</style>