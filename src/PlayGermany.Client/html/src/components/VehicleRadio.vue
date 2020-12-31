<template>
    <div v-show="playerInVehicle && focused" class="container">
        <div class="radio">
            <div class="station">
                <img :src="getAsset(currentRadioImage)" />
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { Howl, HowlCallback } from 'howler'
import Vue from 'vue'

let PlayerSwitching: Howl | null = null
let PlayerRadio: Howl | null = null
const PlayerBeep: Howl = new Howl({
    src: [require(`../assets/VehicleRadio/beep.mp3`)],
    volume: 0.1,
    html5: false,
    format: ['mp3'],
})

class RadioStation {
    constructor(
        public Name: string,
        public Image: string,
        public Url: string,
        public Volume: number
    ) {}
}

export default Vue.extend({
    name: 'VehicleRadio',

    props: {
        playerInVehicle: {
            type: Boolean,
            required: true,
        },
    },

    data: () => {
        return {
            volume: 40,
            currentRadio: 0,
            focused: false,
            loading: false,
            playing: false,
            switching: false,
            radioList: [new RadioStation('OFF', 'off.png', '', 0)],
            switchingAudios: [
                require('../assets/VehicleRadio/switching1.mp3'),
                require('../assets/VehicleRadio/switching2.mp3'),
                require('../assets/VehicleRadio/switching3.mp3'),
                require('../assets/VehicleRadio/switching4.mp3'),
                require('../assets/VehicleRadio/switching5.mp3'),
                require('../assets/VehicleRadio/switching6.mp3'),
            ],
        }
    },

    computed: {
        currentRadioImage() {
            return this.radioList[this.currentRadio].Image
        },
    },

    watch: {
        currentRadio() {
            this.$alt.emit(
                'ui:EmitServer',
                'Vehicle:RadioChanged',
                this.currentRadio
            )
        },

        playerInVehicle(value: boolean) {
            if (!value) {
                this.stopSwitching()
                this.stopRadio()
            }
        },
    },

    mounted() {
        window.addEventListener('wheel', event => {
            if (this.playerInVehicle && this.focused) {
                const delta = Math.sign(event.deltaY)
                this.mouseScroll(delta)
            }
        })

        window.addEventListener('keydown', event => {
            if (this.playerInVehicle && event.keyCode === 81) {
                this.focused = true
            }
        })

        window.addEventListener('keyup', event => {
            if (this.playerInVehicle && event.keyCode === 81) {
                this.focused = false
            }
        })

        this.$alt.on('radio:SwitchStation', radioStation => {
            this.gotoStation(radioStation)
        })

        this.$alt.on('radio:SetStations', (stations: any[]) => {
            this.setRadioStations(stations)
        })

        this.$alt.on('radio:StopPlaying', () => {
            this.stopSwitching()
            this.stopRadio()
        })
    },

    methods: {
        setRadioStations(stations: any[]) {
            for (const station of stations) {
                let vol = station.volume
                if (!vol) vol = 100

                this.radioList.push(
                    new RadioStation(
                        station.name,
                        station.image,
                        station.url,
                        vol
                    )
                )
            }
        },

        createHowl(
            url: string,
            volume: number,
            onplay?: HowlCallback,
            html5 = true
        ): Howl {
            return new Howl({
                src: [url],
                html5,
                format: ['mp3', 'aac'],
                volume: volume * 0.001,
                onplay,
                onplayerror: (id, error) => console.log(id + ': ' + error),
                onloaderror: (id, error) => console.log(id + ': ' + error),
            })
        },

        randomNumber(min: number, max: number) {
            return Math.floor(Math.random() * (max - min + 1) + min)
        },

        play() {
            this.stopSwitching()
            this.stopRadio()

            if (
                this.currentRadio !== 0 &&
                this.radioList[this.currentRadio].Url.length > 0
            ) {
                this.startSwitching()

                setTimeout(() => {
                    if (this.playing) return
                    const newRadio = this.radioList[this.currentRadio]
                    PlayerRadio = this.createHowl(
                        newRadio.Url,
                        newRadio.Volume,
                        this.stopSwitching
                    )

                    PlayerRadio.play()
                    this.playing = true
                }, this.randomNumber(500, 2100))
            }
        },

        mouseScroll(delta: number) {
            if (this.focused) {
                PlayerBeep.play()

                this.stopSwitching()

                if (delta > 0) {
                    this.nextStation()
                } else {
                    this.previousStation()
                }
            }
        },

        nextStation() {
            if (this.currentRadio === this.radioList.length - 1) {
                this.currentRadio = 0
            } else {
                this.currentRadio++
            }

            this.play()
        },

        previousStation() {
            if (this.currentRadio === 0) {
                this.currentRadio = this.radioList.length - 1
            } else {
                this.currentRadio--
            }

            this.play()
        },

        gotoStation(index: number) {
            this.currentRadio = index

            this.focused = true
            this.play()

            setTimeout(() => {
                this.focused = false
            }, 2000)
        },

        startSwitching() {
            if (!this.switching) {
                const audio = this.switchingAudios[this.randomNumber(0, 5)]

                PlayerSwitching = this.createHowl(audio, 50, undefined, false)
                PlayerSwitching.play()
            }

            this.switching = true
        },

        stopSwitching() {
            if (this.switching && PlayerSwitching != null) {
                PlayerSwitching.stop()
                PlayerSwitching.unload()
                PlayerSwitching = null
            }

            this.switching = false
        },

        stopRadio() {
            if (this.playing && PlayerRadio != null) {
                PlayerRadio.stop()
                PlayerRadio.unload()
                PlayerRadio = null
            }

            this.playing = false
        },

        getAsset(uri: string) {
            return require(`../assets/VehicleRadio/${uri}`)
        },
    },
})
</script>

<style scoped>
* {
    box-sizing: border-box;
}

.container {
    padding: 0 !important;
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    display: flex;
    justify-content: center;
    align-items: center;
}

.container .radio {
    width: 300px !important;
    height: 300px !important;
    padding: 0 !important;
    background-color: rgba(0, 0, 0, 0.5);
    float: right;
    color: #dadada;
    text-shadow: 0 0 #000000;
    font-size: 12px;
    display: flex;
    justify-content: center;
    align-items: center;
    border-radius: 50%;
}

.container .radio .station {
    width: 250px;
}

.container .radio .station img {
    width: 100%;
}
</style>
