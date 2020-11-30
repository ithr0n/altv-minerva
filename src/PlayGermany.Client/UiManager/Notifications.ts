import * as alt from 'alt-client'

alt.onServer('UiManager:Notification', (message: string) => {
    alt.emit('UiManager:Emit', 'Notifications:Simple', message)
})
