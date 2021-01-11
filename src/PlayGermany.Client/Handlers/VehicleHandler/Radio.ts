import * as alt from 'alt-client'
import * as natives from 'natives'
import KeyCodes from '../../Utils/KeyCodes'
import * as UiManager from '../../UiManager'

let radioUiActive = false

alt.on('enteredVehicle', (vehicle: alt.Vehicle, seat: number) => {
    const player = alt.Player.local

    let currentStation = 0
    if (player.vehicle.hasStreamSyncedMeta('radioStation')) {
        currentStation = player.vehicle.getStreamSyncedMeta('radioStation')
    }

    UiManager.setAppData('isPlayerInVehicle', true)
    UiManager.emit('VehicleRadio:SwitchStation', currentStation)
})

alt.on('leftVehicle', (vehicle: alt.Vehicle, seat: number) => {
    UiManager.setAppData('isPlayerInVehicle', false)
})

alt.on('streamSyncedMetaChange', (entity: alt.Entity, key: string, value: string) => {
    if (!(entity instanceof alt.Vehicle) || key === 'radioStation') {
        return
    }

    const player = alt.Player.local

    if (player.vehicle === entity) {
        if (player.hasMeta('seat') && player.getMeta('seat') !== 0) {
            // passenger
            UiManager.emit('VehicleRadio:SwitchStation', value)
        }
    } else {
        // later: we can play radio of nearby cars here
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
        //natives.setRadioToStationName('OFF')
    }
})
