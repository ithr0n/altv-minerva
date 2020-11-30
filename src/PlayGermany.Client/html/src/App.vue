<template>
    <div id="container">
        <!--<VuetifyExample />-->
        <span>{{ debugMsg }}</span>
        <PlayerHud v-show="showPlayerHud" />
        <VehicleHud v-show="showVehicleHud" />
        <Notifications />
    </div>
</template>

<script lang="ts">
import Vue from 'vue'
/*import VuetifyExample from './components/VuetifyExample.vue'*/
import PlayerHud from './components/PlayerHud.vue'
import VehicleHud from './components/VehicleHud.vue'
import Notifications from './components/Notifications.vue'

export default Vue.extend({
    name: 'App',

    components: {
        /*VuetifyExample,*/
        PlayerHud,
        VehicleHud,
        Notifications,
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
                _me.debugMsg = 'test'
                if (_me.$data.hasOwnProperty('show' + component)) {
                    if (typeof state === 'undefined') {
                        _me.debugMsg =
                            'state was undefined (component: ' + component + ')'
                        const oldState = !_me.$data['show' + component]
                        _me.$data['show' + component] = !oldState
                    } else {
                        _me.debugMsg =
                            'set ' + component + ' ' + state
                                ? 'visible'
                                : 'hidden'
                        _me.$data['show' + component] = state
                    }
                } else {
                    _me.debugMsg = 'missing property: show' + component
                }
            }
        )

        this.$alt.on('CopyToClipboard', (content: string) => {
            this.$copyText(content)
        })

        this.$alt.emit('loaded')
    },
})
</script>

<style>
#copyArea {
    visibility: hidden;
}
</style>
