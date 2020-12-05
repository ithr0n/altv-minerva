import * as alt from 'alt-client'

let cursorStatus = false;

export function showCursor(show: boolean = true) {
    if (cursorStatus == show) return

    cursorStatus = show
    alt.showCursor(show)
}

export function random(min: number, max: number) {
    return Math.floor(Math.random() * (max - min + 1) + min);
}