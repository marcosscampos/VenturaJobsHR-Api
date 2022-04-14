import HistoricoDeMensagensService from '@/app/pages/PushNotifications/HistoricoDeEnvios/HistoricoDeEnvios.service'

export const OBTER_MENSAGENS = 'OBTER_MENSAGENS'

const state = {
    mensagens: [],
    erro: {}
}

const getters = {}

const actions = {
    async obterMensagensEnviadas({ commit }) {
        await HistoricoDeMensagensService.obterMensagensEnviadas().then( mensagens => {
            commit(OBTER_MENSAGENS, mensagens)
        })
    }
}

const mutations = {
    [OBTER_MENSAGENS](state, obj) {
        if(obj.errors != null || obj.statusCode == 500) {
            state.erro = obj.data
        } else {
            state.mensagens = obj;
            state.erro = null;
        }
    }
}

export default {
    namespaced: true,
    state,
    getters,
    actions,
    mutations
}