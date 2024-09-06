<template>
    <el-card class="todo-card" shadow="always">
        <div class="card-content">
            <div class="todo-header">
                <h3>{{ todo.title }}</h3>
                <el-tag :type="statusTagType">{{ todo.status }}</el-tag>
            </div>
            <div class="todo-body">
                <p><strong>描述：</strong>{{ todo.description }}</p>
                <p><strong>截止日期：</strong>{{ formatDate(todo.dueDate) }}</p>
            </div>
            <div class="todo-footer">
                <el-button type="primary" @click="markAsDone">完成</el-button>
                <el-button type="danger" @click="deleteTodo">删除</el-button>
            </div>
        </div>
    </el-card>
</template>
<script setup>
import { computed, defineProps, ref } from 'vue';
import { ElTag, ElButton } from 'element-plus';

const props = defineProps({
    todo: {
        type: Object,
        required: true
    }
});

const statusTagType = computed(() => {
    switch (props.todo.status) {
        case '未开始':
            return 'warning';
        case '进行中':
            return 'info';
        case '已完成':
            return 'success';
        default:
            return 'default';
    }
});

const formatDate = (date) => {
    return new Date(date).toLocaleDateString();
};

const markAsDone = () => {
    // 处理标记为完成的逻辑
    console.log('标记为完成', props.todo);
};

const deleteTodo = () => {
    // 处理删除待办事项的逻辑
    console.log('删除待办事项', props.todo);
};
</script>

<style scoped>
.todo-card {
    max-width: 480px;

}

.todo-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
}

.todo-body {
    margin: 0;
}

.todo-footer {
    display: flex;
    justify-content: flex-end;
}
</style>