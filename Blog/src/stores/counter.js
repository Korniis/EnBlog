import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import axios from 'axios'
import { ElMessage } from 'element-plus'
import { jwtDecode } from 'jwt-decode'
export const useUser = defineStore('user', {

  state:()=>( {
    user: {
      userid: "",
      username: ""
    },

  }),
  actions: {
   async login(loginData) {
    await  axios.post('/api/Authorize/Login', {
        userName: loginData.username,
        userPwd: loginData.password
      }).then(res => {

        if(res.data.code==200){
          //ok
          const jwtPlayLoad = res.data.data;
          localStorage.setItem("Elog_jwtToken", jwtPlayLoad)
          const decodeJwt = jwtDecode(jwtPlayLoad)
          console.log(decodeJwt);
          this.$state.userid=decodeJwt["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
          this.$state.username=decodeJwt["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];

        }
        else  if(res.data.code==500)
        {
          ElMessage.error(res.data.message);

        }
      })

    }
  }

})
export default useUser
