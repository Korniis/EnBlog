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
        <!-- <span class="article-meta">
          <i></i> <span class="article-meta-separator">|</span><i class="fas fa-comments"></i><a class="twikoo-count"
            href="">
            <slot name="articlenum"></slot>
          </a><span class="article-meta-label"> 人看过</span></span> -->
      </div>
      <div class="content">
        <slot name="artcontent"></slot>
      </div>
    </div>
    <div class="img-content">
      <a href="/" style="">
        <el-image class="showArtImg" error :src="imgsrc" lazy style="height: 100%; width: 100%;  " fit="cover">
          <template #error>
            <div class="error-content">
              <div class="error-background"></div>
              <p class="error-text">图片失踪了</p>
            </div>
          </template>

        </el-image>
      </a>
    </div>
  </div>

</template>
<script setup>
import { ElImage } from 'element-plus';

let propsData = defineProps(['imgsrc', "psgid"])

</script>
<style>
.showcontent {
  margin-bottom: 20px;
  background-color: white;
  box-shadow: var(--el-box-shadow-lighter);
  height: 18em;
  border-radius: 20px;
  display: flex;
  flex-direction: row;
  align-items: center;
  overflow: auto;
  transition: transform 0.3s ease-in-out, box-shadow 0.3s ease-in-out;
}

.showcontent:hover {
  transform: scale(1.02);
  box-shadow: 10px 10px 20px rgba(0, 0, 0, 0.15);

  overflow: hidden;
}

.article-content {
  padding: 20px;
  width: 55%;
  display: flex;
  flex-direction: column;
  text-align: left;
  overflow: hidden;
}

.article-title {
  font-size: 1.5em;
  color: #1f2d3d;
  text-decoration: none;
  margin-bottom: 10px;
  transition: color 0.3s;
}

.article-title:hover {
  color: #49b1f5;
}

.article-meta-wrap {
  font-size: 0.9em;
  color: #888;
  margin-bottom: 15px;
}

.article-meta-wrap i {
  margin-right: 5px;
}

.content {
  display: -webkit-box;
  -webkit-box-orient: vertical;
  -webkit-line-clamp: 3;
  overflow: hidden;
  text-overflow: ellipsis;
}

.img-content {
  height: 100%;
  width: 45%;
  overflow: hidden;
  border-radius: 0 20px 20px 0;
}

.showArtImg {
  transition: transform 0.3s ease-in-out;
  height: 100%;
  width: 100%;
}

.showArtImg:hover {
  transform: scale(1.05);
}

.error-content {
  position: relative;
  width: 100%;
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
  text-align: center;
  color: #fff;
  background-color: rgba(98, 85, 85, 0.3);
  border-radius: 10px;
  overflow: hidden;
}

.error-background {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-image: url('../../public/artimg/image.png');
  background-size: cover;
  background-position: center;
  filter: blur(2px);
  opacity: 0.7;
}

.error-text {
  position: relative;
  font-size: 18px;
  font-weight: bold;
  z-index: 1;
}
</style>