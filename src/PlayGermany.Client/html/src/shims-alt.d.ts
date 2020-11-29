import '@altv/types-webview'

declare module 'vue/types/vue' {
    interface Vue {
        $alt: Alt
    }
}
