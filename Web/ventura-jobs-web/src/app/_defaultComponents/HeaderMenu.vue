<template>
  <ul class="menu-nav">
    <li
      aria-haspopup="true"
      data-menu-toggle="click"
      class="menu-item menu-item-submenu menu-item-open-dropdown"
      v-bind:class="{ 'menu-item-active': hasActiveChildren('/vue-bootstrap') }"
    >
      <a href="#" class="menu-link menu-toggle">
        <span class="menu-text"> Push Notifications </span>
      </a>
      <div class="menu-submenu menu-submenu-classic">
        <div class="menu-subnav">
          <ul class="menu-content">
            <li class="menu-item">
              <ul class="menu-inner">
                <router-link
                  to="/base-clientes"
                  v-slot="{ href, navigate, isActive, isExactActive }"
                >
                  <li
                    aria-haspopup="true"
                    class="menu-item"
                    :class="[
                      isActive && 'menu-item-active',
                      isExactActive && 'menu-item-active',
                    ]"
                  >
                    <a :href="href" class="menu-link" @click="navigate">
                      <span class="menu-text"> Base de Clientes </span>
                    </a>
                  </li>
                </router-link>
                <router-link
                  to=""
                  v-slot="{ href, navigate, isActive, isExactActive }"
                >
                  <li aria-haspopup="true" class="menu-item">
                    <a
                      :href="href"
                      class="menu-link"
                      @click="navigate"
                      v-b-modal="'novo-push-menu'"
                    >
                      <span class="menu-text"> Novo envio de Push </span>
                    </a>
                    <b-modal
                      id="novo-push-menu"
                      hide-footer
                      ref="modal-menu"
                      hide-header
                    >
                      <NovoPush :fechar-modal="fechaModal" />
                    </b-modal>
                  </li>
                </router-link>
                <router-link
                  to="/historico-envios"
                  v-slot="{ href, navigate, isActive, isExactActive }"
                >
                  <li
                    aria-haspopup="true"
                    class="menu-item"
                    :class="[
                      isActive && 'menu-item-active',
                      isExactActive && 'menu-item-active',
                    ]"
                  >
                    <a :href="href" class="menu-link" @click="navigate">
                      <span class="menu-text"> Histórico de Envios </span>
                    </a>
                  </li>
                </router-link>
              </ul>
            </li>
          </ul>
        </div>
      </div>
    </li>
  </ul>
</template>

<script>
import NovoPush from "../pages/PushNotifications/NovoPush/index";

export default {
  name: "KTMenu",
  components: { NovoPush },
  methods: {
    hasActiveChildren(match) {
      return this.$route["path"].indexOf(match) !== -1;
    },
    fechaModal() {
      this.$refs["modal-menu"].hide();
    },
  },
};
</script>
