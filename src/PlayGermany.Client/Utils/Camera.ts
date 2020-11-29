import * as alt from 'alt-client'
import * as natives from 'natives'

export default class Camera {
    private _position: alt.Vector3
    private _rotation: alt.Vector3
    private _fov: number
    private scriptID: number

    /**
     * Create a new camera
     *
     * @param {alt.Vector3} position Initial position of the camera
     * @param {alt.Vector3} rotation Initial rotation of the camera
     * @param {number} fov Initial field of view of the camera
     */
    constructor(position: alt.Vector3, rotation: alt.Vector3, fov: number) {
        this._position = position
        this._rotation = rotation
        this._fov = fov

        this.scriptID = natives.createCamWithParams(
            'DEFAULT_SCRIPTED_CAMERA',
            this._position.x,
            this._position.y,
            this._position.z,
            this._rotation.x,
            this._rotation.y,
            this._rotation.z,
            this._fov,
            true,
            0,
        )

        alt.log(`--> Created camera: ${this.scriptID}`)
    }

    /**
     * @type {number}
     */
    get fov() {
        return this._fov
    }

    set fov(value) {
        this._fov = value
        natives.setCamFov(this.scriptID, this._fov)
        this.render()
    }

    /**
     * @type {alt.Vector3}
     */
    get position() {
        return this._position
    }

    set position(position) {
        this._position = position
        natives.setCamCoord(this.scriptID, this._position.x, this._position.y, this._position.z)
        this.render()
    }

    /**
     * @type {alt.Vector3}
     */
    get rotation() {
        return this._rotation
    }

    set rotation(rotation) {
        this._rotation = rotation
        natives.setCamRot(this.scriptID, this._rotation.x, this._rotation.y, this._rotation.z, 0)
        this.render()
    }

    /**
     * Stops rendering the camera on the screen
     */
    unrender() {
        natives.renderScriptCams(false, false, 0, false, false, 0)
        natives.clearFocus()
        natives.clearHdArea()
    }

    /**
     * Renders the camera view on the screen
     */
    render() {
        natives.setCamActive(this.scriptID, true)
        natives.renderScriptCams(true, false, 0, true, false, 0)
        natives.setFocusPosAndVel(this._position.x, this._position.y, this._position.z, 0, 0, 0)
        //natives.setHdArea(this._position.x, this._position.y, this._position.z, 30)
    }

    /**
     * Destroys the camera
     */
    destroy() {
        this.unrender()
        natives.destroyCam(this.scriptID, false)

        alt.log(`--> Destroyed camera: ${this.scriptID}`)
    }

    /**
     * Rotates camera so it points straight to a ped's specific bone
     *
     * @param {number} entity Ped handle that owns the bone
     * @param {number} bone Bone index
     * @param {number} xOffset Position offset of the camera X
     * @param {number} yOffset Position offset of the camera Y
     * @param {number} zOffset Position offset of the camera Z
     */
    pointAtBone(entity: number, bone: number, xOffset: number, yOffset: number, zOffset: number) {
        natives.pointCamAtPedBone(this.scriptID, entity, bone, xOffset, yOffset, zOffset, false)
        this.render()
    }

    /**
     * Rotates camera so it points straight to a position
     *
     * @param {alt.Vector3} position Vector3 to where to point the camera at
     */
    pointAtCoord(position: alt.Vector3) {
        natives.pointCamAtCoord(this.scriptID, position.x, position.y, position.z)
        this.render()
    }
}
