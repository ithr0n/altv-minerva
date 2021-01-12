import * as alt from 'alt-client'
import CommandRegisterEntry from './CommandRegisterEntry'
import AdminLevel from '../../Contracts/AdminLevel'

const localCommands = new Map<string, CommandRegisterEntry>()

export namespace CommandSystem {
    export const registerClientCommand = (name: string, callbackFunction: (...args: string[]) => {}, requiredAccessLevel = AdminLevel.Player) => {
        if (localCommands.has(name)) {
            alt.log(`Error: Command already registered (${name})!`)
            return
        }

        const entry = new CommandRegisterEntry(requiredAccessLevel, callbackFunction)

        localCommands.set(name, entry)
    }

    export const tryClientCommand = (...args: any[]) => {
        if (args[0] && localCommands.has(args[0])) {
            const argsCopy = Array.from(args)
            argsCopy.shift()

            const commandEntry = localCommands.get(args[0])
            const currentPlayerAccessLevel = alt.Player.local.getMeta('accessLevel')

            if (
                commandEntry.RequiredAccessLevel &&
                (currentPlayerAccessLevel & commandEntry.RequiredAccessLevel) === 0
            ) {
                alt.emit('Notification:Popup', 'Dafür hast du keine Berechtigung!', 'error')
                return false
            }

            commandEntry.Callback(...argsCopy)
            return true
        }

        return false
    }
}

export default CommandSystem