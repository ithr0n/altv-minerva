import * as alt from 'alt-client'
import * as natives from 'natives'
import * as AsyncHelper from '../Utils/AsyncHelper'

const playFieldCoord = { x: -1212.79, y: -1673.52, z: 7 }
const airportCoord = { x: -1466.79, y: -2507.52, z: 0 }

// https://github.com/Sainan/GTA-V-Decompiled-Scripts/blob/43ed1f703af78ffb4e0263a88b9b9655f64d49c2/decompiled_scripts/fm_intro_cut_dev.c

export const Execute = async () => {
    const player = alt.Player.local
    const hashMaleModel = alt.hash('mp_m_freemode_01')
    const hashFemaleModel = alt.hash('mp_f_freemode_01')
    const isFemale = player.model === hashFemaleModel

    natives.doScreenFadeIn(1000)

    //const plane = natives.createVehicle(alt.hash('jet'), -1200, -1490, 142.385, 0, false, false, false)

    natives.prepareMusicEvent('GLOBAL_KILL_MUSIC')
    natives.prepareMusicEvent('FM_INTRO_START')

    natives.triggerMusicEvent('GLOBAL_KILL_MUSIC')
    natives.triggerMusicEvent('FM_INTRO_START')

    natives.requestCutsceneWithPlaybackList('mp_intro_concat', isFemale ? 103 : 31, 8); // 103 = female, 31 = male 
    // natives.setCutsceneEntityStreamingFlags('MP_Male_Character', 0, 1);

    // natives.loadCloudHat('CONTRAILS', 0); // Alt:V doesn't like this native

    // wait for important things
    //natives.requestPtfxAsset()
    //await AsyncHelper.WaitUntil(natives.hasPtfxAssetLoaded)
    await AsyncHelper.WaitUntil(natives.canRequestAssetsForCutsceneEntity)

    // load plane
    //natives.setEntityLodDist(plane, 3000)
    //natives.setEntitySomething(plane, false)

    //const particle1 = natives.startParticleFxLoopedOnEntity('scr_mp_intro_plane_exhaust', plane, -5.403, -8, -2.2, 0, 0, 0, 1, false, false, false)
    //const particle2 = natives.startParticleFxLoopedOnEntity('scr_mp_intro_plane_exhaust', plane, 6.629, -8, -2.2, 0, 0, 0, 1, false, false, false)

    //natives.registerEntityForCutscene(plane, 'MP_Plane', 0, 0, 0)

    // disable + unload other gender
    if (isFemale) {
        natives.setCutsceneEntityStreamingFlags("MP_Male_Character", 0, 1)
        natives.registerEntityForCutscene(0, "MP_Male_Character", 3, hashMaleModel, 0)
    } else {
        natives.setCutsceneEntityStreamingFlags("MP_Female_Character", 0, 1)
        natives.registerEntityForCutscene(0, "MP_Female_Character", 3, hashFemaleModel, 0)
    }

    // load character
    natives.registerEntityForCutscene(player.scriptID, isFemale ? "MP_Female_Character" : "MP_Male_Character", 0, 0, 0)

    // Unload other characters
    for (let i = 0; i <= 7; i++) {
        natives.setCutsceneEntityStreamingFlags("MP_Plane_Passenger_" + i, 0, 1);
        natives.registerEntityForCutscene(0, 'MP_Plane_Passenger_' + i, 3, hashFemaleModel, 0);
        natives.registerEntityForCutscene(player.scriptID, 'MP_Plane_Passenger_' + i, 3, hashMaleModel, 0);
    }

    // Make sure our cutscene looks nice
    natives.newLoadSceneStartSphere(playFieldCoord.x, playFieldCoord.y, playFieldCoord.z, 1000, 0);

    //natives.setOverrideWeather("EXTRASUNNY")
    natives.startCutscene(4)


    await AsyncHelper.Wait(22_000)

    // Make sure our cutscene looks nice
    natives.newLoadSceneStartSphere(airportCoord.x, airportCoord.y, airportCoord.z, 1000, 0)

    await AsyncHelper.Wait(1_100)

    //natives.startParticleFxNonLoopedOnEntity('scr_mp_plane_landing_tyre_smoke', plane, -2.508, -3.666, -3.584, 0, 0, -90, 1, false, false, false)
    //natives.startParticleFxNonLoopedOnEntity('scr_mp_plane_landing_tyre_smoke', plane, 3.508, -3.666, -3.584, 0, 0, -90, 1, false, false, false)

    await AsyncHelper.Wait(7_700)

    // Cleanup and stop cutscene after it's finished
    natives.doScreenFadeOut(1_000)

    await AsyncHelper.Wait(2_000)

    natives.stopCutsceneImmediately()
    natives.triggerMusicEvent("GLOBAL_KILL_MUSIC")

    await AsyncHelper.Wait(2_000)

    natives.doScreenFadeIn(1_000)

    natives.newLoadSceneStop()

    //natives.deleteVehicle(plane)
    //if (natives.doesParticleFxLoopedExist(particle1)) {
    //    natives.removeParticleFx(particle1, false)
    //}
    //if (natives.doesParticleFxLoopedExist(particle2)) {
    //    natives.removeParticleFx(particle2, false)
    //}
    //if (natives.hasPtfxAssetLoaded()) {
    //    natives.removePtfxAsset()
    //}
}
