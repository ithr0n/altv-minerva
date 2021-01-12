import * as alt from 'alt-client'
import * as natives from 'natives'
import { AnyScriptCameraRendering } from '../../Modules/Camera'


alt.everyTick(() => {
    if (AnyScriptCameraRendering()) {
        return
    }

    // disable camera switch (V button)
    natives.disableControlAction(0, 0, true)

    // set to first person again
    if (!alt.Player.local.vehicle && natives.getFollowPedCamViewMode() !== 4) {
        natives.setFollowPedCamViewMode(4)
    }

    // set to first person in vehicle again
    if (alt.Player.local.vehicle && natives.getFollowVehicleCamViewMode() !== 4) {
        natives.setFollowVehicleCamViewMode(4)
    }

    // stay in first person when entering vehicles
    if (natives.getVehiclePedIsEntering(alt.Player.local.scriptID) !== 0) {
        natives.setFollowPedCamViewMode(4)
    }
})

alt.on('connectionComplete', () => {
    // force first person
    natives.setFollowPedCamViewMode(4)
    natives.setFollowVehicleCamViewMode(4)

    // disable health and armor bar
    alt.beginScaleformMovieMethodMinimap('SETUP_HEALTH_ARMOUR')
    natives.scaleformMovieMethodAddParamInt(3)
    natives.endScaleformMovieMethod()
})
