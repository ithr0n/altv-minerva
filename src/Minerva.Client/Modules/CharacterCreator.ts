import * as alt from 'alt-client'
import * as natives from 'natives'
import * as NativeUI from '../NativeUI/NativeUi'
import Camera from '../Modules/Camera'
import * as data from '../Data/CharacterCreatorData'

// i am so sorry for this ugly code, but it's still most clear way
// https://wiki.rage.mp/index.php?title=Male_Torso_Top_Combinations
// https://wiki.rage.mp/index.php?title=Female_Torso_Top_Combinations
const uglyClothesDefinitions = [
    // male
    [
        // pants, shoes, undershirt, top, torso
        [1, 1, 1, 1, 0],
        [5, 9, 15, 38, 8],
        [4, 3, 15, 321, 1],
        [10, 20, 13, 58, 1],
    ],
    // female
    [
        [1, 1, 20, 23, 0],
        [58, 4, 34, 3, 3],
        [23, 13, 34, 26, 12],
        [6, 8, 20, 7, 1],
    ],
]

export const CharCreatorCamPos = new alt.Vector3(402.97583, -999.5868, -98.51465)
export const CharCreatorPedPos = new alt.Vector3(402.93603515625, -996.7662963867188, -100.00023651123047)
export const CharCreatorPedHeading = 180

export class CharacterCreator {
    private currentGender: number
    private selectedClothingPack: number
    private pedHandle: number

    private lastGenderUpdate: Date

    private similarities: string[]
    private angles: string[]
    private zooms: string[]
    private hairColors: string[]
    private blushColors: string[]
    private lipstickColors: string[]
    private opacities: string[]
    private features: string[]

    private camera: Camera

    private creatorMenus: NativeUI.Menu[]
    private appearanceItems: NativeUI.UIMenuListItem[]
    private appearanceOpacityItems: NativeUI.UIMenuListItem[]
    private featureItems: NativeUI.UIMenuListItem[]
    private menuPoint: NativeUI.Point
    private genderItem: NativeUI.UIMenuListItem
    private angleItem: NativeUI.UIMenuListItem
    private zoomItem: NativeUI.UIMenuListItem
    private fatherItem: NativeUI.UIMenuListItem
    private motherItem: NativeUI.UIMenuListItem
    private similarityItem: NativeUI.UIMenuListItem
    private skinSimilarityItem: NativeUI.UIMenuListItem

    private creatorMainMenu: NativeUI.Menu
    private creatorParentsMenu: NativeUI.Menu
    private creatorFeaturesMenu: NativeUI.Menu
    private creatorAppearanceMenu: NativeUI.Menu
    private creatorHairMenu: NativeUI.Menu
    private creatorClothesMenu: NativeUI.Menu

    private hairItem: NativeUI.UIMenuListItem
    private hairColorItem: NativeUI.UIMenuListItem
    private hairHighlightItem: NativeUI.UIMenuListItem
    private eyebrowColorItem: NativeUI.UIMenuListItem
    private beardColorItem: NativeUI.UIMenuListItem
    private eyeColorItem: NativeUI.UIMenuListItem
    private blushColorItem: NativeUI.UIMenuListItem
    private lipstickColorItem: NativeUI.UIMenuListItem
    private chestHairColorItem: NativeUI.UIMenuListItem

    private onCloseCallback: (appearanceData: any) => void

