import * as alt from 'alt-client'
import * as natives from 'natives'

// force first person
natives.setFollowPedCamViewMode(4)
natives.setFollowVehicleCamViewMode(4)

alt.everyTick(() => {
    natives.disableControlAction(0, 0, true)

    if (natives.getVehiclePedIsEntering(alt.Player.local.scriptID) !== 0) {
        natives.setFollowPedCamViewMode(4);
    }
})

// disable health and armor bar
alt.beginScaleformMovieMethodMinimap('SETUP_HEALTH_ARMOUR')
natives.scaleformMovieMethodAddParamInt(3)
natives.endScaleformMovieMethod()

