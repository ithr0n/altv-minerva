<template>
    <div class="container">
        <div class="playerName">{{ playerName }}</div>

        <div class="cash">$ {{ cashFormatted }}</div>

        <div v-visible="displayHunger" :class="{ pulseElement: pulseHunger }">
            <i
                :class="[
                    { pulseIcon: pulseHunger },
                    'mdi',
                    'mdi-food-drumstick',
                    { hungerColor: !displayHungerWarn },
                    { warnColor: displayHungerWarn },
                ]"
            ></i>
        </div>

        <div v-visible="displayThirst" :class="{ pulseElement: pulseThirst }">
            <i
                :class="[
                    { pulseIcon: pulseThirst },
                    'mdi',
                    'mdi-cup-water',
                    { thirstColor: !displayThirstWarn },
                    { warnColor: displayThirstWarn },
                ]"
            ></i>
        </div>

        <div>
            <i v-show="voiceIndex === 0" class="mdi mdi-volume-off"></i>
            <i v-show="voiceIndex === 1" class="mdi mdi-volume-low"></i>
            <i v-show="voiceIndex === 2" class="mdi mdi-volume-medium"></i>
            <i v-show="voiceIndex === 3" class="mdi mdi-volume-high"></i>
            <i v-show="voiceIndex === 4" class="mdi mdi-volume-vibrate"></i>
        </div>
    </div>
</template>

<script lang="ts">
import Vue from 'vue'

export default Vue.extend({
    name: 'PlayerHud',

    data: () => {
        return {
            playerName: 'Spielername',
            cash: 1000,
            hunger: 100,
            thirst: 100,
            voiceIndex: 2,
        }
    },

    computed: {
        cashFormatted() {
            return this.cash?.toFixed(2)
        },
        displayHunger() {
            return this.hunger < 50
        },
        displayThirst() {
            return this.thirst < 50
        },
        displayHungerWarn() {
            return this.hunger < 20
        },
        displayThirstWarn() {
            return this.thirst < 20
        },
        pulseHunger() {
            return this.hunger < 10
        },
        pulseThirst() {
            return this.thirst < 10
        },
    },

    mounted() {
        this.$alt.on('PlayerHud:SetData', (key: string, value: any) => {
            if (this.$data.hasOwnProperty(key)) {
                if (isNaN(+value)) {
                    this.$data[key] = String(value)
                } else {
                    this.$data[key] = Number(value)
                }
            }
        })
    },
})
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
.container {
    position: absolute;
    top: 5vw;
    right: 2vw;
    text-align: right;
}

.playerName {
    font-weight: bold;
    font-size: 30px;
    font-style: italic;
}

.cash {
    color: rgb(0, 168, 0);
    font-size: 42px;
    font-family: 'Pricedown', arial;
    text-align: right;
    text-shadow: -2px -2px 0 #000, 2px -2px 0 #000, -2px 2px 0 #000,
        2px 2px 0 #000;
}

.hungerColor {
    color: rgb(157, 92, 0);
}

.thirstColor {
    color: rgb(20, 78, 169);
}

.warnColor {
    color: rgb(255, 0, 0);
}

i {
    text-align: center;
    color: darkslategray;
    margin-left: 8px;
    font-size: 50px;
}

.pulseIcon {
    text-shadow: 0 0 0 rgba(0, 0, 0, 0);
    animation: pulseShadow 0.3s infinite alternate;
}

.pulseElement {
    transform: scale(0.9);
    animation: pulseSize 0.3s infinite alternate;
}

@keyframes pulseShadow {
    to {
        text-shadow: 0 0 80px rgba(151, 0, 0, 1);
    }
}

@keyframes pulseSize {
    to {
        transform: scale(1);
    }
}
</style>
