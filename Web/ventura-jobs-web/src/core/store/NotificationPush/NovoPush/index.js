import NovoPushService from '@/app/pages/PushNotifications/NovoPush/NovoPush.service'

export const ENVIAR_MENSAGEM = 'ENVIAR_MENSAGEM';


const state = {
    erro: {}
}

const getters = {}

const actions = {
    async enviarMensagem({ commit }, payload) {
        await NovoPushService.EnviarMensagem(payload.mensagem).then( () => {
            commit(ENVIAR_MENSAGEM, null)
        }, (reason) => {
            commit(ENVIAR_MENSAGEM, reason)
        })
    }
}

const mutations = {
    [ENVIAR_MENSAGEM](state, erro) {
        state.erro = erro;
    }
}

export default {
    namespaced: true,
    state,
    getters,
    actions,
    mutations
}