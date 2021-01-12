import * as alt from 'alt-client'
import * as natives from 'natives'

class MarkerEntity {
    public Position: alt.Vector3

    public MarkerType: number
    public Scale: alt.Vector3
    public Rotation: alt.Vector3
    public Direction: alt.Vector3
    public Color: alt.RGBA
    public BobUpDown: boolean
    public FaceCamera: boolean
    public Rotate: boolean
    public TextureDict: string
    public TextureName: string
    public DrawOnEnter: boolean

    public OnDisplay: boolean
}

var entities: MarkerEntity[] = []
const EntityType = 0

alt.onServer("entitySync:create", async (entityId: number, entityType: number, position: alt.Vector3, newEntityData: any) => {
    if (entityType !== EntityType)
        return;

    /*alt.log('entitySync:create (prop)')
    alt.log(entityId)
    alt.log(entityType)
    alt.log(position)
    alt.log(JSON.stringify(newEntityData))*/

    if (newEntityData) {
        let entity = new MarkerEntity()
        entity.MarkerType = newEntityData.markerType
        entity.Scale = newEntityData.scale
        entity.Rotation = newEntityData.rotation
        entity.Direction = newEntityData.direction
        entity.Color = newEntityData.color
        entity.BobUpDown = !!newEntityData.bobUpDown
        entity.FaceCamera = !!newEntityData.faceCamera 
        entity.Rotate = !!newEntityData.rotate
        entity.TextureDict = newEntityData.textureDict
        entity.TextureName = newEntityData.textureName
        entity.DrawOnEnter = !!newEntityData.drawOnEnter

        entity.Position = position
        entity.OnDisplay = true

        entities[entityId] = entity;
    } else {
        let restoredEntity = entities[entityId]
        restoredEntity.OnDisplay = true
    }
})

alt.onServer("entitySync:remove", (entityId, entityType) => {
    if (entityType !== EntityType)
        return;

    if (entities.hasOwnProperty(entityId)) {
        entities[entityId].OnDisplay = false
    }
})

alt.onServer("entitySync:updatePosition", async (entityId, entityType, position) => {
    if (entityType !== EntityType)
        return;

    if (entities.hasOwnProperty(entityId)) {
        entities[entityId].Position = position
    }
})

alt.onServer("entitySync:updateData", async (entityId, entityType, newEntityData) => {
    if (entityType !== EntityType || !entities.hasOwnProperty(entityId))
        return;

    const e = entities[entityId]

    if (newEntityData.hasOwnProperty("markerType")) {
        e.MarkerType = newEntityData.markerType
    }

    if (newEntityData.hasOwnProperty("scale")) {
        e.Scale = newEntityData.scale
    }

    if (newEntityData.hasOwnProperty("rotation")) {
        e.Rotation = newEntityData.rotation
    }

    if (newEntityData.hasOwnProperty("direction")) {
        e.Direction = newEntityData.direction
    }

    if (newEntityData.hasOwnProperty("color")) {
        e.Color = newEntityData.color
    }

    if (newEntityData.hasOwnProperty("bobUpDown")) {
        e.BobUpDown = !!newEntityData.bobUpDown
    }

    if (newEntityData.hasOwnProperty("faceCamera")) {
        e.FaceCamera = !!newEntityData.faceCamera
    }

    if (newEntityData.hasOwnProperty("rotate")) {
        e.Rotate = !!newEntityData.rotate
    }

    if (newEntityData.hasOwnProperty("textureDict")) {
        e.TextureDict = newEntityData.textureDict
    }

    if (newEntityData.hasOwnProperty("textureName")) {
        e.TextureName = newEntityData.textureName
    }

    if (newEntityData.hasOwnProperty("drawOnEnter")) {
        e.DrawOnEnter = !!newEntityData.drawOnEnter
    }
})

alt.onServer("entitySync:clearCache", (entityId, entityType) => {
    if (entityType !== EntityType)
        return;

    if (entities.hasOwnProperty(entityId)) {
        delete entities[entityId];
    }
})

alt.onServer("entitySync:netOwner", (entityId, entityType, isNetOwner) => {
    //...
})

alt.everyTick(() => {
    for (var entity of entities) {
        if (entity.OnDisplay) {
            natives.drawMarker(
                entity.MarkerType,
                entity.Position.x, entity.Position.y, entity.Position.z,
                entity.Direction.x, entity.Direction.y, entity.Direction.z,
                entity.Rotation.x, entity.Rotation.y, entity.Rotation.z,
                entity.Scale.x, entity.Scale.y, entity.Scale.z,
                entity.Color.r, entity.Color.g, entity.Color.b, entity.Color.a,
                entity.BobUpDown,
                entity.FaceCamera,
                2,
                entity.Rotate,
                entity.TextureDict,
                entity.TextureName,
                entity.DrawOnEnter
            );
        }
    }
});