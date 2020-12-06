import * as alt from 'alt-client'
import * as natives from 'natives'

// force first person
natives.setFollowPedCamViewMode(4)
natives.setFollowVehicleCamViewMode(4)

alt.everyTick(() => {
    //natives.disableControlAction(0, 0, true)

    if (natives.getVehiclePedIsEntering(alt.Player.local.scriptID) !== 0) {
        //natives.setFollowPedCamViewMode(4);
    }
})

alt.on('gameEntityCreate', (entity: alt.Entity) => {
    if (entity instanceof alt.Player) {
        if (!entity.getSyncedMeta("isDead")) {
            // clear bloody peds after respawn
            natives.clearPedBloodDamage(entity.scriptID)
        }
    }
})

alt.on('syncedMetaChange', (entity: alt.Entity, key: string, value: any) => {
    if (entity instanceof alt.Player && key === 'isDead' && !value) {
        // clear bloody peds after respawn
        natives.clearPedBloodDamage(entity.scriptID)
    }
})
