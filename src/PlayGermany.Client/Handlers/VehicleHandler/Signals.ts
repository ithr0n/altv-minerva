import * as alt from 'alt-client'
import * as natives from 'natives'

const HASH_TOWTRUCK = alt.hash('towtruck');

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
