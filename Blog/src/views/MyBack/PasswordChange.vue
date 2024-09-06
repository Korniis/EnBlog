<template>
    <div class="infocontent">
        <div class="contentheader">修改密码</div>
        <div class="infomain">
            <!-- Avatar Section -->


            <!-- User Info Form -->
            <el-form :model="form" label-width="120px" class="infoform" hide-required-asterisk="false" :rules="rules"
                ref="ruleFormRef">
                <el-form-item class="pwditem" label="密码" prop="oldpwd">
                    <el-input type="password" class="pwdbox" v-model="form.oldpwd" autocomplete="new-password" />
                </el-form-item>
                <el-form-item class="pwditem" label="重复密码" prop="oldpwdc">
                    <el-input type="password" class="pwdbox" v-model="form.oldpwdc" autocomplete="new-password" />
                </el-form-item>
                <el-form-item class="pwditem" label="新密码" prop="newpwdc">
                    <el-input type="password" class="pwdbox" v-model="form.newpwd" />
                </el-form-item>

                <el-button style="margin-left: 100px;" type="primary" plain @click="open">重置</el-button>


            </el-form>
        </div>
    </div>
</template>

<script setup>
import useUser from '@/stores/useUser';
import axios from 'axios';
import { ElMessage, ElMessageBox } from 'element-plus';
import { ref, onMounted, reactive } from 'vue';
const ruleFormRef = ref(null)
const userstore = useUser();

const form = reactive({

    oldpwd: '',
    oldpwdc: '',
    newpwd: '',
});
const validatePass = (rule, value, callback) => {
    if (value === '') {  // 此时是判空，如果 Password 中为空，就抛出该提示信息;
        callback(new Error('请输入密码'))
    } else {
        if (form.oldpwdc !== '') {  // 此时是：判断 checkPassWord 输入的值是否为空;
            if (!ruleFormRef.value) return  // 只要 checkPassWord 中有输入，此时的 !ruleFormRef.value 的布尔值为 false;
            ruleFormRef.value.validateField('oldpwdc', () => null) // validateField 是用于对表单中的字段进行验证的方法，来确保数据的正确性;
            // 个人理解是：在 checkPassWord 中有输入与 Password 的输入对比的操作，因此可以理解为：对 'checkPassWord'
            // 进行监听，即：有执行了一遍 checkPassWord 的验证(确定：通过 debugger 验证了猜想);
        }
        if (/\d/.test(value) && /[A-z]/.test(value)) return callback();
        callback(new Error("密码强度较弱，请输入带有 字母、数字 组合的密码！"));
    }
}
// checkPassWord 的规则是：让 checkPassWord 的输入值，不能为空，且与 Password 的输入值要保持一致;
const validatePass2 = (rule, value, callback) => {
    if (value === '') {  // 此时是判空，如果 checkPassWord 中为空，就抛出该提示信息;
        callback(new Error('请重复密码'))
    } else if (value !== form.oldpwd) { // 此时是：判断 checkPassWord 输入的值是否与 Password 输入的值相同，若不相同，就抛出该提示信息;
        callback(new Error("两次设置密码不一样"))
    } else { // 只有 checkPassWord 输入的值与 Password 输入的值相同，才没问题(前提是：Passwork 与 checkPassWord 中都有值);
        callback()
    }
}

const rules = ref({
    oldpwd: [

    ],
    oldpwdc: [
        { validator: validatePass2, trigger: 'blur' }
    ],
    newpwd: [
        { required: true, validator: validatePass, trigger: 'blur' }
    ],

})

const open = () => {
    ElMessageBox.confirm(
        '是否修改',
        'Warning',
        {
            confirmButtonText: 'OK',
            cancelButtonText: 'NO',
            type: 'warning',
        }
    )
        .then(() => {
            axios.interceptors.request.use(config => {
                //获取Token
                const token = localStorage.getItem("Elog_jwtToken")

                if (token) {
                    config.headers.Authorization = `Bearer ${token}`
                }
                return config
            })
            axios.put("/api/User/ResetPasswordByOld", {
                oldpwd: form.oldpwd,
                newpwd: form.newpwd,

            }).then(res => {

                if (res.data.code === 500) {
                    ElMessage.error(res.data.message);
                    return;
                }
                Object.assign(form, res.data);
                userstore.userInfoGet();
                ElMessage.success("修改成功");

            }).catch(error => {

                ElMessage.error(error.message)

            });
        })
        .catch(() => {
            ElMessage({
                type: 'info',
                message: '取消',
            })
        })
}


</script>

<style scoped>
.avatar {
    cursor: pointer;
}

.contentheader {
    background-color: #fafafa;
    color: var(--el-color-primary-light-5);
    height: 60px;
    width: 100%;
    font-size: 24px;
    display: flex;
    align-items: center;
    box-shadow: var(--el-box-shadow-lighter);
    margin: auto;
    padding-left: 20px;
}

.infocontent {
    padding: 20px;
    backdrop-filter: blur(6px);
    height: 100%;

}

.pwdbox {
    width: 200px;

}

.pwditem {
    margin: 10px;
}

.infomain {
    display: flex;
    flex-direction: column;
    align-items: center;

    margin: 100px 0 0 0;
}



.infoform {
    display: flex;
    flex-direction: column;
    margin: auto;
    align-items: center;
    width: 100%;
    max-width: 600px;
    background-color: #fff;
    padding: 60px;
    border-radius: 8px;
    box-shadow: var(--el-box-shadow-lighter);
}
</style>
