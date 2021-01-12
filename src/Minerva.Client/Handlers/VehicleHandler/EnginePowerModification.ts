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