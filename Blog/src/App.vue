<template>
    <div ref="backwave" style="height: 100vh">
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
                <div v-if="isLogin">
                    <a href='/Account' class="avatar-link" style="display: flex;">
                        {{ username }}
                        <el-avatar :src="axios.defaults.baseURL + '/images/' + avatar" :size="50"
                            style="margin: auto 10px;" />

                    </a>
                </div>
            </div>

        </el-header>

        <div id="App" class="laymain">
            <RouterView></RouterView>

        </div>
    </div>
</template>
<script setup>
import { RouterLink, RouterView } from 'vue-router'
import router from '@/router';
import { computed, onMounted, onUnmounted, reactive, ref } from 'vue';
import axios from '@/api/index';
import useUser from './stores/useUser';
import { jwtDecode } from 'jwt-decode';
import { ElMessage } from 'element-plus';
import * as THREE from "three";
import NET from 'vanta/dist/vanta.net.min'
const userStroe = useUser();
const isLogin = computed(() => { return userStroe.isLogin });
const username = computed(() => { return userStroe.user.userName });
const avatar = computed(() => { return userStroe.user.avatar });

const backwave = ref();
let vantaEffect;

const viewCount = ref("");
const checkLogin = () => {
    let token = localStorage.getItem("Elog_jwtToken");
    console.log("Token:", token); // 打印令牌
    if (token) {
        userStroe.isLogin = true;
    } else {
        userStroe.isLogin = false;
    }
    console.log("isLogin.value:", userStroe.user.isLogin); // 打印登录状态
}
// const UpWebData = async () => {

//     axios.get("/api/MainView/UpWebData").then(
//         res => {

//             viewCount.value = res.data + 1
//         }
//     )

// }
// const userInfoGet = async () => {
//     let token = localStorage.getItem("Elog_jwtToken");

//     if (token) {
//         axios.get("api/User/GetUserByFont", {
//             headers: {
//                 "Authorization": `Bearer ${token}`
//             }
//         })
//             .then(res => {

//                 userStroe.user.username=res.data.userName;
//                 userStroe.user.avatar=res.data.avatar;
//                 userStroe.user.userid=res.data.id;
//             })
//             .catch(error => {
//                 ElMessage, error(error);
//             });
//     }
// }


onMounted(() => {

    checkLogin();
    //userInfoGet();
    userStroe.userInfoGet();
    vantaEffect = NET({
        el: backwave.value,
        THREE,

        mouseControls: true,
        touchControls: true,
        gyroControls: false,
        minHeight: 500.0,
        minWidth: 200.0,
        scale: 1.0,
        scaleMobile: 1.0,

        color: 0x87ceeb, // 使用浅蓝色 (Light Sky Blue) 作为网格颜色
        backgroundColor: 0xf0f8ff,
    })


    // vantaEffect = WAVES({
    //     el: backwave.value,
    //     THREE: THREE,
    //     mouseControls: true,
    //     touchControls: true,
    //     gyroControls: false,
    //     minHeight: 200.00,
    //     minWidth: 200.00,
    //     scale: 1.00,
    //     scaleMobile: 1.00
    // });

});




onUnmounted(() => {
    if (vantaEffect) {
        vantaEffect.destroy();
    }
});

</script>
<style>
#vanta-background {
    position: fixed;
    top: 0;
    left: 0;
    width: 100vw;
    height: 100vh;
    z-index: -1;
    /* 确保背景位于其他内容之后 */
}

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
    --el-main-padding: 0;
    /* background-image: url("../background/leaves.png"); */
}

.avatar-link {
    display: flex;
    text-decoration: none;
    /* 去掉下划线 */
    color: #333;
    /* 更改字体颜色，可以根据需求调整 */
    align-items: center;
    /* 确保头像和文本垂直居中对齐 */
}

.avatar-link:hover {
    color: #49b1f5;
    /* 添加悬停效果 */
}
</style>
