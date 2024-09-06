<template>
    <el-card class="weather-card">
        <div class="weather-header">
            <h3>{{ weatherData.city }} 天气预报</h3>
            <p>{{ weatherData.reporttime }}</p>
        </div>
        <div class="weather-body">
            <div class="weather-info">
                <p><strong>天气：</strong>{{ weatherData.weather }}</p>
                <p><strong>温度：</strong>{{ weatherData.temperature }}°C</p>
                <p><strong>风向：</strong>{{ weatherData.winddirection }}</p>
                <p><strong>风力：</strong>{{ weatherData.windpower }}</p>
                <p><strong>湿度：</strong>{{ weatherData.humidity }}%</p>
            </div>
        </div>
    </el-card>

</template>
<script setup>
import axios from 'axios';
import { onMounted, reactive } from 'vue';
let weatherData = reactive({
    city: '东城区',
    weather: '大雨',
    temperature: '23',
    winddirection: '东北',
    windpower: '≤3',
    humidity: '100',
    reporttime: '2024-08-26 00:02:54'
});
const getWeather = () => {

    // axios.get("https://restapi.amap.com/v3/weather/weatherInfo?city=370100&key=0fe622aa5232ae19b916ff96ac6aae7a", {
    //     headers: {
    //         'X-Requested-With': 'XMLHttpRequest'
    //     }
    // }).then(res => {
    //     console.log(res.data);
    //     Object.assign(weatherData, res.data.lives[0]);
    // }).catch(err => {
    //     console.error(err);
    // });


}
onMounted(()=>{


    getWeather();



})

</script>
<style>
.weather-card {
    border-radius: 10px;
    margin: 10px 0px 20px 0;
    background: linear-gradient(to bottom right, #8EC5FC, #E0C3FC);
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    color: #333;
    max-width: 480px;
    border-radius: 20px;
    display: block;
}

.weather-header {
    text-align: center;
    margin-bottom: 20px;
}

.weather-header h3 {
    margin: 0;
    font-size: 20px;
    color: #fff;
}

.weather-header p {
    margin: 0;
    font-size: 14px;
    color: #ddd;
}

.weather-body {
    display: flex;
    flex-direction: column;
    align-items: center;
}

.weather-info {
    width: 100%;
    text-align: center;
}

.weather-info p {
    margin: 5px 0;
    font-size: 16px;
    color: #fff;
}
</style>