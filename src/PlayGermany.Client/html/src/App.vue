<template>
    <div id="container">
        <!--<VuetifyExample />-->
        <span>{{ debugMsg }}</span>
        <PlayerHud v-show="showPlayerHud" />
        <VehicleHud v-show="showVehicleHud" />
    </div>
</template>

<script lang="ts">
import Vue from 'vue'
/*import VuetifyExample from './components/VuetifyExample.vue'*/
import PlayerHud from './components/PlayerHud.vue'
import VehicleHud from './components/VehicleHud.vue'

export default Vue.extend({
    name: 'App',

    components: {
        /*VuetifyExample,*/
        PlayerHud,
        VehicleHud,
    },

    data: () => ({
        showPlayerHud: false,
        showVehicleHud: false,
        debugMsg: 'debug',
    }),

    mounted() {
        const _me = this

        this.$alt.on(
            'ToggleComponent',
            (component: string, state?: boolean) => {
                _me.debugMsg = 'test1'

                if (_me.$data.hasOwnProperty('show' + component)) {
                    const oldState = _me.$data['show' + component]

                    if (!state) {
                        _me.$data['show' + component] = !oldState
                    } else {
                        _me.$data['show' + component] = state
                    }
                }
            }
        )
    },
})
</script>
