import * as alt from 'alt-client'
import * as natives from 'natives'
import * as NativesHelper from '../Utils/NativesHelper'
import Notifications from '../Modules/UiManager/Notifications'

alt.on('consoleCommand', (command: string, ...args: string[]) => {
    alt.emitServer('ClientConsoleHandler:Command', command, args)
})

alt.onServer('ConsoleHandler:TeleportToWaypoint', async () => {
    const waypoint = natives.getFirstBlipInfoId(8)

    if (natives.doesBlipExist(waypoint)) {
        const coords = natives.getBlipInfoIdCoord(waypoint)
        const z = await NativesHelper.getGroundZ(coords.x, coords.y, coords.z)

        alt.emitServer('RequestTeleport', new alt.Vector3(coords.x, coords.y, z))
    } else {
        Notifications.error('Du musst zuerst einen Marker setzen!')
    }
})