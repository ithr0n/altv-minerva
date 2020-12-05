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

export enum WeatherTypeHash {
    ExtraSunny = 2544503417,
    Clear = 916995460,
    Clouds = 821931868,
    Smog = 282916021,
    Foggy = 2926802500,
    Overcast = 3146353965,
    Rain = 1420204096,
    Thunder = 3061285535,
    Clearing = 1840358669,
    Neutral = 2764706598,
    Snow = 4021743606,
    Blizzard = 669657108,
    Snowlight = 603685163,
    Xmas = 2865350805,
    Halloween = 3373937154
}

export enum WeatherType {
    ExtraSunny = 0,
    Clear = 1,
    Clouds = 2,
    Smog = 3,
    Foggy = 4,
    Overcast = 5,
    Rain = 6,
    Thunder = 7,
    Clearing = 8,
    Neutral = 9,
    Snow = 10,
    Blizzard = 11,
    Snowlight = 12,
    Xmas = 13,
    Halloween = 14
}