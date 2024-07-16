<template>


<el-card  style="max-width: 480px">
    <template #header>
      <div class="card-header" >

        <slot name="Cname"></slot>
      </div>
    </template>

    <slot name="Ccompany"></slot>
  </el-card>


</template>
<script setup>

</script>
<style>
.card-header{

  max-height: 300px,
}

</style>