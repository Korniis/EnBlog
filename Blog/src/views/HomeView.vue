<template>
    <div class="common-layout">
        <el-container>


            <el-main>
                <div class="maincontent">
                    <el-carousel style="border-radius: 10px; margin-top: 10px" height="300px" direction="vertical"
                        motion-blur :autoplay="true">
                        <el-carousel-item v-for="item in 4" :key="item">
                            <img style="width: 100%;height: 100%;object-fit: cover"
                                src="../../public/showimg/image.png">
                        </el-carousel-item>
                    </el-carousel>



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
                            <div v-infinite-scroll="loadmore"   :infinite-scroll-disabled=pageData.isStop infinite-scroll-immediate=false class="boxRcontent">
                                <div  style="overflow: auto" v-for="item in pageData.psgArt" :key="item">
                                    <show-box :psgid="item.id" imgsrc="../../public/artimg/image.png">
                                        <template #article-title>
                                            {{ item.title }}
                                        </template>
                                        <template #articletime>
                                            {{ item.createTime }}
                                        </template>
                                        <template #articlenum>
                                            {{ item.viewCount }}
                                        </template> <template #artcontent>
                                            <span v-html="item.content"></span>
                                        </template>
                                    </show-box>
                                </div>
                            </div>
                        </div>
                    </div>


                </div>


            </el-main>
        </el-container>
    </div>
</template>

<script setup>
import { computed, onMounted, reactive } from 'vue';
import ShowBox from '../components/ShowBox.vue';
import { useUser } from '@/stores/counter';
import { storeToRefs } from 'pinia';

import axios from '@/api/index';

import { dataType } from 'element-plus/es/components/table-v2/src/common';
import { ElMessage } from 'element-plus';

const user_store = useUser()
const {
    psgCoutNum,
    psgTagNum,
    psgSortNum,
} = storeToRefs(user_store)
let pageData = reactive({
    psgCoutNum: user_store.psgCoutNum,
    psgTagNum: user_store.psgTagNum,
    psgSortNum: user_store.psgSortNum,
    psgArt: [], // 存储文章列表
    pageIndex: 1 ,// 当前页码
    isStop:false

});


const loadmore = ()=>{

    axios.get(`/api/MainView/GetArticle/${pageData.pageIndex}`)
        .then(res => {
            if (res.data.data && res.data.data.length > 0) {
                // 将新加载的数据追加到现有的数据中
                pageData.psgArt.push(...res.data.data);
                // 页码自增
                pageData.pageIndex += 1;
            } else {
                ElMessage.error("没有更多数据了");
                pageData.isStop=true;
            }
        })
        .catch(err => {
            console.log(err);
            ElMessage.error("加载数据时出错");
        });
}

onMounted(() => {


    //头部header变色

    axios.get(`/api/MainView/GetArticle/${0}`).then(res => {
        console.log(res.data)
        pageData.psgArt = res.data.data;
    }).catch(err => {
        console.log(err);
    })

})

</script>

<style scoped>
@import url(../assets/css/HomeView.css);
</style>
