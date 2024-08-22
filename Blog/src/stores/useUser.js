import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import axios from 'axios'
import { ElMessage } from 'element-plus'
import { jwtDecode } from 'jwt-decode'
export const useUser = defineStore('user', {

  state: () => ({
    user: {
      userid: "",
      username: ""
    },
    viewdata: {

      viewCount: '',
      articleCount: '',
      typeCount: '',
      userCount: '',
    },

  }),
  actions: {

    async login(loginData) {
      try {
        const res = await axios.post('/api/Authorize/Login', {
          userName: loginData.username,
          userPwd: loginData.password
        });

        if (res.data.code === 200) {
          // 登录成功
          const jwtPlayLoad = res.data.data;
          localStorage.setItem("Elog_jwtToken", jwtPlayLoad);

          const decodeJwt = jwtDecode(jwtPlayLoad);
          this.$state.userid = decodeJwt["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
          this.$state.username = decodeJwt["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];
          return true;

        } else if (res.data.code === 500) {
          ElMessage.error(res.data.message);
          return false;
        }

      } catch (error) {
        // 捕获异常
        console.error("Login error:", error);
        ElMessage.error("登录失败，请重试。");
        return false;
      }
    },
    async sendEmailCheck(registerData) {
      console.log(registerData)
      await axios.post("/api/Authorize/SendRegisterCode", {
        userName: registerData.name,
        userPwd: registerData.passWord,
        emailAddress: registerData.email
      }).then(res => {
        if (res.data.code == 200) {
          ElMessage.success('验证码已发送');
        }
        else {
          ElMessage.error(res.data.message);
        }
      })
    },
    async checkEmailCheck(emailData) {
      try {
        console.log(emailData)
        axios.post('/api/User/Register', {
          userName: emailData.name,
          userPwd: emailData.passWord,
          emailAddress: emailData.email,
          code: emailData.vildcode


        }).then(res => {
          if (res.data.code == 200) {
            ElMessage.success('注册成功请返回登录');
            return true

          }
          else {
            ElMessage.error(res.data.message);
          }
        })
      } catch {

      }
    },
    getWebData() {
      axios.get("api/MainView/UpWebData").then(res => {

        this.viewdata = res.data.data;


        console.log(this.viewdata);
      }).catch(error => {
        ElMessage.error(error.message);
      })

    }
  }

})
export default useUser
