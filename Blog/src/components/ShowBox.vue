<template>



  <div class="showcontent">
    <div class="article-content">
      <a class="article-title" :href="`/ContentPage/${psgid}`">
        <slot name="article-title"></slot>
      </a>
      <div class="article-meta-wrap"><span class="post-meta-date"><i class="far fa-calendar-alt"></i><span
            class="article-meta-label">发表于 </span>
          <slot name="articletime"></slot>
        </span>
        <span class="article-meta">
          <i></i> <span class="article-meta-separator">|</span><i class="fas fa-comments"></i><a class="twikoo-count"
            href="">
            <slot name="articlenum"></slot>
          </a><span class="article-meta-label"> 人看过</span></span>
      </div>
      <div class="content" >
        <slot name="artcontent"></slot>
      </div>
    </div>
    <div class="img-content">
      <a href="/" style="">
        <el-image class="showArtImg" :src="imgsrc" style="  height: 100%; width: 100%;  " fit="cover"></el-image>
      </a>
    </div>
  </div>

</template>
<script setup>
import { ElImage } from 'element-plus';

let propsData = defineProps(['imgsrc', "psgid"])

</script>
<style>
.article-title {
  font-size: 1.42em;
  color: #1f2d3d;
  text-decoration: none;

  margin: 20px 0;
}

.article-title:hover {
  color: #49b1f5;
  ;
}

.showcontent {
  margin-bottom: 20px;
  background-color: white;
  box-shadow: var(--el-box-shadow-lighter);
  height: 18em;
  border-radius: 20px;
  display: flex;
  flex-direction: row;
  align-items: center;
  overflow: hidden;
}

.article-content {
  padding: 0 0px;
  display: flex;
  width: 57%;
  flex-direction: column;
  text-align: center;
}

.img-content {
  height: 100%;
  width: 43%;
}

.showArtImg {
  transition-duration: 1s;

}

.showArtImg:hover {

  -webkit-transform: scale(1.1);
  -moz-transform: scale(1.1);
  -o-transform: scale(1.1);
  -ms-transform: scale(1.1);
  transition-duration: 1s;
  transform: scale(1.1);
}

.content {
  display: -webkit-box;

  -webkit-box-orient: vertical;

  -webkit-line-clamp:3  ;

  overflow: inherit;
  max-width:  500px; ;
  height: 100px;
}
</style>