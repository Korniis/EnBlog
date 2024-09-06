<template>
  <div class="infocontent">
    <div class="contentheader">我的信息</div>
    <div class="infomain">
      <!-- Avatar Section -->
      <div class="avatar-section">

        <el-upload class="avatar-uploader" :headers="uheaders" action :http-request="uploadImage" method="put"
          :show-file-list="false" :on-success="handleAvatarSuccess" :before-upload="beforeAvatarUpload">

          <div class="avatar-wrapper">
            <img v-if="form.avatar" :src="`${axios.defaults.baseURL}/images/${form.avatar}`" alt="User Avatar"
              class="avatar" />
            <el-icon v-else class="avatar-uploader-icon">
              <Plus />
            </el-icon>
            <div v-if="form.avatar" class="avatar-overlay">

              <span class="overlay-text">更换头像</span>
            </div>
          </div>
        </el-upload>
      </div>


      <!-- User Info Form -->
      <el-form :model="form" label-width="120px" class="infoform">
        <el-form-item label="用户名">
          <el-input v-model="form.userName" />
        </el-form-item>

        <el-form-item label="微信">
          <el-input v-model="form.wxAccount" />
        </el-form-item>
        <el-form-item label="电话">
          <el-input v-model="form.phoneNumber" />
        </el-form-item>
        <el-form-item label="个性签名">
          <el-input v-model="form.myDescriptions" type="textarea" />
        </el-form-item>
        <el-form-item>
          <el-button plain type="primary" @click="onSubmit" style="margin: auto;">修改</el-button>
        </el-form-item>
      </el-form>
    </div>
  </div>
</template>

<script setup>
import useUser from '@/stores/useUser';
import axios from 'axios';
import { ElMessage, ElMessageBox } from 'element-plus';
import { onMounted, reactive, ref } from 'vue';

const userstore = useUser();
const form = reactive({
  id: 23,
  avatar: 'https://via.placeholder.com/150', // Default avatar placeholder
  myDescriptions: "",
  userName: "",
  wxAccount: "",
  phoneNumber: "",
  email: ""
});
const token = localStorage.getItem("Elog_jwtToken");


const uheaders = reactive({
  "Content-Type": "multipart/form-data",
  "Authorization": `Bearer ${token}`,

});
const handleProgress = (event) => {
  this.uploadProgress = Math.round(event.percent);
};
const uploadImage = async (param) => {
  var file = param.file;
  const formData = new FormData();
  formData.append("formData", file);
  try {
    const response = await axios.put('/api/User/EditAvatar', formData, {
      headers: {
        "Content-Type": "multipart/form-data",
        "Authorization": `Bearer ${token}`
      }
    });
    console.log(response);
    if (response.data.code == 200) {
      handleAvatarSuccess(response);
    } else {
      ElMessage.error('上传失败!');
    }
  } catch (error) {
    ElMessage.error(`上传失败: ${error.message}`);
  }

};



const fileInput = ref(null);

const handleAvatarSuccess = (response) => {
  if (response.data.code == 200) {
    form.avatar = response.fileName;
    ElMessage.success('头像上传成功!');
  } else {
    ElMessage.error('头像上传失败!');
  }
};

const beforeAvatarUpload = (file) => {
  const isImage = ['image/jpeg', 'image/png'].includes(file.type);
  const isLt2M = file.size / 1024 / 1024 < 2;

  if (!isImage) {
    ElMessage.error('头像图片必须是 JPG 或 PNG 格式!');
  }
  if (!isLt2M) {
    ElMessage.error('头像图片大小不能超过 2MB!');
  }
  return isImage && isLt2M;
};

// const triggerFileInput = () => {
//   fileInput.value.click();
// };

// const handleFileChange = async (event) => {
//   const file = event.target.files[0];
//   if (!file) return;

//   const formData = new FormData();
//   formData.append('formFile', file);

//   try {
//     const token = localStorage.getItem("Elog_jwtToken")

//     const result = await axios.put('/api/User/EditAvatar', formData, {
//       headers: {
//          "Authorization": `Bearer ${token}`,
//         'Content-Type': 'multipart/form-data',
//       },
//     });

//     handleAvatarSuccess(result.data);
//   } catch (error) {
//     ElMessage.error('上传失败!');
//     console.error(error);
//   }
// };
const onUploadAvatar = () => {
  // Logic to upload and change the avatar
};

const onSubmit = () => {
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
      axios.put("/api/User/UpdateUserByFont", form).then(res => {

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
        message: '取消删除',
      })
    })
};

onMounted(() => {
  form.email = userstore.user.email;
  form.id = userstore.user.id;
  form.userName = userstore.user.userName;
  form.wxAccount = userstore.user.wxAccount;
  form.myDescriptions = userstore.user.myDescriptions;
  form.phoneNumber = userstore.user.phoneNumber;
  form.avatar = userstore.user.avatar // Use user's avatar if available
});
</script>

<style scoped>
.avatar {
  cursor: pointer;
}

.avatar-wrapper {
  position: relative;
  display: inline-block;
}

.avatar {
  width: 100px;
  /* 设置图片大小 */
  height: 100px;
  /* 设置图片大小 */
  border-radius: 50%;
  display: block;
}

.avatar-overlay {
  position: absolute;
  bottom: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  /* 半透明背景 */
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: #fff;
  font-size: 16px;
  opacity: 0;
  transition: opacity 0.3s;
}

.avatar-wrapper:hover .avatar-overlay {
  opacity: 1;
  /* 鼠标悬停时显示覆盖层 */
}

.avatar-overlay-icon {
  font-size: 24px;
  /* 设置图标大小 */
  margin-right: 8px;
}

.overlay-text {
  font-size: 14px;
  /* 设置文字大小 */
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
  height: 100vh;
}

.infomain {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.avatar-section {
  display: flex;
  flex-direction: column;
  align-items: center;
  margin-bottom: 20px;
}

.avatar {
  width: 100px;
  height: 100px;
  border-radius: 50%;

  object-fit: cover;
}

.infoform {
  width: 100%;
  max-width: 600px;
  background-color: #fff;
  padding: 20px;
  border-radius: 8px;
  box-shadow: var(--el-box-shadow-lighter);
}
</style>
