import * as alt from 'alt-client'

alt.onServer('PlayerSpawned', () => {
    alt.emit('UiManager:ShowComponent', 'PlayerHud')
    alt.emit('UiManager:Emit', 'PlayerHud:SetName', alt.Player.local.getStreamSyncedMeta("roleplayName"))
})

alt.setInterval(() => {
    const cash = alt.Player.local.getStreamSyncedMeta('cash')
    const hunger = alt.Player.local.getStreamSyncedMeta('hunger')
    const thirst = alt.Player.local.getStreamSyncedMeta('thirst')

    alt.emit('UiManager:Emit', 'PlayerHud:Update', cash, hunger, thirst)
}, 250)