    constructor() {
        // init members
        this.currentGender = 0
        this.selectedClothingPack = 0
        this.lastGenderUpdate = new Date()

        this.similarities = []
        this.angles = []
        this.zooms = []
        this.hairColors = []
        this.blushColors = []
        this.lipstickColors = []
        this.opacities = []
        this.features = []

        this.creatorMenus = []
        this.appearanceItems = []
        this.appearanceOpacityItems = []
        this.featureItems = []

        for (let i = 0; i < data.maxHairColor; i++) this.hairColors.push(i.toString())
        for (let i = 0; i <= 100; i++) this.similarities.push(`${i}%`)
        for (let i = -180.0; i <= 180.0; i += 5.0) this.angles.push(i.toFixed(1))
        for (let i = 0; i < data.maxBlushColor; i++) this.blushColors.push(i.toString())
        for (let i = 0; i < data.maxLipstickColor; i++) this.lipstickColors.push(i.toString())
        for (let i = -1.0; i <= 1.01; i += 0.01) this.features.push(i.toFixed(2))
        for (let i = 0; i <= 100; i++) this.opacities.push(`${i}%`)
        for (let i = 0; i <= 5; i += 1) this.zooms.push(i.toFixed(1))

        this.menuPoint = new NativeUI.Point(50, 50)

        // re-used components
        this.genderItem = new NativeUI.UIMenuListItem(
            'Geschlecht',
            '~r~Dies setzt deine Veränderungen zurück.',
            new NativeUI.ItemsCollection(['Male', 'Female']),
        )
        this.angleItem = new NativeUI.UIMenuListItem('Rotation', '', new NativeUI.ItemsCollection(this.angles))
        this.zoomItem = new NativeUI.UIMenuListItem('Zoom', '', new NativeUI.ItemsCollection(this.zooms))
        this.fatherItem = new NativeUI.UIMenuListItem(
            'Vater',
            'Dein Vater.',
            new NativeUI.ItemsCollection(data.fatherNames),
        )
        this.motherItem = new NativeUI.UIMenuListItem(
            'Mutter',
            'Deine Mutter.',
            new NativeUI.ItemsCollection(data.motherNames),
        )
        this.similarityItem = new NativeUI.UIMenuListItem(
            'Ähnlichkeit',
            'Ähnlichkeit zu den Eltern.\n(weniger = mutter, höher = vater)',
            new NativeUI.ItemsCollection(this.similarities),
        )
        this.skinSimilarityItem = new NativeUI.UIMenuListItem(
            'Hautfarbe',
            'Ähnlichkeit zu den Eltern.\n(weniger = mutter, höher = vater)',
            new NativeUI.ItemsCollection(this.similarities),
        )

        this.initCreatorMainMenu()
        this.initClothesMainMenu()
        this.initCreatorHairMenu()
        this.initAppeareanceMenu()
        this.initParentsMenu()
        this.initFeaturesMenu()
    }

    resetPedClothesToDefault() {
        natives.setPedComponentVariation(
            this.pedHandle,
            4,
            this.currentGender === 0 ? uglyClothesDefinitions[0][0][0] : uglyClothesDefinitions[1][0][0],
            0,
            2,
        ) // pants
        natives.setPedComponentVariation(
            this.pedHandle,
            6,
            this.currentGender === 0 ? uglyClothesDefinitions[0][0][1] : uglyClothesDefinitions[1][0][1],
            0,
            2,
        ) // shoes
        natives.setPedComponentVariation(
            this.pedHandle,
            8,
            this.currentGender === 0 ? uglyClothesDefinitions[0][0][2] : uglyClothesDefinitions[1][0][2],
            0,
            2,
        ) // undershirt
        natives.setPedComponentVariation(
            this.pedHandle,
            11,
            this.currentGender === 0 ? uglyClothesDefinitions[0][0][3] : uglyClothesDefinitions[1][0][3],
            0,
            2,
        ) // top
        natives.setPedComponentVariation(
            this.pedHandle,
            3,
            this.currentGender === 0 ? uglyClothesDefinitions[0][0][4] : uglyClothesDefinitions[1][0][4],
            0,
            2,
        ) // torso
    }

