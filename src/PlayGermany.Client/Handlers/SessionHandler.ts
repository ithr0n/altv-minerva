import * as alt from 'alt-client'
import * as natives from 'natives'
import * as AsyncHelper from '../Utils/AsyncHelper'
import Camera from '../Utils/Camera'

let loginCamera: Camera

alt.on('connectionComplete', async () => {
    /*await*/ AsyncHelper.RequestModel(alt.hash('mp_f_freemode_01'))
    /*await*/ AsyncHelper.RequestModel(alt.hash('mp_m_freemode_01'))

    loginCamera = new Camera(new alt.Vector3(-637.12085, 4433.934, 26.870361), new alt.Vector3(0, 0, 271.66), 20)
    loginCamera.render()

    natives.setEntityAlpha(alt.Player.local.scriptID, 0, false)
    natives.setEntityInvincible(alt.Player.local.scriptID, true)
    natives.freezeEntityPosition(alt.Player.local.scriptID, true)
    natives.displayRadar(false)
})

alt.onServer('SessionHandler:PlayerSpawned', () => {
    loginCamera.destroy()

    natives.setEntityAlpha(alt.Player.local.scriptID, 255, false)
    natives.setEntityInvincible(alt.Player.local.scriptID, false)
    natives.freezeEntityPosition(alt.Player.local.scriptID, false)
    natives.displayRadar(true)

    natives.setPedConfigFlag(alt.Player.local.scriptID, 241, true) // PED_FLAG_DISABLE_STOPPING_VEH_ENGINE
    natives.setPedConfigFlag(alt.Player.local.scriptID, 429, true) // PED_FLAG_DISABLE_STARTING_VEH_ENGINE
    natives.setPedConfigFlag(alt.Player.local.scriptID, 184, true) // PED_FLAG_DISABLE_SHUFFLING_TO_DRIVER_SEAT
    natives.setPedConfigFlag(alt.Player.local.scriptID, 32, true) // Player_FLAG_CAN_FLY_THRU_WINDSCREEN
})