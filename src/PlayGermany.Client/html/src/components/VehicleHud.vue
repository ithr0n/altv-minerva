<template>
    <div id="container">
        <div id="speedometer">
            <div class="line">
                <div>
                    <b><span class="num_1">0</span></b>
                </div>
                <div><b></b></div>
                <div>
                    <b><span class="num_2">1</span></b>
                </div>
                <div><b></b></div>
                <div>
                    <b><span class="num_3">2</span></b>
                </div>
                <div><b></b></div>
                <div>
                    <b><span class="num_4">3</span></b>
                </div>
                <div><b></b></div>
                <div>
                    <b><span class="num_5">4</span></b>
                </div>
                <div><b></b></div>
                <div>
                    <b><span class="num_6">5</span></b>
                </div>
                <div><b></b></div>
                <div>
                    <b><span class="num_7">6</span></b>
                </div>
                <div><b></b></div>
                <div>
                    <b><span class="num_8">7</span></b>
                </div>
                <div><b></b></div>
                <div>
                    <b><span class="num_9">8</span></b>
                </div>
                <div><b></b></div>
                <div>
                    <b><span class="num_10">9</span></b>
                </div>
                <div><b></b></div>
                <div>
                    <b><span class="num_11">10</span></b>
                </div>
            </div>
            <div id="needle" :style="{ transform: needlePosition }"></div>
            <div id="pin">
                <div id="gearIndexBefore">{{ gearPrevious }}</div>
                <div id="gearIndexCurrent">{{ gearCurrent }}</div>
                <div id="gearIndexNext">{{ gearNext }}</div>
            </div>
            <div id="kmh">
                <div id="speed">{{ speed }}</div>
                <div>km/h</div>
            </div>
            <div v-show="shifting">
                <i id="shift" class="mdi mdi-menu-up"></i>
            </div>
            <div v-show="handbrake">
                <i id="handbrake" class="mdi mdi-car-brake-alert"></i>
            </div>
            <div v-show="tractionControl">
                <i id="inair" class="mdi mdi-car-traction-control"></i>
            </div>
            <div v-show="lightState === 1">
                <i
                    id="lightsOn"
                    class="mdi mdi-car-light-dimmed"
                    style="color: #255894"
                ></i>
            </div>
            <div v-show="lightState === 2">
                <i
                    id="lightsOn"
                    class="mdi mdi-car-light-high"
                    style="color: #3b97ff"
                ></i>
            </div>
            <div v-show="!seatbelt">
                <i id="seatbelt" class="mdi mdi-seatbelt"></i>
            </div>
            <div v-show="isEngineRunning" id="fuel">
                <div id="fuelbar">
                    <div :style="{ height: fuelbarFilledPercentage }"></div>
                </div>
                <div><i class="mdi mdi-fuel"></i></div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import Vue from 'vue'

