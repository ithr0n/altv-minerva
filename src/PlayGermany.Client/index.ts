import * as alt from 'alt-client'
import * as natives from 'natives'

import './Handlers/SessionHandler'
import './EntitySync/PropsStreamer'
import './UiManager'

// todo: find better place for these things
natives.setPedConfigFlag(alt.Player.local.scriptID, 429, true); // _PED_FLAG_DISABLE_STARTING_VEH_ENGINE 
natives.setPedConfigFlag(alt.Player.local.scriptID, 184, true); // _PED_FLAG_DISABLE_SHUFFLING_TO_DRIVER_SEAT
natives.setPedConfigFlag(alt.Player.local.scriptID, 32, true); // Player_FLAG_CAN_FLY_THRU_WINDSCREEN

alt.log('Resource has been loaded.')
