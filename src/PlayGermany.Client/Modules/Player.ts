import * as alt from 'alt-client'
import * as natives from 'natives'
import { SunglassesMale } from '../Data/Clothes'

export namespace PlayerAppearance {
    //
    // Hat
    //
    export const getHat = () => getProp(0)
    export const setHat = (drawableId: number, textureId: number) => setProp(0, drawableId, textureId)

    //
    // Glasses
    //
    export const getGlasses = () => getProp(1)
    export const setGlasses = (drawableId: number, textureId: number) => {
        setProp(1, drawableId, textureId)

        if (SunglassesMale.includes(drawableId)) {
            natives.setTimecycleModifier('sunglasses')
        }
    }

    //
    // Earrings
    //
    export const getEarrings = () => getProp(2)
    export const setEarrings = (drawableId: number, textureId: number) => setProp(2, drawableId, textureId)

    //
    // Watch
    //
    export const getWatch = () => getProp(3)
    export const setWatch = (drawableId: number, textureId: number) => setProp(3, drawableId, textureId)

    export const teleportIntoVehicle = (target: alt.Vehicle) => {

    }

    export const attachObject = () => { }

    // private methods
    const getProp = (propIndex: number) => {
        const drawableId = natives.getPedPropIndex(alt.Player.local.scriptID, propIndex)
        const textureId = natives.getPedPropTextureIndex(alt.Player.local.scriptID, propIndex)

        return [drawableId, textureId]
    }

    const setProp = (propIndex: number, drawableId: number, textureId: number) => {
        const scriptId = alt.Player.local.scriptID

        natives.clearPedProp(scriptId, propIndex)

        if (drawableId) {
            if (!textureId) {
                textureId = 0
            }

            natives.setPedPropIndex(scriptId, propIndex, drawableId, textureId, true)
        }
    }
}

alt.on('ClientPlayer:Clothes:Set', () => {


})