    initClothesMainMenu() {
        this.creatorClothesMenu = new NativeUI.Menu('Kleidung', '', this.menuPoint)
        this.creatorClothesMenu.Visible = false

        this.creatorClothesMenu.AddItem(new NativeUI.UIMenuItem('Einfach', 'Ein einfaches Outfit.'))
        this.creatorClothesMenu.AddItem(new NativeUI.UIMenuItem('Locker', 'Ein lockeres Outfit.'))
        this.creatorClothesMenu.AddItem(new NativeUI.UIMenuItem('Schick', 'Ein schickes Outfit'))
        this.creatorClothesMenu.AddItem(
            new NativeUI.UIMenuItem('Anzug', 'Ein einfacher Anzug für angehende Geschäftsmänner'),
        )

        this.creatorClothesMenu.ItemSelect.on((item, index) => {
            this.selectedClothingPack = index // required for saving data

            natives.setPedComponentVariation(
                this.pedHandle,
                4,
                this.currentGender === 0 ? uglyClothesDefinitions[0][index][0] : uglyClothesDefinitions[1][index][0],
                0,
                2,
            ) // pants
            natives.setPedComponentVariation(
                this.pedHandle,
                6,
                this.currentGender === 0 ? uglyClothesDefinitions[0][index][1] : uglyClothesDefinitions[1][index][1],
                0,
                2,
            ) // shoes
            natives.setPedComponentVariation(
                this.pedHandle,
                8,
                this.currentGender === 0 ? uglyClothesDefinitions[0][index][2] : uglyClothesDefinitions[1][index][2],
                0,
                2,
            ) // undershirt
            natives.setPedComponentVariation(
                this.pedHandle,
                11,
                this.currentGender === 0 ? uglyClothesDefinitions[0][index][3] : uglyClothesDefinitions[1][index][3],
                0,
                2,
            ) // top
            natives.setPedComponentVariation(
                this.pedHandle,
                3,
                this.currentGender === 0 ? uglyClothesDefinitions[0][index][4] : uglyClothesDefinitions[1][index][4],
                0,
                2,
            ) // torso

            // re-apply lost changes
            this.updateHairAndColors()
        })

        // register menus
        this.creatorClothesMenu.ParentMenu = this.creatorMainMenu
        this.creatorClothesMenu.Visible = false
        this.creatorMenus.push(this.creatorClothesMenu)
    }

    initAppeareanceMenu() {
        this.creatorAppearanceMenu = new NativeUI.Menu('Aussehen', '', this.menuPoint)
        this.creatorAppearanceMenu.Visible = false

        for (let i = 0; i < data.appearanceNames.length; i++) {
            const items = []
            for (let z = 0; z < data.appearanceItemNames.length; z++) items.push(z.toString())
            const tempAppearanceItem = new NativeUI.UIMenuListItem(
                data.appearanceNames[i],
                '',
                new NativeUI.ItemsCollection(items),
            )
            this.appearanceItems.push(tempAppearanceItem)
            this.creatorAppearanceMenu.AddItem(tempAppearanceItem)

            const tempAppearanceOpacityItem = new NativeUI.UIMenuListItem(
                `${data.appearanceNames[i]} Opacity`,
                '',
                new NativeUI.ItemsCollection(this.opacities),
            )
            tempAppearanceOpacityItem.Index = 100
            this.appearanceOpacityItems.push(tempAppearanceOpacityItem)
            this.creatorAppearanceMenu.AddItem(tempAppearanceOpacityItem)
        }

        this.creatorAppearanceMenu.AddItem(new NativeUI.UIMenuItem('Randomize', '~r~Zufällig Generieren.'))
        this.creatorAppearanceMenu.AddItem(new NativeUI.UIMenuItem('Zurücksetzen', '~r~Einstellung zurücksetzen.'))

        // Charcreator => Apperance 1
        this.creatorAppearanceMenu.ItemSelect.on((item) => {
            switch (item.Text) {
                case 'Randomize':
                    for (let i = 0; i < data.appearanceNames.length; i++) {
                        this.appearanceItems[i].Index = CharacterCreator.getRandomInt(
                            0,
                            natives.getPedHeadOverlayValue(this.pedHandle, i) - 1,
                        )
                        this.appearanceOpacityItems[i].Index = CharacterCreator.getRandomInt(0, 100)
                        this.updateAppearance(i)
                    }
                    break

                case 'Zurücksetzen':
                    this.resetAppearanceMenu()
                    break

                default:
                    break
            }
        })

        // Charcreator => Apperance 2
        this.creatorAppearanceMenu.ListChange.on(() => {
            const idx =
                this.creatorAppearanceMenu.CurrentSelection % 2 === 0
                    ? this.creatorAppearanceMenu.CurrentSelection / 2
                    : Math.floor(this.creatorAppearanceMenu.CurrentSelection / 2)
            this.updateAppearance(idx)
        })

        // register menus
        this.creatorAppearanceMenu.ParentMenu = this.creatorMainMenu
        this.creatorAppearanceMenu.Visible = false
        this.creatorMenus.push(this.creatorAppearanceMenu)
    }

