import * as alt from 'alt-client'

import './EnginePowerModification'
import './Radio'
import './Signals'
import './Teleportation'

alt.onServer('playerEnteredVehicle', (vehicle: alt.Vehicle, seat: number) => {
    alt.Player.local.setMeta('seat', seat)
})

alt.onServer('playerLeftVehicle', (vehicle: alt.Vehicle, seat: number) => {
    alt.Player.local.deleteMeta('seat')
})

alt.onServer('playerChangedVehicleSeat', (vehicle: alt.Vehicle, oldSeat: number, newSeat: number) => {
    alt.Player.local.setMeta('seat', newSeat)
})