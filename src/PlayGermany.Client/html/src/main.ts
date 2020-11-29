import Vue from 'vue'
import App from './App.vue'
import vuetify from './plugins/vuetify'
import alt from './plugins/alt'
import 'roboto-fontface/css/roboto/roboto-fontface.css'
import store from './store'

Vue.config.productionTip = false

Vue.use(alt)

new Vue({
    vuetify,
    store,
    render: h => h(App),
}).$mount('#app')
