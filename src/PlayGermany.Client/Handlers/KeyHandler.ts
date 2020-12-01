import * as alt from 'alt-client';
import * as natives from 'natives';
import KeyCodes from '../Utils/KeyCodes'

const localPlayer = alt.Player.local

const vehicleClassesWithIndicators: number[] = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 17, 18, 19, 20]

alt.on('keyup', (key: KeyCodes) => {
    if (!alt.gameControlsEnabled()) {
        return
    }

    switch (key) {
        case KeyCodes.VK_F3:
            alt.emitServer('Voice:SwitchVoiceLevel')
            break

        case KeyCodes.VK_ADD: {
            const vehicle = localPlayer.vehicle

            if (vehicle) {
                let isEngineRunning = natives.getIsVehicleEngineRunning(vehicle.scriptID)

                if (isEngineRunning) {
                    natives.setVehicleEngineOn(vehicle.scriptID, false, false, true)
                } else {
                    natives.setVehicleEngineOn(vehicle.scriptID, true, false, true)
                }
            }
        }

        case KeyCodes.VK_NUMPAD4: {
            toggleIndicator(2)
            break
        }

        case KeyCodes.VK_NUMPAD5: {
            toggleIndicator(3)
            break
        }

        case KeyCodes.VK_NUMPAD6: {
            toggleIndicator(1)
            break
        }

        case KeyCodes.VK_BACK: {
            //alt.emit('UiManager:ToggleComponent', 'PlayerHud') // demo code
        }
    }
});

const toggleIndicator = (index: number) => {
    const vehicle = localPlayer.vehicle
    const vehicleClass = natives.getVehicleClass(vehicle.scriptID)
    if (vehicleClassesWithIndicators.includes(vehicleClass)) {
        if (vehicle && natives.getPedInVehicleSeat(vehicle.scriptID, -1, undefined) === localPlayer.scriptID) {
            alt.emitServer("Vehicle:ToggleIndicator", index);
        }
    }
}