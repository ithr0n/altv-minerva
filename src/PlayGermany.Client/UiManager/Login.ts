import * as alt from 'alt-client'

alt.onServer('Login:Callback', (status: boolean) => {
    if (status) {
        alt.emit('UiManager:HideComponent', 'Login')
        alt.emitServer('RequestSpawn')
    } else {
        alt.emit('UiManager:Emit', 'Login:Failed')
    }
})