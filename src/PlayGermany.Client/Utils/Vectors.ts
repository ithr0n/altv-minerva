import * as alt from 'alt-client'
import * as natives from 'natives'

export function distance(vector1: alt.Vector3, vector2: alt.Vector3) {
    if (!vector1 || !vector2) {
        throw new Error('AddVector => vector1 or vector2 is undefined')
    }

    return Math.sqrt((vector1.x - vector2.x) ** 2 + (vector1.y - vector2.y) ** 2 + (vector1.z - vector2.z) ** 2)
}

export function degToRad(degrees: number) {
    return (degrees * Math.PI) / 180
}

export function radToDeg(radians: number) {
    return (radians * 180) / Math.PI
}

function normalizeVector(vector: alt.Vector3) {
    const mag = natives.vmag(vector.x, vector.y, vector.z)

    return { x: vector.x / mag, y: vector.y / mag, z: vector.z / mag }
}

export function rotAnglesToVector(rotation: alt.Vector3) {
    const z = degToRad(rotation.z)
    const x = degToRad(rotation.x)
    const num = Math.abs(Math.cos(x))

    return { x: -(Math.sin(z) * num), y: Math.cos(z) * num, z: Math.sin(x) }
}

export function vectorToRotAngles(vector: alt.Vector3) {
    const normalizedVector = normalizeVector(vector)
    const ax = radToDeg(Math.asinh(normalizedVector.z))
    const az = radToDeg(Math.atan2(normalizedVector.x, normalizedVector.y))

    return { x: ax, y: 0, z: -az }
}

export function add(v1: alt.Vector3, v2: alt.Vector3) {
    return new alt.Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z)
}

export function multiply(vector: alt.Vector3, scalar: number) {
    return new alt.Vector3(vector.x * scalar, vector.y * scalar, vector.z * scalar)
}