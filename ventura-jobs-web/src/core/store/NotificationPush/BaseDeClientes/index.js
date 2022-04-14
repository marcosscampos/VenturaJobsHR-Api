import BaseDeClientesService from '@/app/pages/PushNotifications/BaseDeClientes/BaseDeClientes.service'

export const OBTER_CLIENTES = 'OBTER_CLIENTES'

const state = {
    clientes: [],
    erro: {}
}

const getters = {}

const actions = {
    async obterClientes({ commit }, payload) {
        await BaseDeClientesService.obterClientes(payload.filter).then(clientes => {
            commit(OBTER_CLIENTES, clientes)
        }, (reason) => {
            commit(OBTER_CLIENTES, reason)
        })
    }
}

const mutations = {
    [OBTER_CLIENTES](state, obj) {
        if(obj.errors != null || obj.statusCode == 500) {
            state.erro = obj.data
        } else {
            state.clientes = obj;
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