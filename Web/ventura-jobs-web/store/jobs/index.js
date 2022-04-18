import * as Mutation from './mutationTypes';

const state = {
  jobs: [],
  job: {},
  error: {}
}

const getters = {}

const actions = {
  async getAllJobs({commit}, payload) {
    this.$axios.$get(`/posts?_page=${payload.page}`).then(response => {
      commit(Mutation.GET_ALL_JOBS, response)
    }, (reason) => {
      commit(Mutation.GET_ALL_JOBS, reason)
    })
  }
}

const mutations = {
  [Mutation.GET_ALL_JOBS](state, payload) {
    if (payload.errors != null || payload.statusCode === 500) {
      state.error = payload
    } else {
      state.jobs = payload
      state.error = null
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

