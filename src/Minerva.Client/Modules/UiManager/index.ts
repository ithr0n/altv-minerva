import * as alt from 'alt-client'

import './Notifications'

let view: alt.WebView = null
let eventCallbacks: { event: string, callback: (...args: any[]) => void }[] = []

namespace UiManager {
    export const initialize = (url: string) => {
        if (view) {
            view.destroy()
        }

        view = new alt.WebView(url)
        view.focus()

        for (let e of eventCallbacks) {
            view.on(e.event, e.callback)
        }
    }

    export const emit = (eventName: string, ...args: any[]) => {
        if (!view) return
        view.emit(eventName, ...args)
    }

    export const showComponent = (component: string) => {
        if (!view) return
        view.emit('ToggleComponent', component, true)
    }

    export const hideComponent = (component: string) => {
        if (!view) return
        view.emit('ToggleComponent', component, false)
    }

    export const toggleComponent = (component: string) => {
        if (!view) return
        view.emit('ToggleComponent', component)
    }

    export const setAppData = (key: string, value: any) => {
        if (!view) return
        view.emit('SetAppData', key, value)
    }

    export const copyToClipboard = (contents: string) => {
        if (!view) return
        view.emit('CopyToClipboard', contents)
    }

    export const on = (eventName: string, callback: (...args: any[]) => void) => {
        eventCallbacks.push({ event: eventName, callback })

        if (view) {
            view.on(eventName, (...args) => callback(...args))
        }
    }
}

export default UiManager

alt.onServer('UiManager:CopyToClipboard', UiManager.copyToClipboard)

UiManager.on("ui:Loaded", () => {
    alt.log('ui loaded')
})

UiManager.on('ui:EmitClient', (eventName, ...args) => {
    alt.emit(eventName, args)
})

UiManager.on('ui:EmitServer', (eventName, ...args) => {
    alt.emitServer(eventName, ...args)
})