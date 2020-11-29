import _Vue from 'vue'

declare module '*.vue' {
    interface Vue {
        $alt: Alt
    }
}

const install = (app: typeof _Vue) => {
    if ('alt' in window) app.prototype.$alt = alt
    else
        app.prototype.$alt = {
            emit: (eventName: string, ...args: any[]) => {
                console.log(`[ALT]::[EMIT] - ${eventName} - ${args}`)
            },
            off: (eventName: string, _listener: (...args: any[]) => void) => {
                console.log(`[ALT]::[OFF] - ${eventName}`)
            },
            on: (eventName: string, _listener: (...args: any[]) => void) => {
                console.log(`[ALT]::[ON] - ${eventName}`)
            },
        }
}

export default {
    install,
}
