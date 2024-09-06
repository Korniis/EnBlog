<template>
    <div class="loginback">
        <div class="loginWindow">
            <el-form style=" width: 500px;" hide-required-asterisk="false" :model="loginForm" :rules="formRules"
                ref="loginFormRef" label-position="right" label-width="60px" status-icon>
                <el-form-item label="用户名" prop="username">
                    <el-input v-model="loginForm.username" placeholder="请输入用户名" />
                </el-form-item>
                <el-form-item label="邮箱" prop="email">
                    <el-input v-model="loginForm.email" style="width: 290px;" placeholder="请输入密码" />
                    <el-button style="margin-left: auto;" type="primary" :disabled="isCoolingDown"
                        @click="sendEmailCheck">
                        {{ isCoolingDown ? `冷却中(${cooldownTime}s)` : '发送验证码' }}
                    </el-button>
                </el-form-item>
                <el-form-item label="新密码" prop="newpassword">
                    <el-input v-model="loginForm.newpassword" type="password" placeholder="请输入密码" />
                </el-form-item>
                <el-form-item label="验证码" prop="token">
                    <el-input v-model="loginForm.token" placeholder="请输入密码" />
                </el-form-item>

                <el-form-item label-width="22px" prop="agree">

                </el-form-item>
                <div class="btnrow">
                    <el-button size="large" class="subBtn" type="primary" @click="changePassword">登录</el-button>

                    <el-button size="large" class="subBtn" @click="router.push('/Register')">注册</el-button>
                </div>
            </el-form>
        </div>
    </div>
</template>
<script setup>

import router from '@/router';
import { ElMessage } from 'element-plus'
import { ref, reactive } from 'vue'
import useUser from '@/stores/useUser';
import axios from 'axios';
const loginFormRef = ref(null);

// 表单对象
const loginForm = reactive({
    username: '',
    newpassword: '',
    email: '',
    token: '',
    agree: true
});
const isCoolingDown = ref(false);
const cooldownTime = ref(60);
let cooldownInterval = null;
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
const validatePass = (rule, value, callback) => {

    if (value === '') {  // 此时是判空，如果 Password 中为空，就抛出该提示信息;
        callback(new Error('请输入密码'))
    } else {

        if (/\d/.test(value) && /[A-z]/.test(value)) return callback();
        callback(new Error("密码强度较弱，请输入带有 字母、数字 组合的密码！"));
    }

};


const formRules = {
    email: [
        { required: true, message: '邮箱不能为空', trigger: 'blur' },    // 此时是防空判断;
        { type: "email", message: "请输入正确的邮箱", trigger: "blur" }   // 此时是：判断是否是正确邮箱格式(即：邮箱的校验规则);
    ],

    username: [
        // required - 是否必填
        // message - 校验不通过的提示信息
        // trigger - 校验的触发方式【blur - 失去焦点触发；change - v-model绑定的值改变触发】
        { required: true, message: '用户名不能为空', trigger: 'blur' },

        // validator - 自定义校验规则
        // { validator: validatorUsername, trigger: 'blur' }
    ],
    newpassword: [
        { required: true, message: '密码不能为空', trigger: 'blur' },

        // min - 最小长度
        // max - 最大长度
        { min: 6, max: 14, message: '密码长度为 6~14 个字符带字母', trigger: 'blur' },
        // { validator: validatorPwd, triger: 'blur' }
        { validator: validatePass, trigger: 'blur' }
    ],
    token: [
        { required: true, message: "验证码不为空", trigger: 'blur' }

    ]
};

const sendEmailCheck = async () => {
    try {
        // 校验 email 和 username 字段
        await loginFormRef.value.validateField('username');
        await loginFormRef.value.validateField('email');


        // 发送请求
        const response = await axios.get("/api/Authorize/SendResetToken", {
            params: {
                username: loginForm.username,
                email: loginForm.email
            }
        });

        // 请求成功处理
        console.log(response.data);
        ElMessage.success('验证码已发送');
        startCooldown();  // 启动冷却倒计时

    } catch (error) {
        if (error instanceof ErrorEvent) {
            // 捕获验证失败的错误
            console.error("Validation error:", error.message);
        } else {
            // 捕获请求失败的错误
            console.error("Error sending email check:", error);
            ElMessage.error("发送验证码失败，请检查邮箱和用户名。");
        }
    }
};
const changePassword = async () => {

    const valid = await loginFormRef.value.validate();
    if (valid) {
          axios.put("/api/User/ResetPassword", {

            Username: loginForm.username,
            NewPad: loginForm.newpassword,
            Token: loginForm.token

        }).then(res => {

            if (res.data.code != 200) {
                ElMessage.error(res.data.message);
                return;
            }
            ElMessage.success(res.data.data);
             router.push('/Login');


        }).catch(err => {

            ElMessage.error(err.message);
        });

    }
};
// TODO 表单整体校验 + 登录
const startCooldown = () => {
    isCoolingDown.value = true;
    cooldownTime.value = 60;
    cooldownInterval = setInterval(() => {
        cooldownTime.value -= 1;
        if (cooldownTime.value <= 0) {
            clearInterval(cooldownInterval);
            isCoolingDown.value = false;
        }
    }, 1000);
};
</script>
<style>
.loginback {
    width: 100%;
    height: 100vh;
    display: flex;
    align-items: flex-start;
}

.loginWindow {
    width: 800px;
    height: auto;
    margin: 100px auto auto auto;
    background-color: white;
    border-radius: 10px;
    box-shadow: var(--el-box-shadow-lighter);
    padding: 50px;
    display: flex;
    justify-content: center;
}

.btnrow {
    display: flex;
    justify-content: space-around;
}
</style>
