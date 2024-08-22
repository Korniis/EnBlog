// 此时是去除 ts 的 Form 表单校验规则的 vue3 语法的代码：

<template>
  <div class="Register_content">
    <div class="Register_back">
      <el-form ref="ruleFormRef" :model="ruleForm" status-icon :rules="rules" label-width="100px" class="demo-ruleForm">
        <el-form-item label="用户名:" prop="name">
          <el-input v-model="ruleForm.name" placeholder="请输入" />
        </el-form-item>
        <!-- 此时的 status-icon 属性是决定在框里右侧的图标的(符合规则的图标或不符合规则的图标) -->
        <el-form-item label="密码:" prop="passWord">
          <el-input v-model="ruleForm.passWord" type="password" placeholder="请输入" autocomplete="off" />
        </el-form-item>
        <el-form-item label="重复密码:" prop="checkPassWord">
          <el-input v-model="ruleForm.checkPassWord" type="password" placeholder="请输入" autocomplete="off" />
        </el-form-item>



        <!-- <el-form-item label="邮箱地址:" prop="email">
          <el-input v-model="ruleForm.email" placeholder="请输入" />


        </el-form-item> -->
        <el-form-item label="邮箱地址:" prop="email" style="">
          <el-input v-model="ruleForm.email" placeholder="请输入" style="width: 290px;" />
          <el-button style="margin-left: auto;" type="primary" :disabled="isCoolingDown" @click="validateAndSendCode">
            {{ isCoolingDown ? `冷却中(${cooldownTime}s)` : '发送验证码' }}
          </el-button>

        </el-form-item>
        <el-form-item label="验证码:" prop="vildcode" style="">
          <el-input v-model="ruleForm.vildcode" placeholder="请输入" />

        </el-form-item>
        <el-form-item label-width="22px" prop="agree">
          <el-checkbox size="large" v-model="ruleForm.agree">
            我已同意隐私条款和服务条款
          </el-checkbox>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="submitForm(ruleFormRef)">

            注册 </el-button>
          <el-button @click="resetForm(ruleFormRef)">Reset</el-button>
        </el-form-item>
      </el-form>
    </div>
  </div>
</template>

<script setup>

import { ElMessage } from 'element-plus';
import { ref, reactive } from 'vue'
import useUser from '@/stores/useUser';
import router from '@/router';
const ruleFormRef = ref(null)   // 注意：一定要定义 form 表单中 ref 的 ruleFormRef 的值，否则会一直报错;

const ruleForm = reactive({
  passWord: "",
  checkPassWord: "",
  name: '',
  email: '',
  agree: '',
  vildcode: ''
})
const isCoolingDown = ref(false);
const cooldownTime = ref(60);
let cooldownInterval = null;
// 重置的操作;
const resetForm = (val) => {
  if (!val) return
  val.resetFields()  // resetFields 函数：是将表单字段的值重置为其默认值;
}
const useStore = useUser();

