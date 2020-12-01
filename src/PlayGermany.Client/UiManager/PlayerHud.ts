import * as alt from 'alt-client'

alt.onServer('PlayerSpawned', () => {
    alt.emit('UiManager:ShowComponent', 'PlayerHud')
    alt.emit('UiManager:Emit', 'PlayerHud:SetData', 'playerName', alt.Player.local.getStreamSyncedMeta("roleplayName"))
    alt.emit('UiManager:Emit', 'PlayerHud:SetData', 'cash', alt.Player.local.getStreamSyncedMeta("cash"))
    alt.emit('UiManager:Emit', 'PlayerHud:SetData', 'hunger', alt.Player.local.getStreamSyncedMeta("hunger"))
    alt.emit('UiManager:Emit', 'PlayerHud:SetData', 'thirst', alt.Player.local.getStreamSyncedMeta("thirst"))
    alt.emit('UiManager:Emit', 'PlayerHud:SetData', 'voiceIndex', alt.Player.local.getStreamSyncedMeta("voiceIndex"))
})

alt.on('streamSyncedMetaChange', (entity, key, value) => {
    if (entity !== alt.Player.local) {
        return
    }

    switch (key) {
        case 'cash': {
            alt.emit('UiManager:Emit', 'PlayerHud:SetData', 'cash', value)
            break;
        }

        case 'hunger': {
            alt.emit('UiManager:Emit', 'PlayerHud:SetData', 'hunger', value)
            break;
        }

        case 'thirst': {
            alt.emit('UiManager:Emit', 'PlayerHud:SetData', 'thirst', value)
            break;
        }

        case 'voiceIndex': {
            alt.emit('UiManager:Emit', 'PlayerHud:SetData', 'voiceIndex', value)
            break;
        }
    }
})