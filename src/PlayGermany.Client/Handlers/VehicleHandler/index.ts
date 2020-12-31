import * as alt from 'alt-client'
import * as natives from 'natives'

import './EnginePowerModification'
import './Hud'
import './Radio'
import './Signals'
import './Teleportation'

alt.onServer('playerEnteredVehicle', (vehicle: alt.Vehicle, seat: number) => {
    alt.Player.local.setMeta('seat', seat)

     // apply flags everytime (because they can change)
    natives.setPedConfigFlag(alt.Player.local.scriptID, 241, true) // PED_FLAG_DISABLE_STOPPING_VEH_ENGINE
    natives.setPedConfigFlag(alt.Player.local.scriptID, 429, true) // PED_FLAG_DISABLE_STARTING_VEH_ENGINE
    natives.setPedConfigFlag(alt.Player.local.scriptID, 184, true) // PED_FLAG_DISABLE_SHUFFLING_TO_DRIVER_SEAT
    natives.setPedConfigFlag(alt.Player.local.scriptID, 32, true) // Player_FLAG_CAN_FLY_THRU_WINDSCREEN
})

alt.onServer('playerLeftVehicle', (vehicle: alt.Vehicle, seat: number) => {
    alt.Player.local.deleteMeta('seat')
})

alt.onServer('playerChangedVehicleSeat', (vehicle: alt.Vehicle, oldSeat: number, newSeat: number) => {
    alt.Player.local.setMeta('seat', newSeat)
})