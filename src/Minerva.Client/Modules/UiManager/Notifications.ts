import * as alt from 'alt-client'
import UiManager from '../UiManager'

namespace Notifications {
    export const notify = (message: string) => {
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
}

export default Notifications

alt.onServer('UiManager:Notification', (message: string) => {
    Notifications.notify(message)
})

alt.onServer('UiManager:Info', (message: string) => {
    Notifications.info(message)
})

alt.onServer('UiManager:Success', (message: string) => {
    Notifications.success(message)
})

alt.onServer('UiManager:Warning', (message: string) => {
    Notifications.warning(message)
})

alt.onServer('UiManager:Error', (message: string) => {
    Notifications.error(message)
})
