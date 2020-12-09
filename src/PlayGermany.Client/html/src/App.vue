<template>
    <v-app>
        <v-main>
            <span>{{ debugMsg }}</span>

            <!-- toggable components -->
            <Login v-show="showLogin" />
            <PlayerHud v-show="showPlayerHud" />
            <VehicleHud v-show="showVehicleHud" />

            <!-- always rendered components -->
            <VehicleRadio :playerInVehicle="isPlayerInVehicle" />
            <Notifications />
        </v-main>
    </v-app>
</template>

<script lang="ts">
import Vue from 'vue'
import Login from './components/Login.vue'
import PlayerHud from './components/PlayerHud.vue'
import VehicleHud from './components/VehicleHud.vue'
import VehicleRadio from './components/VehicleRadio.vue'
import Notifications from './components/Notifications.vue'

export default Vue.extend({
    name: 'App',

    components: {
        Login,
        PlayerHud,
        VehicleHud,
        VehicleRadio,
        Notifications,
    },

    data: () => ({
        showLogin: false,
        showPlayerHud: false,
        showVehicleHud: false,
        isPlayerInVehicle: false,
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
                        const oldState = _me.$data['show' + component]
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

        this.$alt.on('SetAppData', (key: string, value: any) => {
            this.$data[key] = value
        })

        this.$alt.emit('loaded')
    },
})
</script>

<style>
* {
    user-select: none;
}

#copyArea {
    visibility: hidden;
}
</style>
