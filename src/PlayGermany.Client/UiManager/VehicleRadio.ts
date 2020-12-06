import * as alt from 'alt-client'
import * as natives from 'natives'
import KeyCodes from '../Utils/KeyCodes'

const player = alt.Player.local;
let focused = false

let radioStations = [
    {
        name: 'Play-Germany Radio',
        url: 'http://gtaradio.play-germany.de:8000/listen.pls',
        image: 'playgermany.png'
    },
    {
        name: '011.FM - Country Roads',
        url: 'http://listen.011fm.com:8016/stream09',
        image: '011-fm.png'
    },
    {
        name: 'Insanity Radio 103.2FM',
        url: 'https://stream.cor.insanityradio.com/insanity320.mp3',
        image: 'insanity.png'
    },
    {
        name: 'B4B Disco Funk',
        url: 'https://radio9.pro-fhi.net/radio/9164/stream',
        image: 'b4b.png'
    },
    {
        name: 'One Love Hip Hop Radio',
        url: 'http://listen.one.hiphop/live.mp3',
        image: 'onelove.png',
        volume: 60
    },
    {
        name: 'ADR.FM',
        url: 'http://192.111.140.6:9683/stream',
        image: 'adr.png'
    }
]

alt.on('UiManager:Loaded', () => {
    alt.emit('UiManager:Emit', 'radio:SetStations', radioStations)
})

alt.onServer('playerEnteredVehicle', (vehicle: alt.Vehicle, seat: number) => {
    let currentStation = 0
    if (player.vehicle.hasStreamSyncedMeta('radioStation')) {
        currentStation = player.vehicle.getStreamSyncedMeta('radioStation')
    }

    alt.emit('UiManager:SetAppData', 'isPlayerInVehicle', true)
    alt.emit('UiManager:Emit', 'radio:SwitchStation', currentStation);
});

alt.onServer('playerLeftVehicle', (vehicle: alt.Vehicle, seat: number) => {
    alt.emit('UiManager:SetAppData', 'isPlayerInVehicle', false)
})

alt.on('streamSyncedMetaChange', (entity: alt.Entity, key: string, value: string) => {
    if (!(entity instanceof alt.Vehicle)) {
        return
    }

    if (key === 'radioStation') {
        if (player.vehicle && player.hasMeta('seat') && player.getMeta('seat') !== -1) {
            alt.emit('UiManager:Emit', 'radio:SwitchStation', value)
        } else {
            // later: we can play radio of nearby cars here
        }
    }
})
// alt.on('syncedMetaChange', (entity, key, value) => {
//     if (entity != player.vehicle) return;
//     const pedInSeat = native.getPedInVehicleSeat(player.vehicle.scriptID, -1);
//     if (key != 'radioStation') return;
//     if (pedInSeat == player.scriptID) return;

//     let radioStation = value;

//     if (browser && mounted) {
//         browser.emit('switchRadio', radioStation);
//     }
// });

alt.everyTick(() => {
    if (player.vehicle) {
        natives.disableControlAction(0, 85, true);
    } else {
        natives.enableControlAction(0, 85, true);
    }

    if (focused) {
        natives.disableControlAction(0, 99, true);
        natives.disableControlAction(0, 100, true);
    } else {
        natives.enableControlAction(0, 99, true);
        natives.enableControlAction(0, 100, true);
    }

    if (natives.isPedSittingInAnyVehicle(player.scriptID)) {
        natives.setRadioToStationName('OFF');
    }
});

alt.on('keydown', key => {
    if (!player.vehicle) {
        return
    }

    if (key === KeyCodes.Q_KEY && player.hasMeta('seat')) {
        if ([-1, 0].includes(+player.getMeta('seat'))) {
            // driver or first passenger
            focused = true
        }
    }
});

alt.on('keyup', key => {
    if (!player.vehicle) {
        return
    }

    if (key === KeyCodes.Q_KEY && player.hasMeta('seat')) {
        if ([-1, 0].includes(+player.getMeta('seat'))) {
            focused = false
        }
    }
});