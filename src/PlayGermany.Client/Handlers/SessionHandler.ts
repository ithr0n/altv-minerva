import * as alt from 'alt-client'
import * as natives from 'natives'
import * as AsyncHelper from '../Utils/AsyncHelper'
import Camera from '../Utils/Camera'
import * as AltHelper from '../Utils/AltHelper'
import * as UiManager from '../UiManager'
import * as NewCharacterIntro from '../Modules/NewCharacterIntro'
import CharacterCreator from 'Modules/CharacterCreator'

let loginCamera: Camera

// Disable idle camera
let idle = alt.setInterval(() => {
    natives.invalidateIdleCam(); // Disable player idle camera
    natives._0x9E4CFFF989258472(); // Disable vehicle idle camera
}, 5000);

alt.on('connectionComplete', async () => {
    /*await*/ AsyncHelper.RequestModel(alt.hash('mp_f_freemode_01'))
    /*await*/ AsyncHelper.RequestModel(alt.hash('mp_m_freemode_01'))

    loginCamera = new Camera(new alt.Vector3(-637.12085, 4433.934, 26.870361), new alt.Vector3(0, 0, 271.66), 20)
    loginCamera.render()

    natives.setEntityAlpha(alt.Player.local.scriptID, 0, false)
    natives.setEntityInvincible(alt.Player.local.scriptID, true)
    natives.freezeEntityPosition(alt.Player.local.scriptID, true)
    natives.displayRadar(false)

    natives.pauseClock(true)
})

alt.onServer('Session:PlayerSpawning', (appearance: string[]) => {
    loginCamera.destroy()

    initializeCharacter(appearance)
})

let charCreationObj = {
    firstName: '',
    lastName: '',
    birthday: new Date(),
    genderIndex: 0,
    appearanceParents: '',
    appearanceFaceFeatures: '',
    appearanceDetails: '',
    appearanceHair: '',
    appearanceClothes: ''
}

alt.onServer('Login:Callback', (status: boolean, characters: any[]) => {
    // characters: { charId, charName }

    if (!status) {
        UiManager.emit('Login:Failed')
        return
    }

    UiManager.hideComponent('Login')
    AltHelper.showCursor(false)

    // here we could add character selection view
    if (characters.length < 1) {
        // todo: create new character
        const charCreator = new CharacterCreator()
        charCreator.show((data) => {
            UiManager.showComponent('CharacterCreation')
            AltHelper.showCursor()

            charCreationObj.genderIndex = data.GenderIndex
            charCreationObj.appearanceParents = data.Parents
            charCreationObj.appearanceFaceFeatures = data.Features
            charCreationObj.appearanceDetails = data.Appearance
            charCreationObj.appearanceHair = data.Hair
            charCreationObj.appearanceClothes = data.Clothes
        })
        
    } else {
        alt.emitServer('Session:RequestCharacterSpawn', characters[0].charId)
    }
})

UiManager.on('CharacterCreation:Submit', (firstName: string, lastName: string, birthday: Date) => {
    UiManager.hideComponent('CharacterCreation')
    AltHelper.showCursor(false)

    charCreationObj.firstName = firstName
    charCreationObj.lastName = lastName
    charCreationObj.birthday = birthday

    alt.emitServer('Session:CreateNewCharacter', charCreationObj)
})

alt.onServer('Session:PlayIntro', async () => {
    await NewCharacterIntro.Execute()

    // ...
})

const initializeCharacter = (appearance: string[]) => {
    if (appearance.length !== 5) return

    const player = alt.Player.local
    const parents = JSON.parse(appearance[0])
    const faceFeatures = JSON.parse(appearance[1])
    const details = JSON.parse(appearance[2])
    const hair = JSON.parse(appearance[3])
    const clothes = JSON.parse(appearance[4])

    natives.setPedHeadBlendData(
        player.scriptID,
        parents.Father,
        parents.Mother,
        0,
        parents.Father,
        parents.Mother,
        0,
        parents.Similarity,
        parents.SkinSimilarity,
        0,
        false,
    )

    natives.setPedComponentVariation(player.scriptID, 2, hair[0], 0, 2) // hair
    natives.setPedHairColor(player.scriptID, hair[1], hair[2]) // hair color
    natives.setPedHeadOverlayColor(player.scriptID, 2, 1, hair[3], 0) // eyebrown color
    natives.setPedHeadOverlayColor(player.scriptID, 1, 1, hair[4], 0) // beard color
    natives.setPedEyeColor(player.scriptID, hair[5]) // eye color
    natives.setPedHeadOverlayColor(player.scriptID, 5, 2, hair[6], 0) // blush color
    natives.setPedHeadOverlayColor(player.scriptID, 8, 2, hair[7], 0) // lipstick color
    natives.setPedHeadOverlayColor(player.scriptID, 10, 1, hair[8], 0) // chest hair color

    // features
    for (let i = 0; i < 20; i++) {
        natives.setPedFaceFeature(player.scriptID, i, faceFeatures[i])
    }

    // appearance
    for (let i = 0; i < 11; i++) {
        natives.setPedHeadOverlay(player.scriptID, i, details[i].Value, details[i].Opacity)
    }

    // clothes
    natives.setPedComponentVariation(player.scriptID, 1, clothes[0][0], clothes[0][1], 2) // masks
    natives.setPedComponentVariation(player.scriptID, 3, clothes[1][0], clothes[1][1], 2) // torso
    natives.setPedComponentVariation(player.scriptID, 4, clothes[2][0], clothes[2][1], 2) // legs
    natives.setPedComponentVariation(player.scriptID, 5, clothes[3][0], clothes[3][1], 2) // bags+parachute
    natives.setPedComponentVariation(player.scriptID, 6, clothes[4][0], clothes[4][1], 2) // shoes
    natives.setPedComponentVariation(player.scriptID, 8, clothes[5][0], clothes[5][1], 2) // undershirts
    natives.setPedComponentVariation(player.scriptID, 9, clothes[6][0], clothes[6][1], 2) // body armor
    natives.setPedComponentVariation(player.scriptID, 11, clothes[7][0], clothes[7][1], 2) // tops

    // make entity visible and movable
    natives.setEntityAlpha(alt.Player.local.scriptID, 255, false)
    natives.setEntityInvincible(alt.Player.local.scriptID, false)
    natives.freezeEntityPosition(alt.Player.local.scriptID, false)
    natives.displayRadar(true)

    // apply default flags
    natives.setPedConfigFlag(alt.Player.local.scriptID, 241, true) // PED_FLAG_DISABLE_STOPPING_VEH_ENGINE
    natives.setPedConfigFlag(alt.Player.local.scriptID, 429, true) // PED_FLAG_DISABLE_STARTING_VEH_ENGINE
    natives.setPedConfigFlag(alt.Player.local.scriptID, 184, true) // PED_FLAG_DISABLE_SHUFFLING_TO_DRIVER_SEAT
    natives.setPedConfigFlag(alt.Player.local.scriptID, 32, true) // Player_FLAG_CAN_FLY_THRU_WINDSCREEN
}