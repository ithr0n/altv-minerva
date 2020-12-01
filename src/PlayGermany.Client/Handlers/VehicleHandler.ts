import * as alt from 'alt-client'
import * as natives from 'natives'

alt.onServer('VehicleHandler:TeleportInto', (vehicle: alt.Vehicle, seat: number) => {
    let cleared = false;

    const interval = alt.setInterval(() => {
        const vehicleScriptId = vehicle.scriptID;
        if (vehicleScriptId) {
            natives.setPedIntoVehicle(alt.Player.local.scriptID, vehicleScriptId, seat);
            alt.clearInterval(interval);
            cleared = true;
        }
    }, 10);

    alt.setTimeout(() => {
        if (!cleared) {
            alt.clearInterval(interval);
        }
    }, 5000);
});

alt.everyTick(() => {
    let veh = alt.Player.local.vehicle;
    if (veh && veh.valid) {
        let power = veh.getStreamSyncedMeta("EnginePowerMultiplier");
        if (!isNaN(+power)) {
            natives.setVehicleCheatPowerIncrease(veh.scriptID, power);
        }
    }
})

alt.on('streamSyncedMetaChange', (entity: alt.Entity, key: string, value: string) => {
    if (!(entity instanceof alt.Vehicle)) {
        return
    }

    const valueNumber = Number(value)
    if (key === 'indicators' && !isNaN(valueNumber)) {
        natives.setVehicleIndicatorLights(entity.scriptID, 1, (valueNumber >= 2))
        natives.setVehicleIndicatorLights(entity.scriptID, 0, (valueNumber === 3 || valueNumber === 1))
    }
})
