<template>
    <div class="avatarcontent">
        <div class="contentheader"> 修改头像</div>
        <div class="avatarmain">
            <div class="avatar">

                <el-avatar shape="square" :size="100" :fit="fit" :src="url" />
            </div>
        </div>
    </div>
</template>

<script setup>
import { defineProps, reactive, ref } from 'vue'

const url = ref("");

const onSubmit = () => {
    console.log('submit!')
}
</script>

<style scoped>
.contentheader {
    background-color: #fafafa;
    color: var(--el-color-primary-light-5);
    height: 60px;
    width: 100%;
    font-size: 24px;
    display: flex;
    align-items: center;
    /* 垂直居中 */
    box-shadow: var(--el-box-shadow-lighter);
    margin: 10px;
}

.avatarcontent {

    height: 100%;
    width: 100%;
    font-size: 30px;
    backdrop-filter: blur(6px);
}

.avatarmain {
    display: flex;
    align-self: center;
    justify-self: center;

}
</style>