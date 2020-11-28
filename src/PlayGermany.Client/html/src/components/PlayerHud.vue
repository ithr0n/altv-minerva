<template>
    <div id="displayCash">
        {{ cash }}
    </div>
</template>

<script lang="ts">
import Vue from 'vue'

export default Vue.extend({
    name: 'PlayerHud',

    props: {
        cash: {
            type: String,
        },
    },

    mounted: () => {},
})
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
#displayCash {
    position: absolute;
    right: 5%;
    top: 3%;
    color: rgb(0, 168, 0);
    font-family: 'Pricedown', arial;
    font-weight: 100;
    font-size: 42px;
    text-shadow: -2px -2px 0 #000, 2px -2px 0 #000, -2px 2px 0 #000,
        2px 2px 0 #000;
}
</style>
