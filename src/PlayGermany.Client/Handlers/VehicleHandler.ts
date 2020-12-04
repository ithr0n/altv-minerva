import * as alt from 'alt-client'
import * as natives from 'natives'

// speedhack
alt.everyTick(() => {
    let veh = alt.Player.local.vehicle
    if (veh && veh.valid) {
        let power = veh.getStreamSyncedMeta("EnginePowerMultiplier")
        if (!isNaN(+power)) {
            natives.setVehicleCheatPowerIncrease(veh.scriptID, power)
        }
    }
})

// teleport into vehicle
alt.onServer('VehicleHandler:TeleportInto', (vehicle: alt.Vehicle, seat: number) => {
    let cleared = false

    const interval = alt.setInterval(() => {
        const vehicleScriptId = vehicle.scriptID
        if (vehicleScriptId) {
            natives.setPedIntoVehicle(alt.Player.local.scriptID, vehicleScriptId, seat)
            alt.clearInterval(interval)
            cleared = true
        }
    }, 10)

    alt.setTimeout(() => {
        if (!cleared) {
            alt.clearInterval(interval)
        }
    }, 5000)
})

alt.on('streamSyncedMetaChange', (entity: alt.Entity, key: string, value: string) => {
    if (!(entity instanceof alt.Vehicle)) {
        return
    }

    if (key === 'indicators' && !isNaN(+value)) {
        natives.setVehicleIndicatorLights(entity.scriptID, 1, (+value >= 2))
        natives.setVehicleIndicatorLights(entity.scriptID, 0, (+value === 3 || +value === 1))
        return
    }

    if (key === 'sirenDisabled') {
        natives.setVehicleHasMutedSirens(entity.scriptID, !!value)
        return
    }
})

const HASH_TOWTRUCK = alt.hash('towtruck');
alt.on('gameEntityCreate', (entity: alt.Entity) => {
    if (!(entity instanceof alt.Vehicle)) {
        return
    }

    if (entity.hasStreamSyncedMeta('indicators')) {
        const value = entity.getStreamSyncedMeta('indicators')
        natives.setVehicleIndicatorLights(entity.scriptID, 1, (+value >= 2))
        natives.setVehicleIndicatorLights(entity.scriptID, 0, (+value === 3 || +value === 1))
    }

    if (entity.model === HASH_TOWTRUCK) {
        // get rid of old bug: tow trucks having sirens if streamed in
        natives.setVehicleHasMutedSirens(entity.scriptID, true)
    } else if (entity.hasStreamSyncedMeta('sirenDisabled'))
        natives.setVehicleHasMutedSirens(entity.scriptID, !!entity.getStreamSyncedMeta('sirenDisabled'))
});

// store seat index
alt.onServer('playerEnteredVehicle', (vehicle: alt.Vehicle, seat: number) => {
    alt.Player.local.setMeta('seat', seat)
})
alt.onServer('playerLeftVehicle', (vehicle: alt.Vehicle, seat: number) => {
    alt.Player.local.deleteMeta('seat')
})
alt.onServer('playerChangedVehicleSeat', (vehicle: alt.Vehicle, oldSeat: number, newSeat: number) => {
    alt.Player.local.setMeta('seat', newSeat)
})