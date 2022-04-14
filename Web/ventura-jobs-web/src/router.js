import Vue from "vue";
import Router from "vue-router";

Vue.use(Router);

export default new Router({
  base: process.env.VUE_APP_BASE_URL,
  mode: 'history',
  routes: [
    {
      path: "/",
      redirect: "/base-clientes",
      component: () => import("@/core/_layout/Layout"),
      children: [
        {
          path: '/base-clientes',
          name: 'base-clientes',
          component: () => import("@/app/pages/PushNotifications/BaseDeClientes/index.vue")
        },
        {
          path: '/historico-envios',
          name: 'historico-envios',
          component: () => import("@/app/pages/PushNotifications/HistoricoDeEnvios/index.vue")
        }
      ]
    },
    {
      path: "*",
      redirect: "/404"
    },
    {
      // the 404 route, when none of the above matches
      path: "/404",
      name: "404",
      component: () => import("@/core/_layout/error/Error-404.vue")
    }
  ]
});
