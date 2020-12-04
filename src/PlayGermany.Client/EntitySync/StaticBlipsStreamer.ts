import * as alt from 'alt-client'
import * as natives from 'natives'

class StaticBlipEntity {
    public Handle: alt.PointBlip
    public Position: alt.Vector3

    public Name: string
    public SpriteId: number
    public Color: number
    public Scale: number
    public ShortRange: boolean
}

var entities: StaticBlipEntity[] = []
const EntityType = 4

alt.onServer("entitySync:create", async (entityId: number, entityType: number, position: alt.Vector3, newEntityData: any) => {
    if (entityType !== EntityType)
        return;

    /*alt.log('entitySync:create (prop)')
    alt.log(entityId)
    alt.log(entityType)
    alt.log(position)
    alt.log(JSON.stringify(newEntityData))*/

    if (newEntityData) {
        let entity = new StaticBlipEntity()
        entity.Name = newEntityData.name
        entity.SpriteId = newEntityData.spriteId
        entity.Color = newEntityData.color
        entity.Scale = newEntityData.scale
        entity.ShortRange = !!newEntityData.shortRange

        entity.Position = position

        entities[entityId] = entity;

        await spawnEntityAtClient(entity)
    } else {
        let restoredEntity = entities[entityId]

        await spawnEntityAtClient(restoredEntity)
    }
})

alt.onServer("entitySync:remove", (entityId, entityType) => {
    if (entityType !== EntityType)
        return;

    if (entities.hasOwnProperty(entityId)) {
        removeEntityAtClient(entities[entityId])
    }
})

alt.onServer("entitySync:updatePosition", async (entityId, entityType, position) => {
    if (entityType !== EntityType)
        return;

    if (entities.hasOwnProperty(entityId)) {
        entities[entityId].Position = position
        entities[entityId].Handle.pos = position
    }
})

alt.onServer("entitySync:updateData", async (entityId, entityType, newEntityData) => {
    if (entityType !== EntityType || !entities.hasOwnProperty(entityId))
        return;

    const e = entities[entityId]

    if (newEntityData.hasOwnProperty("name")) {
        e.Name = newEntityData.name
    }

    if (newEntityData.hasOwnProperty("spriteId")) {
        e.SpriteId = newEntityData.spriteId
    }

    if (newEntityData.hasOwnProperty("color")) {
        e.Color = newEntityData.color
    }

    if (newEntityData.hasOwnProperty("scale")) {
        e.Scale = newEntityData.scale
    }

    if (newEntityData.hasOwnProperty("shortRange")) {
        e.ShortRange = !!newEntityData.shortRange
    }

    removeEntityAtClient(e)
    spawnEntityAtClient(e)
})

alt.onServer("entitySync:clearCache", (entityId, entityType) => {
    if (entityType !== EntityType)
        return;

    if (entities.hasOwnProperty(entityId)) {
        removeEntityAtClient(entities[entityId])

        delete entities[entityId];
    }
})

alt.onServer("entitySync:netOwner", (entityId, entityType, isNetOwner) => {
    //...
})

const spawnEntityAtClient = (e: StaticBlipEntity) => {
    if (!e) return

    e.Handle = new alt.PointBlip(e.Position.x, e.Position.y, e.Position.z);

    e.Handle.sprite = e.SpriteId;
    e.Handle.color = e.Color;
    e.Handle.scale = e.Scale;
    e.Handle.shortRange = e.ShortRange;
    e.Handle.name = e.Name;
}

const removeEntityAtClient = (entity: StaticBlipEntity) => {
    if (!entity) return

    if (entity.Handle) {
        entity.Handle.destroy()
        entity.Handle = null
    }
}