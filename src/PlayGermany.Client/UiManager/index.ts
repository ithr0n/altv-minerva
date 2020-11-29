import * as alt from 'alt-client'
import natives from 'natives'

import './VehicleHud'

let view: alt.WebView = null

alt.onServer('UiManager:Initialize', (url: string) => {
    if (view) {
        view.destroy()
    }

    view = new alt.WebView(url)
    view.focus()

    alt.emitServer("SessionHandler:Spawn")
})

alt.onServer('UiManager:Notification', (text, type) => {

    view.emit('notification', text, type)

})

alt.on('UiManager:Emit', (eventName: string, ...args: any[]) => {
    view.emit(eventName, ...args)
})

alt.on('UiManager:ShowComponent', (component: string) => {
    view.emit('ToggleComponent', component, true)
})

alt.on('UiManager:HideComponent', (component: string) => {
    view.emit('ToggleComponent', component, false)
})
