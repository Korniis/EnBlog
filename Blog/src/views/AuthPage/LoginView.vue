<template>
    <div class="loginback">
        <div class="loginWindow" ref="loginanm">
            <div class="loginborder">
                <el-form hide-required-asterisk="false" :model="loginForm" :rules="formRules" ref="loginFormRef"
                    label-position="right" label-width="60px" status-icon>
                    <el-form-item label="用户名" prop="username">
                        <el-input v-model="loginForm.username" placeholder="请输入用户名" />
                    </el-form-item>

                    <el-form-item label="密码" prop="password">
                        <el-input v-model="loginForm.password" type="password" placeholder="请输入密码" />
                    </el-form-item>

                    <el-form-item label-width="22px" prop="agree">
                        <a style="margin-left: auto;" href="/EmailReset">找回密码</a>
                    </el-form-item>
                    <div class="btnrow">
                        <el-button size="large" class="subBtn" type="primary"
                            @click="login(loginFormRef)">登录</el-button>

                        <el-button size="large" class="subBtn" @click="router.push('/Register')">注册</el-button>
                    </div>
                </el-form>
            </div>
        </div>
    </div>
</template>
<script setup>

import router from '@/router';
import { ElMessage } from 'element-plus'
import { ref, reactive } from 'vue'
import useUser from '@/stores/useUser';

import { onMounted, onUnmounted } from "vue";
import * as THREE from "three";
import WAVES from "vanta/dist/vanta.waves.min";
const loginFormRef = ref(null);
const loginanm = ref();
let vantaEffect;
// 表单对象
const loginForm = reactive({
    username: '',
    password: '',
    agree: true
});

const userStore = useUser();

// 规则对象
/**
 * 自定义校验函数 - 参数介绍
 * @param { any } rule 当前对应的规则
 * @param { any } value 对应文本框的值
 * @param { any } callback 回调函数，不管是否通过校验，都必须执行
*/


// const validatorUsername = (rule, value, callback) => {
//     if (!/^1(3[0-9]|5[0-3,5-9]|7[1-3,5-8]|8[0-9])\d{8}$/.test(value))
//     return callback(new Error("请输入正确格式的手用户名！"));
//     callback();
// };


// const validatorPwd = (rule, value, callback) => {
//     // 检验密码强度
//     if (/\d/.test(value) && /[a-z]/.test(value) ) return callback();
//     callback(new Error("密码强度较弱，请输入带有 大写字母、小写字母、数字三种字符组合的密码！"));
// };


const formRules = {
    username: [
        // required - 是否必填
        // message - 校验不通过的提示信息
        // trigger - 校验的触发方式【blur - 失去焦点触发；change - v-model绑定的值改变触发】
        { required: true, message: '用户名不能为空', trigger: 'blur' },

        // validator - 自定义校验规则
        // { validator: validatorUsername, trigger: 'blur' }
    ],
    password: [
        { required: true, message: '密码不能为空', trigger: 'blur' },

        // min - 最小长度
        // max - 最大长度
        // { min: 6, max: 14, message: '密码长度为 6~14 个字符', trigger: 'blur' },
        // { validator: validatorPwd, triger: 'blur' }
    ],

};


// TODO 表单整体校验 + 登录
const login = async () => {
    loginFormRef.value.validate((valid) => {
        // 不通过校验
        if (!valid) return ElMessage.error('请填写 登录信息 或 同意协议 再进行登录操作！');
        // 通过校验
        // 登录逻辑
    });
    let islogin = await userStore.login(loginForm);
    if (islogin == false) {
        return;
    }
    await router.push('/');
    await location.reload();
    ElMessage.success("登录成功");

};

onMounted(() => {
    vantaEffect = WAVES({
        el: loginanm.value,
        THREE: THREE,
        mouseControls: true,
        touchControls: true,
        gyroControls: false,
        minHeight: 200.00,
        minWidth: 200.00,
        scale: 1.00,
        scaleMobile: 1.00
    });
});

onUnmounted(() => {
    if (vantaEffect) {
        vantaEffect.destroy();
    }
});

</script>
<style>
.loginback {
    width: 100%;
    display: flex;
    flex-direction: column;
    flex-wrap: wrap;
    align-content: stretch;
}

.loginWindow {
    height: auto;
    width: 300px;
    margin: 100px auto auto auto;
    background-color: white;
    border-radius: 10px;
    box-shadow: var(--el-box-shadow-lighter);
    padding: 50px;
    overflow: hidden;
}

.btnrow {
    display: flex;
    justify-content: space-around;
}
</style>
