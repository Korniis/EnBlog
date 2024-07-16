<template>
    <div class="common-layout">
        <el-container>
            <el-header class="header">
                <div class="headerinside">
                    <a style="height: 60px;" href="">
                        <img style="height: 60px; width: auto;" src="/Title.ico">
                    </a>

                    <h2 :style="style_h2">EnBoWer</h2>

                    <div>
                        <el-button type="primary">登录</el-button>
                        <el-button>注册</el-button>
                    </div>
                </div>

            </el-header>
            <el-main class="main">
                <div class="content">
                   


                </div>

            </el-main>

        </el-container>
    </div>
</template>

<script setup>
import { onMounted, reactive } from 'vue';
import ShowBox from '../../components/ShowBox.vue'
import axios from 'axios';
//绑定测试
let style_h2 = reactive({
    margin: '0px',
    color: "",
})

let color = ['#19CAAD ', '#D6D5B7', ' #8CC7B5', '#D1BA74', '#A0EEE1 ', " #E6CEAC", '#BEE7E9', ' #ECAD9E', '#BEEDC7 ', '#F4606C']
//页面信息
let state = reactive({

    goodsData: " ", //商品信息

})
// const getGoodsData = () => {
//     axios.get('/shop/Home/GetList').then(res => { //请求后端


//         console.log(res.data)
//         state.goodsData = res.data;
//     })


// }
onMounted(() => {
    //请求商品信息

    getGoodsData();
    //头部header变色
    let timer = setInterval(() => {
        style_h2.color = color[Math.floor(Math.random() * 10)];
    }, 500)


})

</script>

<style scoped>
.common-layout {
    height: 100vh;
    display: flex;

    margin: 0;
    overflow: hidden;
}

::-webkit-scrollbar {
    display: none;
    /* Chrome Safari */
}

.card-show {
    margin: 5px 5px 5px 0;
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

.content {
    justify-content: center;


}

.card-show:nth-child(4n) {
    margin-right: 0px;
}

.content.el-row::after {
    content: '';
    display: block;
    flex: 1 1 auto;
}

.main {
    margin-top: 60px;
    flex: 1;
    position: relative;
    overflow-y: auto;
    background-color: #f0f0f0;
    --el-main-padding: 0;
    background-image: url("background/leaves.png");
}
.el-carousel__item h3 {
  color: #475669;
  opacity: 0.75;
  line-height: 200px;
  margin: 0;
  text-align: center;
}

.el-carousel__item:nth-child(2n) {
  background-color: #99a9bf;
}

.el-carousel__item:nth-child(2n + 1) {
  background-color: #d3dce6;
}
.headerinside {
    display: flex;
    justify-content: space-between;

}

.footer {
    height: 40px;
    background-color: lightgray;
    text-align: center;
    line-height: 40px;

    bottom: 0;
}
</style>