export default Vue.extend({
    name: 'VehicleHud',

    data: () => {
        return {
            isEngineRunning: true,
            isHandbrakeActive: true,
            isVehicleOnAllWheels: false,
            lightState: 1,
            rpm: 9100,
            speed: 193,
            gear: 1,
            isElectric: false,
            fuelPercentage: 80,
            seatbelt: false,
        }
    },

    computed: {
        shifting() {
            return Math.floor(this.rpm / 100) > 90
        },
        tractionControl() {
            return this.isEngineRunning && !this.isVehicleOnAllWheels
        },
        handbrake() {
            return this.isEngineRunning && this.isHandbrakeActive
        },
        gearPrevious() {
            if (!this.isEngineRunning) {
                return 'R'
            } else {
                if (this.speed === 0) return 'R'
                if (this.speed > 0 && this.gear === 0) return ''
                if (this.isElectric || this.gear === 1) return 'P'
                return this.gear - 1
            }
        },
        gearCurrent() {
            if (!this.isEngineRunning) {
                if (this.speed > 0) return 'N'
                return 'P'
            } else {
                if (this.speed === 0) return 'P'
                if (this.gear === 0 && this.speed > 0) return 'R'
                if (this.isElectric) return 'A'
                return this.gear
            }
        },
        gearNext() {
            if (!this.isEngineRunning) {
                if (this.speed > 0) return 'P'
                else return '1'
            } else {
                if (this.isElectric && this.gear >= 1) return ''
                if (this.gear >= 6) return ''
                return this.gear + 1
            }
        },
        fuelbarFilledPercentage() {
            return this.fuelPercentage + '%'
        },
        needlePosition() {
            return 'rotate(' + Math.round((this.rpm / 1000) * 27 + 180) + 'deg)'
        },
    },

    mounted() {
        const _me = this

        this.$alt.on('VehicleHud:Update', (data: any) => {
            _me.isEngineRunning = data.isEngineRunning
            _me.isHandbrakeActive = data.isHandbrakeActive
            _me.isVehicleOnAllWheels = data.isVehicleOnAllWheels
            _me.lightState = data.lightState
            _me.rpm = data.rpm
            _me.speed = data.speed
            _me.gear = data.gear
            _me.isElectric = data.isElectric
            _me.seatbelt = data.seatbelt
            _me.fuelPercentage = data.fuelPercentage
        })

        this.$alt.on('VehicleHud:Reset', () => {
            this.reset()
        })

        this.reset()
    },

    methods: {
        reset() {
            this.isEngineRunning = false
            this.isHandbrakeActive = true
            this.isVehicleOnAllWheels = false
            this.lightState = 0
            this.rpm = 0
            this.speed = 0
            this.gear = 1
            this.isElectric = false
            this.fuelPercentage = 0
            this.seatbelt = false
        },
    },
})
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
@font-face {
    font-family: 'Varela Round';
    font-style: normal;
    font-weight: 400;
    src: url('../assets/fonts/varela-round-v12-latin-regular.eot');
    src: local('Varela Round Regular'), local('VarelaRound-Regular'),
        url('../assets/fonts/varela-round-v12-latin-regular.eot?#iefix')
            format('embedded-opentype'),
        url('../assets/fonts/varela-round-v12-latin-regular.woff2')
            format('woff2'),
        url('../assets/fonts/varela-round-v12-latin-regular.woff')
            format('woff'),
        url('../assets/fonts/varela-round-v12-latin-regular.ttf')
            format('truetype'),
        url('../assets/fonts/varela-round-v12-latin-regular.svg#VarelaRound')
            format('svg');
}

* {
    margin: 0;
    padding: 0;
    font-family: 'Varela Round', sans-serif;
    font-size: 1vw;
}

::selection {
    background: transparent;
}

#container {
    position: absolute;
    width: 15vw;
    height: 15vw;
    right: 2vw;
    bottom: 2vw;
    overflow: hidden;
    --red: rgb(165, 32, 32);
    --orange: rgb(221, 140, 18);
    --blue: #193a61;
    --background: #141414;
}

#speedometer {
    background: var(--background);
    width: 100%;
    height: 100%;
    /*border: 0.1vw solid rgb(0, 0, 0);*/
    border-radius: 100%;
    display: block;
    position: relative;
    opacity: 1;
}

.line {
    position: absolute;
    width: 100%;
    height: 100%;
}

.line div {
    position: absolute;
    width: 100%;
    height: 100%;
    padding: 0.25vw;
    box-sizing: border-box;
}

.line div b {
    position: absolute;
    display: block;
    left: 50%;
    width: 0.1vw;
    height: 0.5vw;
    background: #fff;
}

.line div:nth-child(2n + 1) b {
    width: 0.15vw;
    height: 0.75vw;
}

.line div:nth-child(1) {
    transform: rotate(180deg);
}

.line div:nth-child(2) {
    transform: rotate(193.5deg);
}

.line div:nth-child(3) {
    transform: rotate(207deg);
}

.line div:nth-child(4) {
    transform: rotate(220.5deg);
}

.line div:nth-child(5) {
    transform: rotate(234deg);
}

.line div:nth-child(6) {
    transform: rotate(247.5deg);
}

.line div:nth-child(7) {
    transform: rotate(261deg);
}

.line div:nth-child(8) {
    transform: rotate(274.5deg);
}

.line div:nth-child(9) {
    transform: rotate(288deg);
}

.line div:nth-child(10) {
    transform: rotate(301.5deg);
}

.line div:nth-child(11) {
    transform: rotate(315deg);
}

.line div:nth-child(12) {
    transform: rotate(328.5deg);
}

.line div:nth-child(13) {
    transform: rotate(342deg);
}

.line div:nth-child(14) {
    transform: rotate(355.5deg);
}

.line div:nth-child(15) {
    transform: rotate(9deg);
}

.line div:nth-child(16) {
    transform: rotate(22.5deg);
}

.line div:nth-child(17) {
    transform: rotate(36deg);
}

