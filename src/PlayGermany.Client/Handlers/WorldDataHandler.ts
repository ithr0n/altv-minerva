import * as alt from 'alt-client'
import * as natives from 'natives'
import { WeatherType, WeatherTypeHash } from '../Utils/NativesHelper'
import { random } from '../Utils/AltHelper'

let weatherTransitionInterval: number = -1

alt.on('connectionComplete', () => {
    alt.setWeatherSyncActive(false)
    natives.clearOverrideWeather()
    natives.clearWeatherTypePersist()
    natives.setWeatherTypeNow('CLEAR')
})

alt.on('globalSyncedMetaChange', (key: string, value: any, oldValue: any) => {
    if (key === 'clockPaused') {
        natives.pauseClock(value)
        return
    }

    if (key === 'weather') {
        if (value !== oldValue) {
            const oldWeatherHash : number = (<any>WeatherTypeHash)[WeatherType[oldValue]]
            const newWeatherHash : number = (<any>WeatherTypeHash)[WeatherType[value]]
            
            if (typeof (oldWeatherHash) !== 'undefined' && typeof (newWeatherHash) !== 'undefined') {
                if (weatherTransitionInterval >= 0) {
                    // transition still running, stop the old
                    alt.clearInterval(weatherTransitionInterval)
                }

                let i = 0;
                weatherTransitionInterval = alt.setInterval(() => {
                    if (++i < 100) {
                        natives.setWeatherTypeTransition(oldWeatherHash, newWeatherHash, (i / 100));
                    } else {
                        alt.clearInterval(weatherTransitionInterval)
                        weatherTransitionInterval = -1
                    }
                }, random(15000, 35000) / 100)

                if (value === WeatherType.Xmas) {
                    natives.setForceVehicleTrails(true);
                    natives.setForcePedFootstepsTracks(true);
                } else {
                    natives.setForceVehicleTrails(false);
                    natives.setForcePedFootstepsTracks(false);
                }
            }
        }

        return
    }

    if (key === 'blackout') {
        natives.setArtificialLightsState(!!value)
    }
})
