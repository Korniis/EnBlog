<template>
  <el-container class="layout-container-demo" style="height: 100vh">
    <el-aside width="200px">
      <el-scrollbar>
        <el-menu :router="true" :default-openeds="['1']">
          <el-sub-menu index="1">
            <template #title>
              <el-icon>
                <message />
              </el-icon>个人信息
            </template>
            <el-menu-item index="/Account/InfoPage">我的信息</el-menu-item>
        
            <el-menu-item index="/Account/PasswordChange">修改密码</el-menu-item>
            <el-menu-item index="/Account/EmailChange">修改邮箱</el-menu-item>
          </el-sub-menu>
          <el-sub-menu index="2">
            <template #title>
              <el-icon><icon-menu /></el-icon>文章管理
            </template>
            <!-- 子菜单项配置相同的 index -->
            <el-menu-item index="/Account/ArticleManage">文章管理</el-menu-item>

          </el-sub-menu>
          <el-menu-item index="/Account/MySetting">设置</el-menu-item>

        </el-menu>

      </el-scrollbar>
    </el-aside>

    <el-container>
      <!-- <el-header style="text-align: right; font-size: 12px">
        <div class="toolbar">
          <el-dropdown>
            <el-icon style="margin-right: 8px; margin-top: 1px">
              <setting />
            </el-icon>
            <template #dropdown>
              <el-dropdown-menu>
                <el-dropdown-item>View</el-dropdown-item>
                <el-dropdown-item>Add</el-dropdown-item>
                <el-dropdown-item>Delete</el-dropdown-item>
              </el-dropdown-menu>
            </template>
          </el-dropdown>
          <span>Tom</span>
        </div>
      </el-header> -->

      <el-main>
        <router-view>

        </router-view>
      </el-main>
    </el-container>
  </el-container>
</template>

<script setup>
import { onMounted, ref } from 'vue'
import { Menu as IconMenu, Message, Setting } from '@element-plus/icons-vue'
import useUser from '@/stores/useUser';

const useStore = useUser();



onMounted(()=>{

useStore.userInfoGet();

})
</script>

<style scoped>
.contentheader
{
  background-color: white;
}
.layout-container-demo .el-header {
  position: relative;
  background-color: var(--el-color-primary-light-9);
  color: var(--el-text-color-primary);
}

.layout-container-demo .el-aside {
  color:#FAFAFA;
  background:#FAFAFA;
}

.layout-container-demo .el-menu {
  border-right: none;
}

.layout-container-demo .el-main {
  padding: 0;
}

.layout-container-demo .toolbar {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  height: 100%;
  right: 20px;
}
</style>