<template>
    <div class="common-layout">
        <el-container>


            <el-main>
                <div class="maincontent">
                    <el-carousel style="border-radius: 10px; margin-top: 10px" height="300px" direction="vertical"
                        motion-blur :autoplay="true">
                        <el-carousel-item v-for="item in 4" :key="item">
                            <img style="width: 100%;height: 100%;object-fit: cover" src="../../../showimg/image.png">
                        </el-carousel-item>
                    </el-carousel>



                    <div class="bodyclearfix">
                        <div class="boxL">
                            <div class=boxLcontent>
                                <div class=iscenter>
                                    <div class=imgcard>
                                        <img src="../../../avator/avator.png">
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
                                <div v-for="item in pageData.psgArt" :key="item">
                                    <show-box :psgid="item.id" imgsrc="../../artimg/image.png">
                                        <template #article-title>
                                            {{ item.title }}
                                        </template>
                                        <template #articletime>
                                            {{ item.createdDate }}
                                        </template>
                                        <template #articlenum>
                                            {{ item.readCount }}
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
import { useUserStore } from '@/stores/counter';
import { storeToRefs } from 'pinia';

import axios from 'axios';

import { dataType } from 'element-plus/es/components/table-v2/src/common';

const user_store = useUserStore()
const {
    psgCoutNum,
    psgTagNum,
    psgSortNum,
} = storeToRefs(user_store)
let pageData = reactive({

    psgCoutNum: user_store.psgCoutNum,
    psgTagNum: user_store.psgTagNum,
    psgSortNum: user_store.psgSortNum,


});





onMounted(() => {
    //请求商品信息

    //头部header变色

    axios.get('/api/Home/GetPageInfo').then(res => {
        console.log(res.data)
        pageData.psgArt = res.data;
    }).catch(err => {
        console.log(err);
    })

})

</script>

<style scoped>
@import url(../assets/css/HomeView.css);
</style>
