import * as alt from 'alt-client';
import * as natives from 'natives';
import KeyCodes from '../Utils/KeyCodes'

const localPlayer = alt.Player.local

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
            const vehicle = localPlayer.vehicle
            if (vehicle && natives.getPedInVehicleSeat(vehicle.scriptID, -1, undefined) === localPlayer.scriptID) {
                alt.emitServer("Vehicle:ToggleIndicator", 1);
            }
            break
        }

        case KeyCodes.VK_NUMPAD5: {
            const vehicle = localPlayer.vehicle
            if (vehicle && natives.getPedInVehicleSeat(vehicle.scriptID, -1, undefined) === localPlayer.scriptID) {
                alt.emitServer("Vehicle:ToggleIndicator", 2);
            }
            break
        }

        case KeyCodes.VK_NUMPAD6: {
            const vehicle = localPlayer.vehicle
            if (vehicle && natives.getPedInVehicleSeat(vehicle.scriptID, -1, undefined) === localPlayer.scriptID) {
                alt.emitServer("Vehicle:ToggleIndicator", 0);
            }
            break
        }

        case KeyCodes.VK_BACK: {
            //alt.emit('UiManager:ToggleComponent', 'PlayerHud') // demo code
        }
    }
});