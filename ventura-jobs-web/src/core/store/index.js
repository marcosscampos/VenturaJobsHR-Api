import Vue from "vue";
import Vuex from "vuex";

import auth from "./auth.module";
import htmlClass from "./htmlclass.module";
import config from "./config.module";
import breadcrumbs from "./breadcrumbs.module";
import profile from "./profile.module";
import baseDeClientes from './NotificationPush/BaseDeClientes'
import historicoDeEnvios from './NotificationPush/HistoricoDeEnvios'
import novoPush from './NotificationPush/NovoPush'

Vue.use(Vuex);

export default new Vuex.Store({
  modules: {
    auth,
    htmlClass,
    config,
    breadcrumbs,
    profile,
    baseDeClientes,
    historicoDeEnvios,
    novoPush
  }
});
