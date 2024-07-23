
<template>
       <el-header class="header">
                <div class="headerinside">
                    <div class="infobar">
                        <a style="height: 60px;" href="">
                            <img style="height: 60px; width: auto;" src="/Title.ico">
                        </a>

                        <h2 class="style_h2">EnBoWer</h2>
                    </div>
                    <div>
                        <el-button type="primary">登录</el-button>
                        <el-button>注册</el-button>
                    </div>
                </div>

            </el-header>
            <div class="laymain">
 <RouterView></RouterView></div>
</template>
<script setup>
import { RouterLink, RouterView } from 'vue-router'

</script>
<style >
body{

    margin: 0;
}
.header {

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
.laymain
{
    margin-top: 60px;
    flex: 1;
    position: relative;
    overflow-y: auto;
    background-color: #f0f0f0;
    --el-main-padding: 0;
    background-image: url("background/leaves.png");
}
</style>
