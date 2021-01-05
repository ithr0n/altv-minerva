import * as alt from 'alt-client'
import * as natives from 'natives'
import KeyCodes from './Utils/KeyCodes'

let tvSpawned = false
let tvView: alt.WebView = null
let tvViewObject = -1

alt.on('keyup', (key: KeyCodes) => {
    if (key === KeyCodes.VK_F10) {
        if (!tvSpawned) {
            let pos = alt.Player.local.pos
            tvViewObject = natives.createObject(alt.hash('gr_prop_gr_trailer_tv'), pos.x, pos.y, pos.z, false, false, true)
            //alt.log('exist = ' + alt.isTextureExistInArchetype(alt.hash('gr_prop_gr_trailer_tv'), 'script_rt_gr_trailertv_01'))
            let inter = alt.setInterval(() => {
                if (alt.isTextureExistInArchetype(alt.hash('gr_prop_gr_trailer_tv'), 'script_rt_gr_trailertv_01')) {
                    tvView = new alt.WebView('https://www.youtube.com/watch?v=K9KWUUKcgw0', alt.hash('gr_prop_gr_trailer_tv'), 'script_rt_gr_trailertv_01')
                    alt.clearInterval(inter);
                    return;
                }
            }, 50)
        } else {
            if (tvView && tvViewObject > 0) {
                tvView.destroy()
                natives.deleteObject(tvViewObject)
                tvView = null
            }
        }
    }
})
