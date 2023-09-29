import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import axios from 'axios'
import VueAxios from 'vue-axios'
import VueCookies from 'vue-cookies';
import "bootstrap/dist/css/bootstrap.min.css"
import "bootstrap"

const app = createApp(App).use(store);
app.use(router);
app.use(VueAxios, axios);
app.use(VueCookies);
app.provide('axios', app.config.globalProperties.axios);

app.axios.defaults.baseURL = process.env.VUE_APP_TODO_BASE_URL;
app.config.errorHandler = function (err, vm, info) {
    console.log(err);
    console.log(vm);
    console.log(info);
    let url = window.location.protocol + "//" + window.location.hostname;
    window.location.replace(url);
}

app.mount('#app');