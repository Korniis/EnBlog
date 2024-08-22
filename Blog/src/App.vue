<template>
    <el-header class="header">
        <div class="headerinside">
            <div class="infobar">
                <a style="height: 60px;" href="/">
                    <img style="height: 60px; width: auto;" src="/Title.ico">
                </a>

                <h2 class="style_h2">EnBoWer</h2>
            </div>
            <div v-if="!isLogin" style="display: flex;  ">


                <el-button style="margin: auto 5px;" type="primary" @click="router.push('/Login')">登录</el-button>
                <el-button style="margin: auto 5px;" @click="router.push('/Register')">注册</el-button>
            </div>
            <div v-if="isLogin" style="display: flex;">
                <el-avatar :size="50" style="margin: auto 5px;" />
                {{ username }}
            </div>
        </div>

    </el-header>
    <div id ="App" class="laymain">
        <RouterView></RouterView>

    </div>
</template>
<script setup>
import { RouterLink, RouterView } from 'vue-router'
import router from '@/router';
import { onMounted, onUnmounted, reactive, ref } from 'vue';
import axios from 'axios';

const isLogin = ref(false);
const username = ref("sad");
const viewCount =ref("");
const checkLogin = () => {
    let token = localStorage.getItem("Elog_jwtToken");
    console.log("Token:", token); // 打印令牌
    if (token) {
        isLogin.value = true;
    } else {
        isLogin.value = false;
    }
    console.log("isLogin.value:", isLogin.value); // 打印登录状态
}
const UpWebData =async ()=>{

    axios.get("/api/MainView/UpWebData").then(
        res=>{

            viewCount.value=res.data+1
        }
    )

}

onMounted(() => {

    checkLogin();
    UpWebData();
})
</script>
<style>
body {

    margin: 0;
}

.header {
    background-color: white;
    position: fixed;
    width: 100%;
    top: 0;
    z-index: 1000;
    text-align: center;
    height: 60px;
    line-height: 60px;
    /* background: linear-gradient(to bottom right, rgba(95, 165, 200, 0.949), rgba(38, 117, 213, 0.73)); */
    box-shadow: 0 3px 6px 0 rgba(0, 0, 0, .2);
}

.headerinside {
    display: flex;
    justify-content: space-between;


}

.style_h2 {
    margin: 0;

}

.infobar {
    display: flex;
}

::-webkit-scrollbar {
    display: none;
    /* Chrome Safari */
}

.laymain {
    margin-top: 60px;
    flex: 1;
    position: relative;
    overflow-y: auto;
    background-color: #f0f0f0;
    --el-main-padding: 0;
    background-image: url("../background/leaves.png");
}
</style>
