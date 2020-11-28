import Vue from 'vue'
import Vuetify from 'vuetify'
import de from 'vuetify/src/locale/de'

Vue.use(Vuetify)

export default new Vuetify({
    lang: {
        locales: { de },
        current: 'de',
    },
    icons: {
        iconfont: 'fa',
    },
})
