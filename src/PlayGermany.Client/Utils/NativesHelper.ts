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
    EXTRASUNNY = 2544503417,
    CLEAR = 916995460,
    CLOUDS = 821931868,
    SMOG = 282916021,
    FOGGY = 2926802500,
    OVERCAST = 3146353965,
    RAIN = 1420204096,
    THUNDER = 3061285535,
    CLEARING = 1840358669,
    NEUTRAL = 2764706598,
    SNOW = 4021743606,
    BLIZZARD = 669657108,
    SNOWLIGHT = 603685163,
    XMAS = 2865350805,
    HALLOWEEN = 3373937154
}

export enum WeatherType {
    ExtraSunny = 0,
    CLEAR = 1,
    CLOUDS = 2,
    SMOG = 3,
    FOGGY = 4,
    OVERCAST = 5,
    RAIN = 6,
    THUNDER = 7,
    CLEARING = 8,
    NEUTRAL = 9,
    SNOW = 10,
    BLIZZARD = 11,
    SNOWLIGHT = 12,
    XMAS = 13,
    HALLOWEEN = 14
}