import * as alt from 'alt-client'
import * as natives from 'natives'
import KeyCodes from '../../Utils/KeyCodes'
import * as UiManager from '../../UiManager'

let radioUiActive = false

const radioStations = [
    {
        name: 'Play-Germany Radio',
        url: 'http://gtaradio.play-germany.de:8000/stream',
        image: 'playgermany.png'
    },
    {
        name: '011.FM - Country Roads',
        url: 'http://listen.011fm.com:8016/stream09',
        image: '011-fm.png'
    },
    {
        name: 'Insanity Radio 103.2FM',
        url: 'https://stream.cor.insanityradio.com/insanity320.mp3',
        image: 'insanity.png'
    },
    {
        name: 'B4B Disco Funk',
        url: 'https://radio9.pro-fhi.net/radio/9164/stream',
        image: 'b4b.png'
    },
    {
        name: 'One Love Hip Hop Radio',
        url: 'http://listen.one.hiphop/live.mp3',
        image: 'onelove.png',
        volume: 60
    },
    {
        name: 'ADR.FM',
        url: 'http://192.111.140.6:9683/stream',
        image: 'adr.png'
    },
    {
        name: 'SWR1',
        url: 'http://swr-swr1-bw.cast.addradio.de/swr/swr1/bw/mp3/64/stream.mp3',
        image: 'swr1.png'
    }
]

UiManager.on('ui:Loaded', () => {
    alt.log('radio stations set')
    UiManager.emit('radio:SetStations', radioStations)
})

alt.onServer('playerEnteredVehicle', (vehicle: alt.Vehicle, seat: number) => {
    const player = alt.Player.local

    let currentStation = 0
    if (player.vehicle.hasStreamSyncedMeta('radioStation')) {
        currentStation = player.vehicle.getStreamSyncedMeta('radioStation')
    }

    UiManager.setAppData('isPlayerInVehicle', true)
    UiManager.emit('radio:SwitchStation', currentStation)
})

alt.onServer('playerLeftVehicle', (vehicle: alt.Vehicle, seat: number) => {
    UiManager.setAppData('isPlayerInVehicle', false)
})

alt.on('streamSyncedMetaChange', (entity: alt.Entity, key: string, value: string) => {
    if (!(entity instanceof alt.Vehicle)) {
        return
    }

    const player = alt.Player.local

    if (key === 'radioStation') {
        if (player.vehicle === entity && player.hasMeta('seat') && player.getMeta('seat') !== -1) {
            UiManager.emit('radio:SwitchStation', value)
        } else {
            // later: we can play radio of nearby cars here
        }
    }
})
// alt.on('syncedMetaChange', (entity, key, value) => {
//     if (entity != player.vehicle) return
//     const pedInSeat = native.getPedInVehicleSeat(player.vehicle.scriptID, -1)
//     if (key != 'radioStation') return
//     if (pedInSeat == player.scriptID) return

//     let radioStation = value

//     if (browser && mounted) {
//         browser.emit('switchRadio', radioStation)
//     }
// })

alt.on('keydown', key => {
    const player = alt.Player.local

    if (!player.vehicle) {
        return
    }

    if (key === KeyCodes.Q_KEY && player.hasMeta('seat')) {
        if ([-1, 0].includes(+player.getMeta('seat'))) {
            // driver or first passenger
            radioUiActive = true
        }
    }
})

alt.on('keyup', key => {
    const player = alt.Player.local

    if (!player.vehicle) {
        return
    }

    if (key === KeyCodes.Q_KEY && player.hasMeta('seat')) {
        if ([-1, 0].includes(+player.getMeta('seat'))) {
            radioUiActive = false
        }
    }
})


alt.everyTick(() => {
    const player = alt.Player.local

    if (player.vehicle) {
        // disable native radio wheel (Q)
        natives.disableControlAction(0, 85, true)
    } else {
        natives.enableControlAction(0, 85, true)
    }

    if (radioUiActive) {
        // disable weapon switch on mouse wheel if UI is focused
        natives.disableControlAction(0, 99, true)
        natives.disableControlAction(0, 100, true)
    } else {
        natives.enableControlAction(0, 99, true)
        natives.enableControlAction(0, 100, true)
    }

    if (natives.isPedSittingInAnyVehicle(player.scriptID)) {
        natives.setRadioToStationName('OFF')
    }
})
