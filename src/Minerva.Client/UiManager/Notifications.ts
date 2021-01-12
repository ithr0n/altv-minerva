import * as alt from 'alt-client'
import * as UiManager from '../UiManager'

export const notification = (message: string) => {
    UiManager.emit('Notifications:Notify', message)
}

export const info = (message: string) => {
    UiManager.emit('Notifications:Info', message)
}

export const success = (message: string) => {
    UiManager.emit('Notifications:Success', message)
}

export const warning = (message: string) => {
    UiManager.emit('Notifications:Warning', message)
}

export const error = (message: string) => {
    UiManager.emit('Notifications:Error', message)
}

alt.onServer('UiManager:Notification', (message: string) => {
    notification(message)
})

alt.onServer('UiManager:Info', (message: string) => {
    info(message)
})

alt.onServer('UiManager:Success', (message: string) => {
    success(message)
})

alt.onServer('UiManager:Warning', (message: string) => {
    warning(message)
})

alt.onServer('UiManager:Error', (message: string) => {
    error(message)
})