    initCreatorMainMenu() {
        this.creatorMainMenu = new NativeUI.Menu('Creator', '', this.menuPoint)
        this.creatorMainMenu.Visible = false
        this.creatorMainMenu.AddItem(this.genderItem)
        this.creatorMainMenu.AddItem(new NativeUI.UIMenuItem('Eltern', 'Deine Eltern.'))
        this.creatorMainMenu.AddItem(new NativeUI.UIMenuItem('Gesicht', 'Dein Gesicht.'))
        this.creatorMainMenu.AddItem(new NativeUI.UIMenuItem('Aussehen', 'Dein Aussehen.'))
        this.creatorMainMenu.AddItem(new NativeUI.UIMenuItem('Haare & Farben', 'Deine Haare und Farben.'))
        this.creatorMainMenu.AddItem(new NativeUI.UIMenuItem('Kleidung', 'Deine Kleidung.'))
        this.creatorMainMenu.AddItem(this.angleItem)
        this.creatorMainMenu.AddItem(this.zoomItem)
        this.creatorMainMenu.AddItem(new NativeUI.UIMenuItem('Einreisen', 'Erstelle deinen Charakter.'))

        // Charcreator = 1
        this.creatorMainMenu.ListChange.on((item, listIndex) => {
            if (item === this.genderItem) {
                // too fast changes will crash the game, so prevent it
                const compareDate = new Date(this.lastGenderUpdate.getTime() + 1000)

                if (compareDate > new Date()) {
                    const oldIndex = listIndex === 0 ? 1 : 0
                    this.genderItem.Index = oldIndex
                    return
                }

                this.lastGenderUpdate = new Date()

                this.currentGender = listIndex

                // mp.players.local.clearTasksImmediately();
                // applyCreatorOutfit();
                this.resetFeaturesMenu(true)
                this.resetAppearanceMenu(true)

                this.creatorHairMenu.Clear()
                this.fillHairMenu()
                this.creatorHairMenu.RefreshIndex()

                if (this.pedHandle) {
                    natives.deleteEntity(this.pedHandle)
                }

                const model = listIndex === 0 ? 'mp_m_freemode_01' : 'mp_f_freemode_01'
                this.createPed(model)
            } else if (item === this.angleItem) {
                natives.setEntityHeading(this.pedHandle, this.angleItem.SelectedValue)
                // mp.players.local.clearTasksImmediately();
            } else if (item === this.zoomItem) {
                this.camera.fov = 50 - this.zoomItem.SelectedValue * 2
            }
        })

        // Charcreator = 2
        this.creatorMainMenu.ItemSelect.on((item, index) => {
            switch (index) {
                case 1:
                    this.creatorMainMenu.Visible = false
                    this.creatorParentsMenu.Visible = true
                    break

                case 2:
                    this.creatorMainMenu.Visible = false
                    this.creatorFeaturesMenu.Visible = true
                    break

                case 3:
                    this.creatorMainMenu.Visible = false
                    this.creatorAppearanceMenu.Visible = true
                    break

                case 4:
                    this.creatorMainMenu.Visible = false
                    this.creatorHairMenu.Visible = true
                    break

                case 5:
                    this.creatorMainMenu.Visible = false
                    this.creatorClothesMenu.Visible = true
                    break

                case 8: {
                    const parentData = {
                        Father: this.fatherItem.Index,
                        Mother: this.motherItem.Index,
                        Similarity: 1.0 - this.similarityItem.Index / 100,
                        SkinSimilarity: 1.0 - this.skinSimilarityItem.Index / 100,
                    }

                    const featureData = []
                    for (let i = 0; i < this.featureItems.length; i++)
                        featureData.push(parseFloat(this.featureItems[i].SelectedValue))

                    const appearanceData = []
                    for (let i = 0; i < this.appearanceItems.length; i++)
                        appearanceData.push({
                            Value: this.appearanceItems[i].Index === 0 ? 255 : this.appearanceItems[i].Index - 1,
                            Opacity: this.appearanceOpacityItems[i].Index * 0.01,
                        })

                    const hairAndColors = [
                        data.hairList[this.currentGender][this.hairItem.Index].ID,
                        this.hairColorItem.Index,
                        this.hairHighlightItem.Index,
                        this.eyebrowColorItem.Index,
                        this.beardColorItem.Index,
                        this.eyeColorItem.Index,
                        this.blushColorItem.Index,
                        this.lipstickColorItem.Index,
                        this.chestHairColorItem.Index,
                    ]

                    const clothes = [
                        // drawable, texture
                        [0, 0], // 1: masks
                        // 2: hair styles -> not here
                        [uglyClothesDefinitions[this.currentGender][this.selectedClothingPack][4], 0], // 3: torsos
                        [uglyClothesDefinitions[this.currentGender][this.selectedClothingPack][0], 0], // 4: legs
                        [0, 0], // 5: bags+parachutes
                        [uglyClothesDefinitions[this.currentGender][this.selectedClothingPack][1], 0], // 6: shoes
                        // 7: accessories -> not here
                        [uglyClothesDefinitions[this.currentGender][this.selectedClothingPack][2], 0], // 8: undershirts
                        [0, 0], // 9: body armors
                        // 10: decals -> not (yet) here
                        [uglyClothesDefinitions[this.currentGender][this.selectedClothingPack][3], 0], // 11: tops
                    ]

                    this.hide()

                    this.onCloseCallback({
                        GenderIndex: this.currentGender,
                        Parents: parentData,
                        Features: featureData,
                        Appearance: appearanceData,
                        Hair: hairAndColors,
                        Clothes: clothes,
                    })

                    break
                }

                default:
                    break
            }
        })

        // Charcreator = Close Menu
        this.creatorMainMenu.MenuClose.on(() => {
            this.creatorMainMenu.Visible = true
        })
    }

