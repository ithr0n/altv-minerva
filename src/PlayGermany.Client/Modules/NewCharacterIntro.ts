import * as alt from 'alt-client'
import * as natives from 'natives'
import * as AsyncHelper from '../Utils/AsyncHelper'

const playFieldCoord = { x: -1212.79, y: -1673.52, z: 7 }
const airportCoord = { x: -1466.79, y: -2507.52, z: 0 }

export const Execute = async () => {
    const player = alt.Player.local
    const hashMaleModel = alt.hash('mp_m_freemode_01')
    const hashFemaleModel = alt.hash('mp_f_freemode_01')
    const isFemale = player.model === hashFemaleModel

    natives.prepareMusicEvent('GLOBAL_KILL_MUSIC')
    natives.prepareMusicEvent('FM_INTRO_START')

    natives.triggerMusicEvent('GLOBAL_KILL_MUSIC')
    natives.triggerMusicEvent('FM_INTRO_START')

    natives.requestCutsceneWithPlaybackList('mp_intro_concat', isFemale ? 103 : 31, 8); // 103 = female, 31 = male 
    natives.setCutsceneEntityStreamingFlags('MP_Male_Character', 0, 1);

    // native.loadCloudHat('CONTRAILS', 0); // Alt:V doesn't like this native

    // wait so that we can (un)register entities
    await AsyncHelper.WaitUntil(natives.canRequestAssetsForCutsceneEntity)

    // disable + unload other gender
    if (isFemale) {
        natives.setCutsceneEntityStreamingFlags("MP_Male_Character", 0, 1)
        natives.registerEntityForCutscene(0, "MP_Male_Character", 3, hashMaleModel, 0)
    } else {
        natives.setCutsceneEntityStreamingFlags("MP_Female_Character", 0, 1)
        natives.registerEntityForCutscene(0, "MP_Female_Character", 3, hashFemaleModel, 0)
    }

    // Unload other characters
    for (let i = 0; i <= 7; i++) {
        natives.setCutsceneEntityStreamingFlags("MP_Plane_Passenger_" + i, 0, 1);
        natives.registerEntityForCutscene(0, 'MP_Plane_Passenger_' + i, 3, hashFemaleModel, 0);
        natives.registerEntityForCutscene(0, 'MP_Plane_Passenger_' + i, 3, hashMaleModel, 0);
    }

    // Make sure our cutscene looks nice
    natives.newLoadSceneStartSphere(playFieldCoord.x, playFieldCoord.y, playFieldCoord.z, 1000, 0);

    //natives.setOverrideWeather("EXTRASUNNY")
    natives.startCutscene(4)

    // load gender character
    natives.registerEntityForCutscene(player.scriptID, isFemale ? "MP_Female_Character" : "MP_Male_Character", 0, 0, 0)

    await AsyncHelper.Wait(22_000)

    // Make sure our cutscene looks nice
    natives.newLoadSceneStartSphere(airportCoord.x, airportCoord.y, airportCoord.z, 1000, 0)
    await AsyncHelper.Wait(8_800)

    // Cleanup and stop cutscene after it's finished
    natives.doScreenFadeOut(1_000)
    await AsyncHelper.Wait(2_000)
    natives.stopCutsceneImmediately()
    natives.triggerMusicEvent("GLOBAL_KILL_MUSIC")
    await AsyncHelper.Wait(2_000)
    natives.doScreenFadeIn(1_000)

    natives.newLoadSceneStop()
}
