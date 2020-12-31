<template>
    <v-app id="vueApp">
        <v-main>
            <span>{{ debugMsg }}</span>

            <!-- toggable components -->
            <Login v-show="showLogin" />
            <CharacterCreation v-show="showCharacterCreation" />
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
import CharacterCreation from './components/CharacterCreation.vue'
import PlayerHud from './components/PlayerHud.vue'
import VehicleHud from './components/VehicleHud.vue'
import VehicleRadio from './components/VehicleRadio.vue'
import Notifications from './components/Notifications.vue'

export default Vue.extend({
    name: 'App',

    components: {
        Login,
        CharacterCreation,
        PlayerHud,
        VehicleHud,
        VehicleRadio,
        Notifications,
    },

    data: () => ({
        showLogin: false,
        showCharacterCreation: false,
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
                _me.toggleComponent(component, state)
            }
        )

        this.$alt.on('CopyToClipboard', (content: string) => {
            this.$copyText(content)
        })

        this.$alt.on('SetAppData', (key: string, value: any) => {
            this.$data[key] = value
        })

        this.$nextTick(() => {
            setTimeout(() => {
                this.$alt.emit('ui:Loaded')
            }, 200)
        })
    },

    methods: {
        toggleComponent(component: string, state?: boolean) {
            this.debugMsg = 'test'

            if (this.$data.hasOwnProperty('show' + component)) {
                if (typeof state === 'undefined') {
                    this.debugMsg =
                        'state was undefined (component: ' + component + ')'
                    const oldState = this.$data['show' + component]

                    this.$data['show' + component] = !oldState
                } else {
                    this.debugMsg =
                        'set ' + component + ' ' + state ? 'visible' : 'hidden'

                    this.$data['show' + component] = state
                }
            } else {
                this.debugMsg = 'missing property: show' + component
            }
        },
    },
})
</script>

<style>
* {
    user-select: none;
}

*,
*:focus {
    outline: none;
}

#vueApp {
    background: transparent;
}

#copyArea {
    visibility: hidden;
}
</style>