// Password 的规则是：让 Password 的输入值，不能为空，且若 checkPassWord 的输入值不为空时，在 Password 的输入值后在执行一遍 checkPassWord 的规则;
const validatePass = (rule, value, callback) => {
  if (value === '') {  // 此时是判空，如果 Password 中为空，就抛出该提示信息;
    callback(new Error('请输入密码'))
  } else {
    if (ruleForm.checkPassWord !== '') {  // 此时是：判断 checkPassWord 输入的值是否为空;
      if (!ruleFormRef.value) return  // 只要 checkPassWord 中有输入，此时的 !ruleFormRef.value 的布尔值为 false;
      ruleFormRef.value.validateField('checkPassWord', () => null) // validateField 是用于对表单中的字段进行验证的方法，来确保数据的正确性;
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
  } else if (value !== ruleForm.passWord) { // 此时是：判断 checkPassWord 输入的值是否与 Password 输入的值相同，若不相同，就抛出该提示信息;
    callback(new Error("两次设置密码不一样"))
  } else { // 只有 checkPassWord 输入的值与 Password 输入的值相同，才没问题(前提是：Passwork 与 checkPassWord 中都有值);
    callback()
  }
}


// age 的规则是：让 age 的值，不能为空，为数字，且数字值不小于 18;
// const checkAge = (rule, value, callback) => {
//   if (!value) {  // 此时是判空，如果 age 中为空，就抛出该提示信息;
//     return callback(new Error('Please input the age'))
//   }
//   setTimeout(() => {
//     if (!Number.isInteger(value)) {   // 此时是：判断 age 中输入的值是否是数字，如果不是数字就抛出该提示信息;
//       callback(new Error('Please input digits'))
//     } else {
//       if (value < 18) { // 此时是：判断 age 中输入的数字是否小于 18，如果小于 18，就抛出该提示信息;
//         callback(new Error('Age must be greater than 18'))
//       } else {
//         callback()
//       }
//     }
//   }, 1000)
// }

// phone 的规则是：让 phone 的值，不能为空，为数字，且符合手机号的校验规则;
// const validatePhone = (rule, value, callback) => {
//   if (!value) {  // 此时是判空，如果 phone 中为空，就抛出该提示信息;
//     return callback(new Error('Please input the phone number'))
//   }
//   setTimeout(() => {
//     if (!Number.isInteger(Number(value))) { // 此时是：判断 phone 中输入的值是否是数字，如果不是数字就抛出该提示信息;
//       // 此时：虽然 value 的值是一个字符串，但是当输入的是数字时，通过 Number(value) 操作此时已经变成了数字, 然后再进行 Number.isInteger() 的判断;
//       callback(new Error('Please input digits'))
//     } else {
//       if (/^1[3|4|5|6|7|8|9][0-9]\d{8}$/.test(value)) { // 此时是：判断 phone 中输入的数字是否符合手机号的校验规则，如果不符合就抛出提示信息;
//         callback()
//       } else {
//         callback(new Error('Please input the valid phone number'))
//       }
//     }
//   })
// }

// 可能存在的判断是否有重复手机号的校验(即：根据后台的返回值来判断)：
// const checkTelCode = async (rule, value, callback) => {
//   if (value) {
//     await validPhoneUnique({ phonenumber: value }).then((response) => {
//       if (response.code == 200) {
//         callback();
//       } else {
//         callback(new Error("手机号已存在"));
//       }
//     });
//   }
// }

// 注意：在 rules 方法中 validator 属性后面的方法，一定要定义在 rules 函数前，否则会报错;
const rules = ref({
  // form 表单前面有没有小红星，取决于有没有 'required: true' 的属性，如果有这个属性页面就有小红星，而没有这个属性就没有小红星;
  passWord: [
    { required: true, validator: validatePass, trigger: 'blur' }
  ],
  checkPassWord: [
    { validator: validatePass2, trigger: 'blur' }
  ],
  // age: [
  //   { validator: checkAge, trigger: 'blur' }
  // ],
  name: [
    { required: true, message: '用户名 不能为空', trigger: 'blur' },       // 此时是防空判断;
    { min: 4, max: 20, message: '长度在 4 到 20 个字符', trigger: 'blur' }  // 此时是：判断输入值是否是 7~11 位(即：name 的校验规则);
  ],
  // ip: [
  //   { required: true, message: 'IP不能为空', trigger: 'blur' },
  //   { pattern: /^((25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))$/, message: "请填写合法的IP地址", trigger: "blur" }
  //   // 此时的 pattern 是合法 ip 地址的正则表达式，当失去焦点后，就会触发正则表达式的判断, 若不满足正则表达式的条件就会提示 message 中的信息;
  // ],
  email: [
    { required: true, message: '邮箱 不能为空', trigger: 'blur' },    // 此时是防空判断;
    { type: "email", message: "请输入正确的邮箱", trigger: "blur" }   // 此时是：判断是否是正确邮箱格式(即：邮箱的校验规则);
  ],
  // phone: [
  //   { required: true, validator: validatePhone, trigger: 'blur' },    // 此时是防空判断;
  //   // { pattern: /^1[3|4|5|6|7|8|9][0-9]\d{8}$/, message: "请输入正确的电话", trigger: "blur" }  // 此时是：手机号的校验规则；
  // ],
  agree: [
    // 自定义校验规则
    {
      validator: (rule, value, callback) => {
        if (!value) return callback(new Error('请勾选同意协议！'));
        callback();
      },
      trigger: 'change'
    }
  ],
  vildcode:
    [

      { required: true, message: '验证码不能为空', trigger: 'blur' },

    ]
})

const validateAndSendCode = async () => {
  await ruleFormRef.value.validateField('email');
  await ruleFormRef.value.validateField('name');
  await ruleFormRef.value.validateField('passWord');
  startCooldown();

  await useStore.sendEmailCheck(ruleForm);
};
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
// 此时是：提交表单的操作;
const submitForm = async () => {
  if (!ruleFormRef.value) return;

  try {
    const valid = await ruleFormRef.value.validate();  // 基于 Promise 的验证

    if (valid) {
      await useStore.checkEmailCheck(ruleForm);  // 异步操作
      await router.push('/Login');
      location.reload();

      // 路由跳转

    } else {
      ElMessage.error('Error submit!');
    }
  } catch (error) {
    ElMessage.error('验证有误!');
  }
};


</script>

<style>
.Register_content {

  height: 100vh;

}

.Register_back {
  width: 500px;
  background-color: white;
  box-shadow: var(--el-box-shadow-lighter);
  padding: 50px;
  margin: 100px auto auto auto;
  border-radius: 10px;
  display: flex;
  justify-content: center;

}

.demo-ruleForm {
  width: 800px;
}
</style>