    initCreatorHairMenu() {
        this.creatorHairMenu = new NativeUI.Menu('Haare & Farben', '', this.menuPoint)
        this.creatorHairMenu.Visible = false

        this.fillHairMenu()

        // Charctraor => Hair 1
        this.creatorHairMenu.ItemSelect.on((item) => {
            switch (item.Text) {
                case 'Randomize':
                    this.hairItem.Index = CharacterCreator.getRandomInt(0, data.hairList[this.currentGender].length - 1)
                    this.hairColorItem.Index = CharacterCreator.getRandomInt(0, data.maxHairColor)
                    this.hairHighlightItem.Index = CharacterCreator.getRandomInt(0, data.maxHairColor)
                    this.eyebrowColorItem.Index = CharacterCreator.getRandomInt(0, data.maxHairColor)
                    this.beardColorItem.Index = CharacterCreator.getRandomInt(0, data.maxHairColor)
                    this.eyeColorItem.Index = CharacterCreator.getRandomInt(0, data.maxEyeColor)
                    this.blushColorItem.Index = CharacterCreator.getRandomInt(0, data.maxBlushColor)
                    this.lipstickColorItem.Index = CharacterCreator.getRandomInt(0, data.maxLipstickColor)
                    this.chestHairColorItem.Index = CharacterCreator.getRandomInt(0, data.maxHairColor)
                    this.updateHairAndColors()
                    break

                case 'Zurücksetzen':
                    this.resetHairAndColorsMenu()
                    break

                default:
                    break
            }
        })

        // Charcreator => Hair 2
        this.creatorHairMenu.ListChange.on((item, listIndex) => {
            if (item === this.hairItem) {
                const hairStyle = data.hairList[this.currentGender][listIndex]
                // mp.players.local.setComponentVariation(2, hairStyle.ID, 0, 2);
                natives.setPedComponentVariation(this.pedHandle, 2, hairStyle.ID, 0, 2)
            } else {
                switch (this.creatorHairMenu.CurrentSelection) {
                    case 1: // hair color
                        natives.setPedHairColor(this.pedHandle, listIndex, this.hairHighlightItem.Index)
                        break

                    case 2: // hair highlight color
                        natives.setPedHairColor(this.pedHandle, this.hairColorItem.Index, listIndex)
                        break

                    case 3: // eyebrow color
                        natives.setPedHeadOverlayColor(this.pedHandle, 2, 1, listIndex, 0)
                        break

                    case 4: // facial hair color
                        natives.setPedHeadOverlayColor(this.pedHandle, 1, 1, listIndex, 0)
                        break

                    case 5: // eye color
                        natives.setPedEyeColor(this.pedHandle, listIndex)
                        break

                    case 6: // blush color
                        natives.setPedHeadOverlayColor(this.pedHandle, 5, 2, listIndex, 0)
                        break

                    case 7: // lipstick color
                        natives.setPedHeadOverlayColor(this.pedHandle, 8, 2, listIndex, 0)
                        break

                    case 8: // chest hair color
                        natives.setPedHeadOverlayColor(this.pedHandle, 10, 1, listIndex, 0)
                        break

                    default:
                        break
                }
            }
        })

        // register menus
        this.creatorHairMenu.ParentMenu = this.creatorMainMenu
        this.creatorHairMenu.Visible = false
        this.creatorMenus.push(this.creatorHairMenu)
    }

