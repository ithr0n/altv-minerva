<template>
    <div id="container">
        <span class="cashColor cashFont">$ {{ cashFormatted }}</span>
        <i class="cashColor mdi mdi-cash-usd"></i>

        <span class="hungerColor needsFont">{{ hunger }}&nbsp;%</span>
        <i class="hungerColor mdi mdi-food-drumstick"></i>

        <span class="thirstColor needsFont">{{ thirst }}&nbsp;%</span>
        <i class="thirstColor mdi mdi-cup-water"></i>
    </div>
</template>

<script lang="ts">
import Vue from 'vue'

export default Vue.extend({
    name: 'PlayerHud',

    data: () => {
        return {
            cash: 1000,
            hunger: 100,
            thirst: 100,
        }
    },

    computed: {
        cashFormatted() {
            return this.cash.toFixed(2)
        },
    },

    mounted() {
        const _me = this
        this.$alt.on('PlayerHud:UpdateCash', cash => {
            _me.cash = cash
        })
        this.$alt.on('PlayerHud:UpdateHunger', hunger => {
            _me.hunger = hunger
        })
        this.$alt.on('PlayerHud:UpdateThirst', thirst => {
            _me.thirst = thirst
        })
    },
})
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
#container {
    display: grid;
    grid-template-columns: 1fr auto;
    position: absolute;
    right: 30px;
    top: 30px;
    font-weight: bold;
    font-size: 37px;
    text-align: right;
}

.cashColor {
    color: rgb(0, 168, 0);
}

.cashFont {
    font-family: 'Pricedown', arial;
    text-align: right;
    text-shadow: -2px -2px 0 #000, 2px -2px 0 #000, -2px 2px 0 #000,
        2px 2px 0 #000;
}

.hungerColor {
    color: orange;
}

.thirstColor {
    color: cornflowerblue;
}

.needsFont {
    font-family: 'Pricedown', arial;
    text-align: right;
    text-shadow: -2px -2px 0 #000, 2px -2px 0 #000, -2px 2px 0 #000,
        2px 2px 0 #000;
}

i {
    text-align: center;
    color: darkslategray;
    margin-left: 8px;
}
</style>
