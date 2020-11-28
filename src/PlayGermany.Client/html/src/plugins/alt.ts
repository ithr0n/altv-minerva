import Vue from 'vue'

declare module '*.vue' {
    interface Vue {
        $alt: Alt
    }
}

const install = (vue: typeof Vue) => {
    if ('alt' in window) vue.prototype.$alt = alt
    else
        vue.prototype.$alt = {
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
