import * as alt from 'alt-client'

alt.onServer('UiManager:Notification', (message: string) => {
    alt.emit('UiManager:Emit', 'Notifications:Notify', message)
})

alt.onServer('UiManager:Info', (message: string) => {
    alt.emit('UiManager:Emit', 'Notifications:Info', message)
})

alt.onServer('UiManager:Success', (message: string) => {
    alt.emit('UiManager:Emit', 'Notifications:Success', message)
})

alt.onServer('UiManager:Warning', (message: string) => {
    alt.emit('UiManager:Emit', 'Notifications:Warning', message)
})

alt.onServer('UiManager:Error', (message: string) => {
    alt.emit('UiManager:Emit', 'Notifications:Error', message)
})
