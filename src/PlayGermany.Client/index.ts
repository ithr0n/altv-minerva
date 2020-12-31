import * as alt from 'alt-client'
import * as natives from 'natives'

import './loadIpls'

import './Handlers/SessionHandler'
import './Handlers/ConsoleHandler'
import './Handlers/VehicleHandler'
import './Handlers/KeyHandler'
import './Handlers/PedHandler'
import './Handlers/WorldDataHandler'

import './EntitySync/PropsStreamer'
import './EntitySync/MarkersStreamer'

import './UiManager'

import './teststuff'

alt.log('Resource has been loaded.')

alt.everyTick(() => {
    // disable attacks without aiming
    if (natives.isControlPressed(0, 25)) {
        natives.disableControlAction(0, 24, true)
        natives.disableControlAction(0, 140, true)
    }
})