    fillHairMenu() {
        this.hairItem = new NativeUI.UIMenuListItem(
            'Haare',
            'Deine Haare.',
            new NativeUI.ItemsCollection(data.hairList[this.currentGender].map((h) => h.Name)),
        )
        this.creatorHairMenu.AddItem(this.hairItem)

        this.hairColorItem = new NativeUI.UIMenuListItem(
            'Haar Farbe',
            'Deine Haarfarbe.',
            new NativeUI.ItemsCollection(this.hairColors),
        )
        this.creatorHairMenu.AddItem(this.hairColorItem)

        this.hairHighlightItem = new NativeUI.UIMenuListItem(
            'Haar Highlight Farbe',
            'Deine Haar Highlight Farbe.',
            new NativeUI.ItemsCollection(this.hairColors),
        )
        this.creatorHairMenu.AddItem(this.hairHighlightItem)

        this.eyebrowColorItem = new NativeUI.UIMenuListItem(
            'Augenbrauen Farbe',
            'Deine Augenbrauen Farbe.',
            new NativeUI.ItemsCollection(this.hairColors),
        )
        this.creatorHairMenu.AddItem(this.eyebrowColorItem)

        this.beardColorItem = new NativeUI.UIMenuListItem(
            'Bart Farbe',
            'Deine Bart Farbe.',
            new NativeUI.ItemsCollection(this.hairColors),
        )
        this.creatorHairMenu.AddItem(this.beardColorItem)

        this.eyeColorItem = new NativeUI.UIMenuListItem(
            'Augen Farbe',
            'Deine Augenfarbe.',
            new NativeUI.ItemsCollection(data.eyeColors),
        )
        this.creatorHairMenu.AddItem(this.eyeColorItem)

        this.blushColorItem = new NativeUI.UIMenuListItem(
            'Rötungsfarbe',
            'Deine Rötungsfarbe.',
            new NativeUI.ItemsCollection(this.blushColors),
        )
        this.creatorHairMenu.AddItem(this.blushColorItem)

        this.lipstickColorItem = new NativeUI.UIMenuListItem(
            'Lippenstift Farbe',
            'Deine Lippenstift Farbe.',
            new NativeUI.ItemsCollection(this.lipstickColors),
        )
        this.creatorHairMenu.AddItem(this.lipstickColorItem)

        this.chestHairColorItem = new NativeUI.UIMenuListItem(
            'Körper Haar Farbe',
            'Deine Körper Haar Farbe.',
            new NativeUI.ItemsCollection(this.hairColors),
        )
        this.creatorHairMenu.AddItem(this.chestHairColorItem)

        this.creatorHairMenu.AddItem(new NativeUI.UIMenuItem('Randomize', '~r~Zufällig generieren.'))
        this.creatorHairMenu.AddItem(new NativeUI.UIMenuItem('Zurücksetzen', '~r~Einstellung zurücksetzen.'))
    }

