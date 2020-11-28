import * as alt from 'alt-client'

let cursorStatus = false;

export function showCursor(show: boolean = true) {
    if (cursorStatus == show) return

    cursorStatus = show
    alt.showCursor(show)
}