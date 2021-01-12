import * as alt from 'alt-client'
import CommandSystem from './CommandSystem'

alt.on('consoleCommand', (...args) => {
    const argsStr = args.join(' ').trim()
    if (argsStr.length < 1) return
    if (CommandSystem.tryClientCommand(...args)) return
    alt.emitServer('Commands:Execute', argsStr)
})