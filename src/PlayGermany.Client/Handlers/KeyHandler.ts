import * as alt from 'alt-client';
import * as natives from 'natives';
import KeyCodes from '../Utils/KeyCodes'

alt.on('keyup', (key: KeyCodes) => {
    if (!alt.gameControlsEnabled()) {
        return;
    }

    switch (key) {
        case KeyCodes.VK_F3:
            alt.emitServer('Voice:SwitchVoiceLevel');
            break;

        case KeyCodes.VK_ADD: {
            let vehicle = alt.Player.local.vehicle

            if (vehicle) {
                let isEngineRunning = natives.getIsVehicleEngineRunning(vehicle.scriptID)

                if (isEngineRunning) {
                    natives.setVehicleEngineOn(vehicle.scriptID, false, false, true)
                } else {
                    natives.setVehicleEngineOn(vehicle.scriptID, true, false, true)
                }
            }
        }
    }
});