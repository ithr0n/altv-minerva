import Vue from 'vue'
import Vuex from 'vuex'
import playerHud from './modules/playerHud'

Vue.use(Vuex)

export default new Vuex.Store({
    modules: {
        playerHud,
    },
})
