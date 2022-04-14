// action types
export const LOGOUT = "logout";

// mutation types
export const PURGE_AUTH = "logOut";

const state = {
  errors: null,
  isAuthenticated: !!sessionStorage.authToken
};

const getters = {
  isAuthenticated(state) {
    return state.isAuthenticated;
  }
};

const actions = {
  [LOGOUT](context) {
    context.commit(PURGE_AUTH);
    window.blue ? window.blue.auth.logout() : void(0);
  },
};

const mutations = {
  [PURGE_AUTH](state) {
    state.isAuthenticated = false;
    state.errors = {};
  }
};

export default {
  state,
  actions,
  mutations,
  getters
};
