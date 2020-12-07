import * as alt from 'alt-client'
import * as natives from 'natives'
import { WeatherType, WeatherTypeHash } from '../Utils/NativesHelper'

let currentWeatherId: number = WeatherType['CLEAR']

alt.on('connectionComplete', () => {
    alt.setWeatherSyncActive(true)
    natives.clearOverrideWeather()
    natives.setWeatherTypeNowPersist(WeatherType[currentWeatherId])
})

alt.on('globalSyncedMetaChange', (key: string, value: any, oldValue: any) => {
    if (key === 'clockPaused') {
        natives.pauseClock(value)
        return
    }

    if (key === 'weather') {
        if (value !== oldValue) {
            changeWeather(++currentWeatherId, false)
        }

        return
    }

    if (key === 'overrideWeather') {
        if (value === null) {
            natives.clearOverrideWeather()
        } else {
            natives.setOverrideWeather(WeatherType[++value])
        }
        return
    }

    if (key === 'blackout') {
        natives.setArtificialLightsState(!!value)
        return
    }
})

alt.onServer('World:SetWeatherImmediately', () => {
    changeWeather(currentWeatherId, true)
})

const changeWeather = (weatherTypeId: number, immediately: boolean) => {
    currentWeatherId = weatherTypeId

    if (immediately) {
        natives.setWeatherTypeNowPersist(WeatherType[weatherTypeId])
        toggleWinterEffects()
    } else {
        natives.setWeatherTypePersist(WeatherType[weatherTypeId])
        alt.setTimeout(() => toggleWinterEffects, 1000 * 60 * 2)
    }
}

const toggleWinterEffects = () => {
    if (currentWeatherId === WeatherType.XMAS) {
        natives.setForceVehicleTrails(true);
        natives.setForcePedFootstepsTracks(true);
    } else {
        natives.setForceVehicleTrails(false);
        natives.setForcePedFootstepsTracks(false);
    }
}