.line div:nth-child(18) {
    transform: rotate(49.5deg);
}

.line div:nth-child(19) {
    transform: rotate(63deg);
}

.line div:nth-child(20) {
    transform: rotate(76.5deg);
}

.line div:nth-child(21) {
    transform: rotate(90deg);
}

[class^='num_'] {
    color: #fff;
    display: block;
    position: absolute;
    width: 3vw;
    font-size: 1vw;
    text-align: center;
    text-transform: uppercase;
    text-decoration: none;
    top: 1vw;
    left: -1.4vw;
}

.num_1 {
    transform: rotate(180deg);
}

.num_2 {
    transform: rotate(153deg);
}

.num_3 {
    transform: rotate(126deg);
}

.num_4 {
    transform: rotate(99deg);
}

.num_5 {
    transform: rotate(72deg);
}

.num_6 {
    transform: rotate(45deg);
}

.num_7 {
    transform: rotate(18deg);
}

.num_8 {
    transform: rotate(351deg);
}

.num_9 {
    transform: rotate(324deg);
}

.num_10 {
    transform: rotate(297deg);
}

.num_11 {
    transform: rotate(270deg);
    top: 1.25vw;
}

.line div:nth-child(19) > b,
.line div:nth-child(20) > b,
.line div:nth-child(21) > b {
    background-color: var(--red);
}

.num_10,
.num_11 {
    color: var(--red);
}

#needle {
    background-color: var(--red);
    position: absolute;
    height: 0;
    width: 0;
    left: 50%;
    top: 50%;
    border-radius: 50% 50% 0 0;
    margin: -4.5vw -0.2vw 0;
    padding: 4.5vw 0.2vw 0;
    z-index: 2;
    transform-origin: 50% 100%;
    transform: rotate(70deg);
}

#pin {
    position: absolute;
    width: 3vw;
    height: 3vw;
    left: calc(50% - 1.5vw);
    top: calc(50% - 1.5vw);
    background-color: #000;
    color: #fff;
    border: 0.1vw solid #fff;
    border-radius: 50%;
    z-index: 3;
    font-size: 0.75vw;
    display: flex;
    align-items: center;
    justify-content: center;
}

#pin > div:nth-child(2) {
    padding: 0.1vw;
    font-size: 2vw;
}

#kmh {
    width: 5vw;
    height: 5vw;
    position: absolute;
    color: #fff;
    bottom: 1.5vw;
    right: 1.5vw;
    display: flex;
    align-items: center;
    justify-content: center;
    flex-direction: column;
    font-size: 0.5vw;
}

#speed {
    font-size: 2vw;
}

#shift,
#handbrake,
#inair,
#lightsOn,
#seatbelt {
    font-size: 2vw;
    width: 2vw;
    height: 2vw;
    line-height: 2vw;
    text-align: center;
    position: absolute;
    top: calc(50% - 3.5vw);
}

#shift {
    color: var(--red);
    left: calc(50% - 1vw);
}

#handbrake,
#inair,
#lightsOn,
#seatbelt {
    left: calc(30% - 1vw);
    font-size: 1vw;
    color: var(--orange);
}

#inair {
    left: calc(70% - 1vw);
    top: calc(50% - 1vw);
}

#handbrake {
    left: calc(70% - 1vw);
}

#lightsOn {
    top: calc(70% - 1vw);
    left: calc(50% - 1vw);
    color: var(--blue);
}

#seatbelt {
    top: calc(79% - 1vw);
    left: calc(50% - 1vw);
    color: var(--red);
    animation-duration: 800ms;
    animation-name: blink;
    animation-iteration-count: infinite;
    animation-direction: alternate;
}

@keyframes blink {
    from {
        /*opacity: 1;*/
        color: var(--orange);
    }

    to {
        /*opacity: 0;*/
        color: var(--red);
    }
}

#fuel {
    position: absolute;
    display: flex;
    align-items: flex-start;
    justify-content: center;
    flex-direction: column;
    top: 35%;
    left: 30%;
    color: #007c20;
    font-size: 0.5vw;
}

#fuelbar {
    opacity: 0.5;
    height: 4vw;
    width: 0.5vw;
    border: 0.01vw solid #7a7a7a;
    margin-bottom: 0.1vw;
    transform: rotate(180deg);
    margin: 0 auto;
}

#fuelbar > div {
    width: 100%;
    background-color: #00ff42;
}
</style>
