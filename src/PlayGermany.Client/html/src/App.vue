<template>
    <div id="container">
        <!--<VuetifyExample />-->
        <PlayerHud />
        <VehicleHud />
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
        //
    }),
})
</script>
