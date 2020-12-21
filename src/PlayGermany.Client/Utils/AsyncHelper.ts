import * as alt from 'alt-client'
import * as natives from 'natives'

/**
 *
 * @param {string} scaleform
 * @returns {Promise<number>} handle
 */
export const RequestScaleform = async (scaleform: string, timeoutMs: number = 10000) => {
    return new Promise((resolve, reject) => {
        /* natives.doesScaleformExist(dictName) does not exists */

        /* before checking if the movie is loaded we need the handle */

        const handle = natives.requestScaleformMovie(scaleform)

        const deadline = new Date().getTime() + timeoutMs

        const inter = alt.setInterval(() => {
            if (natives.hasScaleformMovieLoaded(handle)) {
                alt.clearInterval(inter)
                //alt.log(`Scaleform loaded: ${scaleform}`)
                resolve(handle)
            } else if (deadline < new Date().getTime()) {
                alt.clearInterval(inter)
                const error = `Error: Async loading failed for scaleformMovie: ${scaleform}`
                alt.log(error)
                reject(new Error(error)) // probably better resolve(false)
            }
        }, 10)
    })
}

/**
 *
 * @param {string} dictName
 * @returns {Promise<boolean>} loaded
 */
export const RequestAnimDict = (dictName: string, timeoutMs: number = 10000) => {
    return new Promise((resolve, reject) => {
        if (!natives.doesAnimDictExist(dictName)) {
            reject(new Error(`Animation dictionary does not exist: ${dictName}`))
            return
        }

        if (natives.hasAnimDictLoaded(dictName)) {
            resolve(true)
            return
        }

        natives.requestAnimDict(dictName)

        const deadline = new Date().getTime() + timeoutMs

        const inter = alt.setInterval(() => {
            if (natives.hasAnimDictLoaded(dictName)) {
                alt.clearInterval(inter)
                //alt.log(`Animation dictionary loaded: ${dictName}`)
                resolve(true)
            } else if (deadline < new Date().getTime()) {
                alt.clearInterval(inter)
                const error = `Error: Async loading failed for animDict: ${dictName}`
                alt.log(error)
                reject(new Error(error)) // probably better resolve(false)
            }
        }, 50)
    })
}

/**
 *
 * @param {number} model
 * @returns {Promise<boolean>} loaded
 */
export const RequestModel = async (model: number, timeoutMs: number = 10000) => {
    return new Promise((resolve, reject) => {
        if (!natives.isModelValid(model)) {
            reject(new Error(`Model does not exist: ${model}`))
            return
        }

        if (natives.hasModelLoaded(model)) {
            resolve(true)
            return
        }

        natives.requestModel(model)

        const deadline = new Date().getTime() + timeoutMs

        const inter = alt.setInterval(() => {
            if (natives.hasModelLoaded(model)) {
                alt.clearInterval(inter)
                //alt.log(`Model loaded: ${model}`)
                resolve(true)
            } else if (deadline < new Date().getTime()) {
                alt.clearInterval(inter)
                const error = `Error: Async loading failed for model: ${model}`
                alt.log(error)
                reject(new Error(error)) // probably better resolve(false)
            }
        }, 10)
    })
}

export const RequestNamedPtfxAsset = (assetName: string, timeoutMs: number = 10000) => {
    return new Promise((resolve, reject) => {
        if (natives.hasNamedPtfxAssetLoaded(assetName)) {
            return resolve(true);
        }

        natives.requestNamedPtfxAsset(assetName);

        const deadline = new Date().getTime() + timeoutMs

        let inter = alt.setInterval(() => {
            if (natives.hasNamedPtfxAssetLoaded(assetName)) {
                alt.clearInterval(inter);
                //alt.log('Asset loaded: ' + assetName);
                return resolve(true);
            } else if (deadline < new Date().getTime()) {
                alt.clearInterval(inter)
                const error = `Error: Async loading failed for namedPtfxAsset: ${assetName}`
                alt.log(error)
                reject(new Error(error)) // probably better resolve(false)
            }
        }, 10);
    });
}

export const Wait = (timeout: number) => {
    return new Promise(resolve => {
        alt.setTimeout(() => {
            resolve(true)
        }, timeout);
    })
}

export const WaitUntil = (action: (...args: any[]) => boolean, timeoutMs: number = 10000, ...args: any[]) => {
    return new Promise((resolve, reject) => {
        const deadline = new Date().getTime() + timeoutMs

        const et = alt.everyTick(() => {
          if (!action(...args)) {
            if (deadline < new Date().getTime()) {
                alt.clearEveryTick(et)
                const error = `Error: Async waiting for callback failed!!!`
                alt.log(error)
                reject(new Error(error)) // probably better resolve(false)
            }

            return
          };
          
          alt.clearEveryTick(et);
          resolve(true);
        });
      });
}