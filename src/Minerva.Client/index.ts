import * as alt from 'alt-client'
import * as natives from 'natives'


import './Handlers/SessionHandler'
import './Handlers/ConsoleHandler'
import './Handlers/VehicleHandler'
import './Handlers/KeyHandler'
import './Handlers/PlayerHandler'
import './Handlers/WorldDataHandler'

import './Modules/CommandSystem'
import './Modules/EntitySync'
import './Modules/UiManager'

import './loadIpls'
import './teststuff'

alt.log('Resource has been loaded.')

alt.everyTick(() => {
    // disable attacks without aiming
    if (natives.isControlPressed(0, 25)) {
        natives.disableControlAction(0, 24, true)
        natives.disableControlAction(0, 140, true)
    }
})