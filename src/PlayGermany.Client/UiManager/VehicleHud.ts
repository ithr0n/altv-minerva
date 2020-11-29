import * as alt from 'alt-client'
import * as natives from 'natives'
import KeyCodes from '../Utils/KeyCodes'

let electric = [
    2445973230,// neon
    1560980623,// airtug
    1147287684,// caddy
    3164157193,// dilettante
    2400073108,// surge
    544021352,// khamelion 
    2672523198,// voltic
    1031562256,// tezeract
    1392481335,// cyclone
    2765724541// raiden
]

let handbrakeActive = false

alt.on('keydown', (key) => {
    if (key === KeyCodes.VK_SPACE) handbrakeActive = true

    if (key === KeyCodes.VK_ADD) {
        let currenctVehicle = alt.Player.local.vehicle

        if (currenctVehicle) {
            let isRunning = natives.getIsVehicleEngineRunning(currenctVehicle.scriptID)
            if (isRunning) {
                natives.setVehicleEngineOn(currenctVehicle.scriptID, false, true, true)
            } else {
                natives.setVehicleEngineOn(currenctVehicle.scriptID, true, true, true)
            }
        }
    }
})
alt.on('keyup', (key) => {
    if (key === KeyCodes.VK_SPACE) handbrakeActive = false
})

alt.everyTick(() => {
    let vehicle = alt.Player.local.vehicle

    if (vehicle) {
        const [_, lowBeam, highBeam] = natives.getVehicleLightsState(vehicle.scriptID, undefined, undefined)
        let lightState = 0
        if (lowBeam && !highBeam) lightState = 1
        if (lowBeam && highBeam) lightState = 2

        alt.emit('UiManager:Emit', 'VehicleHud:Update', {
            gear: vehicle.gear,
            rpm: Math.floor(vehicle.rpm * 10000),
            speed: Math.floor(natives.getEntitySpeed(vehicle.scriptID) * 3.6),
            isElectric: electric.includes(vehicle.model),
            isEngineRunning: natives.getIsVehicleEngineRunning(vehicle.scriptID),
            isVehicleOnAllWheels: natives.isVehicleOnAllWheels(vehicle.scriptID),
            isHandbrakeActive: handbrakeActive,
            lightState,
            fuelPercentage: 80, // todo
            seatbelt: false // todo
        })
    }
})

alt.onServer('playerEnteredVehicle', (vehicle, seat) => {
    alt.emit('UiManager:ShowComponent', 'VehicleHud')
})

alt.onServer('playerLeftVehicle', (vehicle, seat) => {
    alt.emit('UiManager:HideComponent', 'VehicleHud')
    alt.emit('UiManager:Emit', 'VehicleHud:Reset')
})
