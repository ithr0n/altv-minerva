import * as alt from 'alt-client'
import natives from 'natives'

let view: alt.WebView = null

alt.onServer('View:Initialize', (url: string) => {
    if (view) {
        view.destroy()
    }

    view = new alt.WebView(url)
    view.isVisible = true
})

alt.onServer('View:Notification', (text, type) => {

    view.emit('notification', text, type)

})