    initParentsMenu() {
        this.creatorParentsMenu = new NativeUI.Menu('Eltern', '', this.menuPoint)
        this.creatorParentsMenu.Visible = false
        this.creatorParentsMenu.AddItem(this.fatherItem)
        this.creatorParentsMenu.AddItem(this.motherItem)
        this.creatorParentsMenu.AddItem(this.similarityItem)
        this.creatorParentsMenu.AddItem(this.skinSimilarityItem)
        this.creatorParentsMenu.AddItem(new NativeUI.UIMenuItem('Randomize', '~r~Zufällig generieren.'))

        // Charcreator => Parents 1
        this.creatorParentsMenu.ItemSelect.on((item) => {
            switch (item.Text) {
                case 'Randomize':
                    this.fatherItem.Index = CharacterCreator.getRandomInt(0, data.fathers.length - 1)
                    this.motherItem.Index = CharacterCreator.getRandomInt(0, data.mothers.length - 1)
                    this.similarityItem.Index = CharacterCreator.getRandomInt(0, 100)
                    this.skinSimilarityItem.Index = CharacterCreator.getRandomInt(0, 100)
                    this.updateParents()
                    break

                default:
                    break
            }
        })

        // Charcreator => Parents 2
        this.creatorParentsMenu.ListChange.on(() => {
            this.updateParents()
        })

        // register menus
        this.creatorParentsMenu.ParentMenu = this.creatorMainMenu
        this.creatorParentsMenu.Visible = false
        this.creatorMenus.push(this.creatorParentsMenu)
    }

    initFeaturesMenu() {
        this.creatorFeaturesMenu = new NativeUI.Menu('Gesicht', '', this.menuPoint)
        this.creatorFeaturesMenu.Visible = false

        // Charcreator => Features 1
        this.creatorFeaturesMenu.ItemSelect.on((item) => {
            switch (item.Text) {
                case 'Randomize':
                    for (let i = 0; i < data.featureNames.length; i++) {
                        this.featureItems[i].Index = CharacterCreator.getRandomInt(0, 200)
                        this.updateFaceFeature(i)
                    }
                    break

                case 'Zurücksetzen':
                    this.resetFeaturesMenu()
                    break

                default:
                    break
            }
        })

        // Charcreator => Features 2
        this.creatorFeaturesMenu.ListChange.on((item) => {
            this.updateFaceFeature(this.featureItems.indexOf(item))
        })

        for (let i = 0; i < data.featureNames.length; i++) {
            const tempFeatureItem = new NativeUI.UIMenuListItem(
                data.featureNames[i],
                '',
                new NativeUI.ItemsCollection(this.features),
            )
            tempFeatureItem.Index = 100
            this.featureItems.push(tempFeatureItem)
            this.creatorFeaturesMenu.AddItem(tempFeatureItem)
        }

        this.creatorFeaturesMenu.AddItem(new NativeUI.UIMenuItem('Randomize', '~r~Zufällige Generierung.'))
        this.creatorFeaturesMenu.AddItem(new NativeUI.UIMenuItem('Zurücksetzen', '~r~Einstellung zurücksetzen.'))

        // register menus
        this.creatorFeaturesMenu.ParentMenu = this.creatorMainMenu
        this.creatorFeaturesMenu.Visible = false
        this.creatorMenus.push(this.creatorFeaturesMenu)
    }

    updateParents() {
        natives.setPedHeadBlendData(
            this.pedHandle,
            this.fatherItem.Index,
            this.motherItem.Index,
            0,
            this.fatherItem.Index,
            this.motherItem.Index,
            0,
            1.0 - this.similarityItem.Index / 100,
            1.0 - this.skinSimilarityItem.Index / 100,
            0,
            false,
        )
    }

    updateFaceFeature(index: number) {
        natives.setPedFaceFeature(this.pedHandle, index, this.featureItems[index].SelectedValue)
    }

    updateAppearance(index: number) {
        const overlayID = this.appearanceItems[index].Index === 0 ? 255 : this.appearanceItems[index].Index - 1
        // mp.players.local.setHeadOverlay(index, overlayID, appearanceOpacityItems[index].Index * 0.01, colorForOverlayIdx(index), 0);
        natives.setPedHeadOverlay(this.pedHandle, index, overlayID, this.appearanceOpacityItems[index].Index * 0.01)
    }

