import Vue from 'vue'
import App from './App.vue'
import vuetify from './plugins/vuetify'
import 'roboto-fontface/css/roboto/roboto-fontface.css'
import alt from './plugins/alt'
import VueVisible from 'vue-visible'
import VueClipboard from 'vue-clipboard2'
import Toast, { TYPE } from 'vue-toastification'
import 'vue-toastification/dist/index.css'
import store from './store'

Vue.config.productionTip = false

Vue.use(alt)
Vue.use(VueVisible)
Vue.use(VueClipboard)
Vue.use(Toast, {
    toastDefaults: {
        [TYPE.ERROR]: {
            timeout: 6000,
            hideProgressBar: false,
            showCloseButtonOnHover: true,
            icon: {
                iconClass: 'mdi mdi-alert',
            },
        },
        [TYPE.WARNING]: {
            timeout: 4500,
            hideProgressBar: false,
            showCloseButtonOnHover: true,
            icon: {
                iconClass: 'mdi mdi-message-alert',
            },
        },
        [TYPE.SUCCESS]: {
            timeout: 3000,
            hideProgressBar: true,
            showCloseButtonOnHover: true,
            icon: {
                iconClass: 'mdi mdi-party-popper',
            },
        },
        [TYPE.INFO]: {
            timeout: 3000,
            hideProgressBar: true,
            showCloseButtonOnHover: true,
            icon: {
                iconClass: 'mdi mdi-information-variant',
            },
        },
        [TYPE.DEFAULT]: {
            timeout: 3000,
            hideProgressBar: true,
            showCloseButtonOnHover: true,
            icon: {
                iconClass: 'mdi mdi-message-reply',
            },
        },
    },
})

new Vue({
    vuetify,
    store,
    render: h => h(App),
}).$mount('#app')
