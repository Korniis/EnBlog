<template>
    <el-card class="webdatashow">
        <div class="card-content">
            <div class="data-block">
                <span class="data-title">今日人数</span>
                <span class="data-value">{{ userstore.viewdata.viewCount }}</span>
            </div>
            <div class="data-block">
                <span class="data-title">文章数量</span>
                <span class="data-value">{{ userstore.viewdata.articleCount }}</span>
            </div>
            <div class="data-block">
                <span class="data-title">种类数量</span>
                <span class="data-value">{{ userstore.viewdata.typeCount }}</span>
            </div>
            <div class="data-block">
                <span class="data-title">用户数量</span>
                <span class="data-value">{{ userstore.viewdata.userCount }}</span>
            </div>
        </div>
    </el-card>

</template>



<script setup>
import useUser from '@/stores/useUser';
import { onMounted } from 'vue';

const userstore = useUser();

onMounted(() => {



})
</script>
<style>
.webdatashow {
    max-width: 480px;
    border-radius: 20px;
    display: block;
    padding: 20px;
    background-image: url("/public/background/CardBg.png");
    background-size: cover;
    /* 确保背景图片覆盖整个卡片 */
    background-position: center;
    /* 居中对齐背景图片 */
    background-repeat: no-repeat;
    /* 防止背景图片重复 */
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
    margin-bottom: 20px;
    padding: 20px;
}

.card-content {
    display: flex;
    flex-direction: column;
    gap: 20px;
}

.data-block:hover {
    transform: translateY(-5px);
}



.data-title {
    font-size: 18px;
    font-weight: bold;
    color: #333;
}

.data-value {
    font-size: 24px;
    font-weight: bold;
    color: #409eff;
    /* Element Plus 默认主题颜色 */
}

.data-block {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 10px;
    border-radius: 10px;
    background-color: #ffffff;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
    transition: transform 0.2s ease;
}
</style>