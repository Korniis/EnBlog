// 此时是去除 ts 的 Form 表单校验规则的 vue3 语法的代码：
<template>
    <div class="loginback">
        <div class="loginWindow">
            <el-form hide-required-asterisk="false" :model="loginForm" :rules="formRules" ref="loginFormRef"
                label-position="right" label-width="60px" status-icon>
                <el-form-item label="账户" prop="username">
                    <el-input v-model="loginForm.username" placeholder="请输入手机号" />
                </el-form-item>

                <el-form-item label="密码" prop="password">
                    <el-input v-model="loginForm.password" placeholder="请输入密码" />
                </el-form-item>

                <el-form-item label-width="22px" prop="agree">
                    <el-checkbox size="large" v-model="loginForm.agree">
                        我已同意隐私条款和服务条款
                    </el-checkbox>
                </el-form-item>

                <el-button size="large" class="subBtn" @click="login(loginFormRef)">点击登录</el-button>
            </el-form>
        </div>
    </div>
</template>
<script setup>

import { ElMessage } from 'element-plus'
import { ref, reactive } from 'vue'

const loginFormRef = ref(null);

// 表单对象
const loginForm = reactive({
    username: '',
    password: '',
    agree: true
});



// 规则对象
/**
 * 自定义校验函数 - 参数介绍
 * @param { any } rule 当前对应的规则
 * @param { any } value 对应文本框的值
 * @param { any } callback 回调函数，不管是否通过校验，都必须执行
*/


const validatorUsername = (rule, value, callback) => {
    if (!/^1(3[0-9]|5[0-3,5-9]|7[1-3,5-8]|8[0-9])\d{8}$/.test(value)) return callback(new Error("请输入正确格式的手机号！"));
    callback();
};


const validatorPwd = (rule, value, callback) => {
    // 检验密码强度
    if (/\d/.test(value) && /[a-z]/.test(value) && /[A-Z]/.test(value)) return callback();
    callback(new Error("密码强度较弱，请输入带有 大写字母、小写字母、数字三种字符组合的密码！"));
};


const formRules = {
    username: [
        // required - 是否必填
        // message - 校验不通过的提示信息
        // trigger - 校验的触发方式【blur - 失去焦点触发；change - v-model绑定的值改变触发】
        { required: true, message: '用户名不能为空', trigger: 'blur' },

        // validator - 自定义校验规则
        { validator: validatorUsername, trigger: 'blur' }
    ],
    password: [
        { required: true, message: '密码不能为空', trigger: 'blur' },

        // min - 最小长度
        // max - 最大长度
        { min: 6, max: 14, message: '密码长度为 6~14 个字符', trigger: 'blur' },
        { validator: validatorPwd, triger: 'blur' }
    ],
    agree: [
        // 自定义校验规则
        {
            validator: (rule, value, callback) => {
                if (!value) return callback(new Error('请勾选同意协议！'));
                callback();
            },
            trigger: 'change'
        }
    ]
};


// TODO 表单整体校验 + 登录
const login = () => {
    loginFormRef.value.validate((valid) => {
        // 不通过校验
        if (!valid) return ElMessage.error('请填写 登录信息 或 同意协议 再进行登录操作！');
        // 通过校验
        // 登录逻辑
    });
};
</script>
<style>
.loginback {
    width: 100%;
    height: 100vh;
    display: flex;
}

.loginWindow {
    height: auto;
    margin: auto auto;
    background-color: white;
    border-radius: 10px;
    box-shadow: var(--el-box-shadow-lighter);
    padding: 50px;
}
</style>
