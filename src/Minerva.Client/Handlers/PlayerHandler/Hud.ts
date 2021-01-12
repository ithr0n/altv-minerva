import * as alt from 'alt-client'
import * as UiManager from '../../UiManager'

alt.onServer('PlayerSpawned', () => {
    UiManager.showComponent('PlayerHud')
    UiManager.emit('PlayerHud:SetData', 'playerName', alt.Player.local.getStreamSyncedMeta("roleplayName"))
    UiManager.emit('PlayerHud:SetData', 'cash', alt.Player.local.getStreamSyncedMeta("cash"))
    UiManager.emit('PlayerHud:SetData', 'hunger', alt.Player.local.getStreamSyncedMeta("hunger"))
    UiManager.emit('PlayerHud:SetData', 'thirst', alt.Player.local.getStreamSyncedMeta("thirst"))
    UiManager.emit('PlayerHud:SetData', 'voiceIndex', alt.Player.local.getStreamSyncedMeta("voiceIndex"))
})

alt.on('streamSyncedMetaChange', (entity: alt.Entity, key: string, value: string) => {
    if (!(entity instanceof alt.Player)) return
    if (entity !== alt.Player.local) return

    switch (key) {
        case 'cash': {
            UiManager.emit('PlayerHud:SetData', 'cash', value)
            break;
        }

        case 'hunger': {
            UiManager.emit('PlayerHud:SetData', 'hunger', value)
            break;
        }

        case 'thirst': {
            UiManager.emit('PlayerHud:SetData', 'thirst', value)
            break;
        }

        case 'voiceIndex': {
            UiManager.emit('PlayerHud:SetData', 'voiceIndex', value)
            break;
        }
    }
})