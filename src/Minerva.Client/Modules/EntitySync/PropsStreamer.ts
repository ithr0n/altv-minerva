import * as alt from 'alt-client'
import * as natives from 'natives'
import * as AsyncHelper from '../../Utils/AsyncHelper'

class PropEntity {
    public Handle: number
    public Position: alt.Vector3

    public Model: string
    public Rotation: alt.Vector3
    public LodDistance: number
    public TextureVariation: number
    public Dynamic: boolean
    public Visible: boolean
    public OnFire: boolean
    public Frozen: boolean
    public LightColor: alt.RGBA
    public Velocity: alt.Vector3
    public IsCollisionless: boolean
    public IsOnGround: boolean
    public SlideToPosition: alt.Vector3

    public FireHandle: number
}

var entities: PropEntity[] = []
const EntityType = 2

alt.onServer("entitySync:create", async (entityId: number, entityType: number, position: alt.Vector3, newEntityData: any) => {
    if (entityType !== EntityType)
        return;

    /*alt.log('entitySync:create (prop)')
    alt.log(entityId)
    alt.log(entityType)
    alt.log(position)
    alt.log(JSON.stringify(newEntityData))*/

    if (newEntityData) {
        let entity = new PropEntity()
        entity.Model = newEntityData.model
        entity.Rotation = newEntityData.rotation
        entity.LodDistance = newEntityData.lodDistance
        entity.TextureVariation = newEntityData.textureVariation
        entity.Dynamic = !!newEntityData.dynamic
        entity.Visible = !!newEntityData.visible
        entity.OnFire = !!newEntityData.onFire
        entity.Frozen = !!newEntityData.freezed
        entity.LightColor = newEntityData.lightColor
        entity.Velocity = newEntityData.velocity
        entity.IsCollisionless = !!newEntityData.isCollisionless
        entity.IsOnGround = !!newEntityData.placedOnGround

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

        if (entities[entityId].Handle) {
            natives.setEntityCoordsNoOffset(entities[entityId].Handle, position.x, position.y, position.z, false, false, false)
        }
    }
})

alt.onServer("entitySync:updateData", async (entityId, entityType, newEntityData) => {
    if (entityType !== EntityType || !entities.hasOwnProperty(entityId))
        return;

    const e = entities[entityId]

    if (newEntityData.hasOwnProperty("model")) {
        e.Model = newEntityData.model

        if (e.Handle) {
            removeEntityAtClient(e)
            await spawnEntityAtClient(e)
        }
    }

    if (newEntityData.hasOwnProperty("rotation")) {
        e.Rotation = newEntityData.rotation
        if (e.Handle) natives.setEntityRotation(e.Handle, e.Rotation.x, e.Rotation.y, e.Rotation.z, 2, true);
    }

    if (newEntityData.hasOwnProperty("lodDistance")) {
        e.LodDistance = newEntityData.lodDistance
        if (e.Handle) natives.setEntityLodDist(e.Handle, e.LodDistance);
    }

    if (newEntityData.hasOwnProperty("textureVariation")) {
        e.TextureVariation = newEntityData.textureVariation
        if (e.Handle) natives.setObjectTextureVariation(e.Handle, e.TextureVariation);
    }

    if (newEntityData.hasOwnProperty("dynamic")) {
        e.Dynamic = !!newEntityData.dynamic
        if (e.Handle) natives.setEntityDynamic(e.Handle, e.Dynamic);
    }

    if (newEntityData.hasOwnProperty("visible")) {
        e.Visible = !!newEntityData.visible
        if (e.Handle) natives.setEntityVisible(e.Handle, e.Visible, false);
    }

    if (newEntityData.hasOwnProperty("onFire")) {
        e.OnFire = !!newEntityData.onFire

        if (e.FireHandle) {
            natives.removeScriptFire(e.FireHandle);

            if (e.OnFire) {
                e.FireHandle = natives.startScriptFire(e.Position.x, e.Position.y, e.Position.z, 1, false);
            } else {
                e.FireHandle = null;
            }
        }
    }

    if (newEntityData.hasOwnProperty("frozen")) {
        e.Frozen = !!newEntityData.frozen
        if (e.Handle) natives.freezeEntityPosition(e.Handle, e.Frozen);
    }

    if (newEntityData.hasOwnProperty("lightColor")) {
        e.LightColor = newEntityData.lightColor

        if (e.Handle) {
            if (e.LightColor) {
                natives.setObjectLightColor(e.Handle, true, e.LightColor.r, e.LightColor.g, e.LightColor.b);
            } else {
                natives.setObjectLightColor(e.Handle, true, 0, 0, 0);
            }
        }
    }

    if (newEntityData.hasOwnProperty("velocity")) {
        e.Velocity = newEntityData.velocity
        if (e.Handle) natives.setEntityVelocity(e.Handle, e.Velocity.x, e.Velocity.y, e.Velocity.z);
    }

    if (newEntityData.hasOwnProperty("isCollisionless")) {
        e.IsCollisionless = !!newEntityData.isCollisionless
        if (e.Handle) natives.setEntityCollision(e.Handle, e.IsCollisionless, false); // todo keep physics?!
    }

    if (newEntityData.hasOwnProperty("placedOnGround")) {
        e.IsOnGround = !!newEntityData.placedOnGround
        if (e.Handle) natives.placeObjectOnGroundProperly(e.Handle);
    }

    if (newEntityData.hasOwnProperty("slideToPosition")) {
        e.SlideToPosition = newEntityData.slideToPosition
        if (e.Handle) natives.slideObject(e.Handle, e.SlideToPosition.x, e.SlideToPosition.y, e.SlideToPosition.z, 8, 8, 8, true);
    }
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

const spawnEntityAtClient = async (e: PropEntity) => {
    if (!e) return

    let modelHash = Number(e.Model)
    if (isNaN(modelHash)) modelHash = alt.hash(e.Model)

    await AsyncHelper.RequestModel(modelHash)

    e.Handle = natives.createObjectNoOffset(modelHash, e.Position.x, e.Position.y, e.Position.z, false, false, false);
    natives.setEntityRotation(e.Handle, e.Rotation.x, e.Rotation.y, e.Rotation.z, 2, true);
    natives.setEntityDynamic(e.Handle, e.Dynamic);
    natives.freezeEntityPosition(e.Handle, e.Frozen);
    natives.setEntityCollision(e.Handle, e.IsCollisionless, false) // todo keep physics?

    if (e.TextureVariation !== null) {
        natives.setObjectTextureVariation(e.Handle, e.TextureVariation);
    }

    if (e.Velocity) {
        natives.setEntityVelocity(e.Handle, e.Velocity.x, e.Velocity.y, e.Velocity.z);
    }

    if (e.IsOnGround) {
        natives.placeObjectOnGroundProperly(e.Handle);
    }

    if (e.OnFire) {
        e.FireHandle = natives.startScriptFire(e.Position.x, e.Position.y, e.Position.z, 1, false);
    }

    if (e.LightColor) {
        natives.setObjectLightColor(e.Handle, true, e.LightColor.r, e.LightColor.g, e.LightColor.b);
    }
}

const removeEntityAtClient = (entity: PropEntity) => {
    if (!entity) return

    if (entity.Handle) {
        natives.deleteObject(entity.Handle);
        entity.Handle = null;
    }

    if (entity.FireHandle) {
        natives.stopEntityFire(entity.FireHandle)
        entity.FireHandle = null
    }
}