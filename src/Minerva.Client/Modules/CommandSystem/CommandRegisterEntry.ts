import AdminLevel from "../../Contracts/AdminLevel";

export default class CommandRegisterEntry {
    constructor(
        public RequiredAccessLevel: AdminLevel,
        public Callback: (...args: string[]) => {}) {
    }
}