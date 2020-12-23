import * as alt from 'alt-client'
import * as AltHelper from '../Utils/AltHelper'

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
        view.emit('ToggleComponent', 'Login', true)
        AltHelper.showCursor()
        alt.emit('UiManager:Loaded')
    })

    view.on('ui:EmitClient', (eventName, ...args) => {
        alt.emit(eventName, args)
    })

    view.on('ui:EmitServer', (eventName, ...args) => {
        alt.emitServer(eventName, args)
    })
})

export const emit = (eventName: string, ...args: any[]) => {
    if (view === null) return
    view.emit(eventName, args)
}

export const showComponent = (component: string) => {
    if (view === null) return
    view.emit('ToggleComponent', component, true)
}

export const hideComponent = (component: string) => {
    if (view === null) return
    view.emit('ToggleComponent', component, false)
}

export const toggleComponent = (component: string) => {
    if (view === null) return
    view.emit('ToggleComponent', component)
}

export const setAppData = (key: string, value: any) => {
    if (view === null) return
    view.emit('SetAppData', key, value)
}

export const copyToClipboard = (contents: string) => {
    if (view === null) return
    view.emit('CopyToClipboard', contents)
}

export const on = (eventName: string, callback: (...args: any[]) => void) => {
    if (view === null) return
    view.on(eventName, (...args) => callback(args))
}

alt.onServer('UiManager:CopyToClipboard', copyToClipboard)