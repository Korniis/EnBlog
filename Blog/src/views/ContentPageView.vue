<template>

    <div class="titlecontent">
        <div class="titlemusk">
            <div class="title">
                无题
            </div>
            <div id="post-meta">
    <div class="meta-firstline">
        <span class="post-meta-date">
            <span class="post-meta-label">发表于</span>
            <time class="post-meta-date-created" datetime="2022-12-01T14:10:23.280Z" title="发表于 2022-12-01 22:10:23">2022-12-01</time>
            <span class="post-meta-separator">|</span>
            <span class="post-meta-label">更新于</span>
            <time class="post-meta-date-updated" datetime="2022-12-03T07:04:27.399Z" title="更新于 2022-12-03 15:04:27">2022-12-03</time>
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


                            </div>
                        </div>
                    </div></div>
</template>
<script setup>
import { computed, onMounted, reactive } from 'vue';
import { useUserStore } from '@/stores/counter';
import { storeToRefs } from 'pinia';
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


</script>

<style>
@import url(../assets/ContentPageView.css);
.titlecontent {
    height: 400px;
    background-image: url("../../artimg/image.png");

    width: 100%;
    background-color: #49b1f5;
    background-position: center center;
    background-size: cover;
    background-repeat: no-repeat;
}

.titlemusk {
    height: 100%;
    display: block;
    color: #fff;
    backdrop-filter: brightness(60%);
    position: relative;
    z-index: 2;
    text-align: center;
    box-sizing: border-box;

    padding: 0 8px;
    display: flex;
    /* 设置为 Flex 容器 */
    align-items: center;
    /* 垂直居中 */
    justify-content: center;
    flex-direction: column;
}
.meta-firstline
{
    margin: 2px;
}
.title {

    width: 100%;
    margin-bottom: 8px;
    font-weight: normal;
    font-size: 2.5em;
    line-height: 1.5;
    -webkit-line-clamp: 3;


}
</style>