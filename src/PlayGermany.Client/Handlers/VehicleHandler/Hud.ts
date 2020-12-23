import * as alt from 'alt-client'
import * as natives from 'natives'
import KeyCodes from '../../Utils/KeyCodes'
import * as UiManager from '../../UiManager'

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
})
alt.on('keyup', (key) => {
    if (key === KeyCodes.VK_SPACE) handbrakeActive = false
})

alt.everyTick(() => {
    let vehicle = alt.Player.local.vehicle

    if (vehicle) {
        const [_, lowBeam, highBeam] = natives.getVehicleLightsState(vehicle.scriptID, undefined, undefined)
        let lightState = 0
        if (lowBeam) lightState = 1
        if (highBeam) lightState = 2

        UiManager.emit('VehicleHud:Update', {
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

const vehiclesWithHud: number[] = [
    0, // compacts
    1, // sedans
    2, // suvs
    3, // coupes
    4, // muscle
    5, // sports classics
    6, // sports
    7, // super cars
    8, // motorcycles
    9, // off-roads
    10, // industrials
    11, // utilities
    12, // vans
    17, // service
    18, // emergency
    19, // military
    20, // commercials
]

alt.onServer('playerEnteredVehicle', (vehicle: alt.Vehicle, seat) => {
    const vehicleClass = natives.getVehicleClass(vehicle.scriptID)

    if (vehiclesWithHud.includes(vehicleClass)) {
        UiManager.showComponent('VehicleHud')
    }
})

alt.onServer('playerLeftVehicle', (vehicle, seat) => {
    UiManager.hideComponent('VehicleHud')
    UiManager.emit('VehicleHud:Reset')
})