    updateHairAndColors() {
        // mp.players.local.setComponentVariation(2, hairList.hairList[currentGender][hairItem.Index].ID, 0, 2);
        natives.setPedComponentVariation(
            this.pedHandle,
            2,
            data.hairList[this.currentGender][this.hairItem.Index].ID,
            0,
            2,
        )
        natives.setPedHairColor(this.pedHandle, this.hairColorItem.Index, this.hairHighlightItem.Index)
        natives.setPedEyeColor(this.pedHandle, this.eyeColorItem.Index)
        natives.setPedHeadOverlayColor(this.pedHandle, 1, 1, this.beardColorItem.Index, 0) // maybe not working?
        natives.setPedHeadOverlayColor(this.pedHandle, 2, 1, this.eyebrowColorItem.Index, 0)
        natives.setPedHeadOverlayColor(this.pedHandle, 5, 2, this.blushColorItem.Index, 0)
        natives.setPedHeadOverlayColor(this.pedHandle, 8, 2, this.lipstickColorItem.Index, 0)
        natives.setPedHeadOverlayColor(this.pedHandle, 10, 1, this.chestHairColorItem.Index, 0)
    }

    resetParentsMenu(refresh = false) {
        this.fatherItem.Index = 0
        this.motherItem.Index = 0
        this.similarityItem.Index = this.currentGender === 0 ? 100 : 0
        this.skinSimilarityItem.Index = this.currentGender === 0 ? 100 : 0

        this.updateParents()
        if (refresh) this.creatorParentsMenu.RefreshIndex()
    }

    resetFeaturesMenu(refresh = false) {
        for (let i = 0; i < data.featureNames.length; i++) {
            this.featureItems[i].Index = 100
            this.updateFaceFeature(i)
        }

        if (refresh) this.creatorFeaturesMenu.RefreshIndex()
    }

    resetAppearanceMenu(refresh = false) {
        for (let i = 0; i < data.appearanceNames.length; i++) {
            this.appearanceItems[i].Index = 0
            this.appearanceOpacityItems[i].Index = 100
            this.updateAppearance(i)
        }

        if (refresh) this.creatorAppearanceMenu.RefreshIndex()
    }

    resetHairAndColorsMenu(refresh = false) {
        this.hairItem.Index = 0
        this.hairColorItem.Index = 0
        this.hairHighlightItem.Index = 0
        this.eyebrowColorItem.Index = 0
        this.beardColorItem.Index = 0
        this.eyeColorItem.Index = 0
        this.blushColorItem.Index = 0
        this.lipstickColorItem.Index = 0
        this.chestHairColorItem.Index = 0
        this.updateHairAndColors()

        if (refresh) this.creatorHairMenu.RefreshIndex()
    }

    static getRandomInt(min: number, max: number) {
        return Math.floor(Math.random() * (max - min + 1)) + min
    }

    show(callback: (appearanceData: any) => void) {
        if (this.creatorMainMenu.Visible) return false

        this.onCloseCallback = callback

        this.createPed('mp_m_freemode_01')

        this.camera = new Camera(CharCreatorCamPos, new alt.Vector3(0, 0, 0), 50)
        this.camera.pointAtCoord(new alt.Vector3(CharCreatorPedPos.x, CharCreatorPedPos.y, CharCreatorPedPos.z + 1))
        this.camera.render()

        this.creatorMainMenu.Visible = true

        return true
    }

    createPed(model: string) {
        if (this.pedHandle) {
            natives.deleteEntity(this.pedHandle)
        }

        this.pedHandle = natives.createPed(2, alt.hash(model), CharCreatorPedPos.x, CharCreatorPedPos.y, CharCreatorPedPos.z, CharCreatorPedHeading, false, false)

        this.resetPedClothesToDefault()
        this.updateParents()

        //natives.setEntityCoords(this.pedHandle, CharCreatorPedPos.x, CharCreatorPedPos.y, CharCreatorPedPos.z - 1, true, false, false, true)
        //natives.setEntityHeading(this.pedHandle, CharCreatorPedHeading)
        natives.setEntityInvincible(this.pedHandle, true)
        natives.freezeEntityPosition(this.pedHandle, true)
    }

    hide() {
        if (this.pedHandle) {
            natives.deleteEntity(this.pedHandle);
        }

        this.camera.destroy()

        natives.displayRadar(true)
        natives.doScreenFadeOut(0)

        this.creatorMainMenu.Visible = false
        this.resetAppearanceMenu()
        this.resetFeaturesMenu()
        this.resetParentsMenu()
    }

    isVisible() {
        return this.creatorMainMenu.Visible
    }
}

export default CharacterCreator