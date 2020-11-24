/* eslint-disable import/prefer-default-export */

import * as natives from 'natives'
import * as AsyncHelper from './AsyncHelper'

/**
 *
 * @param {number} x
 * @param {number} y
 * @param {number} z
 */
export async function getGroundZ(x: number, y: number, z: number): Promise<number> {
    natives.setFocusPosAndVel(x, y, z, 0, 0, 0)

    await AsyncHelper.Wait(100)

    const [, height] = natives.getGroundZFor3dCoord(x, y, 1000, undefined, undefined, undefined)

    natives.clearFocus()

    return height
}

/**
 * Display default notification
 * @param {string} text
 */
export function displayNotification(text: string) {
    natives.beginTextCommandThefeedPost('STRING')
    natives.addTextComponentSubstringPlayerName(text)
    natives.endTextCommandThefeedPostTicker(false, true)
}

/**
 * Display notification with advanced options
 * @param {string} message
 * @param {string} title
 * @param {string} subtitle
 * @param {string} notifImage
 * @param {number} iconType
 * @param {number} backgroundColor
 * @param {number} durationMult
 */
export function displayAdvancedNotification(
    message: string,
    title = 'Title',
    subtitle = 'subtitle',
    notifImage: string = null,
    iconType = 0,
    backgroundColor: number = null,
    durationMult = 1,
) {
    natives.beginTextCommandThefeedPost('STRING')
    natives.addTextComponentSubstringPlayerName(message)

    if (backgroundColor != null) {
        natives.thefeedSetNextPostBackgroundColor(backgroundColor)
    }

    if (notifImage != null) {
        natives.endTextCommandThefeedPostMessagetextTu(
            notifImage,
            notifImage,
            false,
            iconType,
            title,
            subtitle,
            durationMult,
        )
    }

    return natives.endTextCommandThefeedPostTicker(false, true)
}
