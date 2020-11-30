import * as alt from 'alt-client'

import './PlayerHud'
import './VehicleHud'
import './Notifications'

let view: alt.WebView = null

alt.onServer('UiManager:Initialize', async (url: string) => {
    // this call can take a while, thats why we have async function
    if (view) {
        view.destroy()
    }

    view = new alt.WebView(url)
    view.focus()

    view.on("loaded", () => {
        alt.emitServer("RequestSpawn")
    })
})

const uiCopyToClipboardHandler = (contents: string) => {
    view.emit('CopyToClipboard', contents)
}
alt.on('UiManager:CopyToClipboard', uiCopyToClipboardHandler)
alt.onServer('UiManager:CopyToClipboard', uiCopyToClipboardHandler)

alt.on('UiManager:Emit', (eventName: string, ...args: any[]) => {
    view.emit(eventName, ...args)
})

alt.on('UiManager:ShowComponent', (component: string) => {
    view.emit('ToggleComponent', component, true)
})

alt.on('UiManager:HideComponent', (component: string) => {
    view.emit('ToggleComponent', component, false)
})
