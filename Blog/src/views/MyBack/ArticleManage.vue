<template>
   <div class="infocontent">
      <div class="contentheader">文章管理</div>
      <div class="infomain">
         <!-- Avatar Section -->

         <el-table v-loading="loading" :default-sort="{ prop: 'createTime', order: 'descending' }"
            :data="pageData.tableData" style="width: 100% ; ">
            <el-table-column label="文章标题" width="300">
               <template #default="scope">
                  <div style="display: flex; align-items: center">
                     <span style="
          margin-left: 0px;
          white-space: nowrap;
          overflow: hidden;
          text-overflow: ellipsis;
          max-width: 100%;
          cursor: pointer;
        " :title="scope.row.title">
                        {{ scope.row.title }}
                     </span>
                  </div>
               </template>
            </el-table-column>
            <el-table-column label="作者" width="100">
               <template #default="scope">
                  <div style="display: flex; align-items: center">

                     <span style="margin-left: 0px">{{ scope.row.userName }}</span>
                  </div>
               </template>
            </el-table-column>
            <el-table-column label="分类" width="100">
               <template #default="scope">
                  <div style="display: flex; align-items: center">

                     <span style="">{{ scope.row.typeName }}</span>
                  </div>
               </template>
            </el-table-column>
            <el-table-column label="封面" width="180">

               <template #default="scope">
                  <el-popover effect="light" trigger="hover" placement="top" width="auto">
                     <template #default>
                        <div style="display: flex; flex-direction: column;align-items: center">

                           <img v-if="scope.row.imgSrc" :src="`${axios.defaults.baseURL}/images/${scope.row.imgSrc}`" />
                        </div>
                     </template>
                     <template #reference>
                        <el-tag>{{ scope.row.imgSrc }}</el-tag>
                     </template>
                  </el-popover>
               </template>

            </el-table-column>

            <el-table-column label="文章数据" width="100">
               <template #default="scope">
                  <el-popover effect="light" trigger="hover" placement="top" width="auto">
                     <template #default>
                        <div>浏览人数: {{ scope.row.viewCount }}</div>
                        <div>喜欢人数: {{ scope.row.likeCount }}</div>
                     </template>
                     <template #reference>
                        <el-tag>{{ scope.row.viewCount }}</el-tag>
                     </template>
                  </el-popover>
               </template>
            </el-table-column>
            <el-table-column label="创建时间" prop="createTime" sortable width="200">
               <template #default="scope">
                  <div style="display: flex; align-items: center">
                     <el-icon>
                        <timer />
                     </el-icon>
                     <span style="margin-left: 10px">{{ scope.row.createTime }}</span>
                  </div>
               </template>
            </el-table-column>
            <el-table-column label="操作">
               <template #default="scope">
                  <el-button size="default" @click="handleEdit(scope.$index, scope.row)">
                     修改
                  </el-button>
                  <el-button size="default" type="danger" @click="handleDelete(scope.$index, scope.row)">
                     删除
                  </el-button>
               </template>
            </el-table-column>
         </el-table>
         <div class="pagination-container">
            <el-pagination :current-page="pageData.pageNum" :page-size="pageData.pageSize" background :pager-count="11"
               class="custom-pagination" layout="prev, pager, next" :total="pageData.total"
               @current-change="handlePageChange" />
         </div>

      </div>
   </div>
</template>

<script setup>
import useUser from '@/stores/useUser';
import axios from 'axios';
import { Timer } from '@element-plus/icons-vue'

import { ElMessage, ElMessageBox } from 'element-plus';
import { ref, onMounted, reactive } from 'vue';
import router from '@/router';


const pageData = reactive({
   total: 204,
   pageNum: 1,
   pageSize: 20,
   tableData: [],
});
const loading = ref(true)

const handleEdit = (index, row) => {
   console.log(index, row)
   router.push(`/EditPageView/${row.id}`)
};
const handleDelete = async (index, row) => {
   ElMessageBox.confirm(
      `是否删除${row.title}`,
      '警告',
      {
         confirmButtonText: '确定',
         cancelButtonText: '取消',
         type: 'warning',
      }
   )
      .then(async () => {
         console.log(index, row);
         let token = localStorage.getItem("Elog_jwtToken");
         loading.value = true;
         try {
            const response = await axios.delete('/api/Article/Delete', {
               headers: {
                  "Authorization": `Bearer ${token}`
               },
               params: {
                  id: row.id
               },
            });
            if (response.data.code === 200) {
               loading.value = false;
               fetchTableData();
            }
            else {
               ElMessage({
                  type: 'error',
                  message: '删除出错' + response.data.message,
               });
            }
         } catch (error) {
            console.error('获取表格数据时出错:', error);
         }
         ElMessage({
            type: 'success',
            message: '删除成功',
         });
      })
      .catch(() => {
         ElMessage({
            type: 'info',
            message: '删除已取消',
         });
      });
};

const fetchTableData = async () => {
   let token = localStorage.getItem("Elog_jwtToken");

   try {
      const response = await axios.get('/api/Article/GetArticleByUser', {

         headers: {
            "Authorization": `Bearer ${token}`
         },

         params: {

            pageNum: pageData.pageNum,
            pageSize: pageData.pageSize,
         },
      });
      const { data } = response.data;
      pageData.tableData = data.articles;
      pageData.total = data.count;
      loading.value = false;
   } catch (error) {
      console.error('Error fetching table data:', error);
   }
};

const handlePageChange = (newPage) => {
   loading.value = true;
   pageData.pageNum = newPage;
   fetchTableData();
};

onMounted(() => {
   fetchTableData();
});

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
   box-shadow: var(--el-box-shadow-lighter);
   margin: auto;
   padding-left: 20px;
}

.infocontent {
   padding: 20px;
   backdrop-filter: blur(6px);
   height: 100%;

}

.infomain {
   margin-top: 20px;
   padding-bottom: 60px;
   /* 为分页留出空间 */
}

.pagination-container {

   display: flex;
   flex-direction: row;
   justify-content: space-around;
   margin: 20px;

}

.el-pagination {

   --el-pagination-button-width: 50px;
   --el-pagination-button-height: 50px;
}

.example-showcase .el-loading-mask {
   z-index: 9;
}
</style>
