<template>
  <div class="topbar-item">
    <div
      class="btn btn-icon btn-hover-transparent-white btn-lg mr-1"
      id="kt_quick_panel_toggle"
    >
      <span class="svg-icon svg-icon-xl">
        <inline-svg
          src="https://static-blue.ativainvestimentos.com.br/v2/media/svg/icons/Layout/Layout-4-blocks.svg"
        />
      </span>
    </div>

    <!-- begin::Quick Panel -->
    <div
      id="kt_quick_panel"
      ref="kt_quick_panel"
      class="offcanvas offcanvas-right pt-5 pb-10"
      style="overflow: hidden"
    >
      <!--begin::Header-->
      <div
        class="offcanvas-header offcanvas-header-navs d-flex align-items-center justify-content-between mb-5"
      >
        <div class="offcanvas-close mt-n1 pr-5">
          <a
            href="#"
            class="btn btn-xs btn-icon btn-light btn-hover-primary"
            id="kt_quick_panel_close"
          >
            <i class="ki ki-close icon-xs text-muted"></i>
          </a>
        </div>
      </div>
      <!--end::Header-->
      <div class="offcanvas-content">
        <ul
          class="nav nav-bold nav-tabs nav-tabs-line nav-tabs-line-3x nav-tabs-primary flex-grow-1 px-5"
          role="tablist"
        >
          <li class="nav-item">
            <a
              class="nav-link active"
              v-on:click="setActiveTab"
              data-tab="0"
              data-toggle="tab"
              href="#"
              role="tab"
              aria-selected="true"
            >
              Aplicações Blue
            </a>
          </li>
        </ul>
        <div class="tab-content">
          <div
            class="scroll pr-7 mr-n7"
            id="kt_quick_panel_notifications"
            v-if="aplicacoesDisponiveis.length === 0"
          >
            <!--begin::Nav-->

            <div class="d-flex align-items-center rounded p-5 mt-10 mb-5 bg-light-warning">
              <span class="svg-icon mr-5 svg-icon-warning"></span>

              <div class="d-flex flex-column flex-grow-1 mr-2">
                <p class="font-weight-normal text-dark-75 font-size-lg mb-1 mt-0">
                  Nenhuma aplicação disponível
                </p>
                <span class="text-muted font-size-sm">
                  Verifique seus acessos com o time de Governança pelo
                  e-mail
                  <a href="mailto:gti@ativainvestimentos.com.br?subject=Solicitação de acesso">gti@</a>
                </span>
              </div>

              <span class="font-weight-bolder py-1 font-size-lg text-warning" style="white-space: nowrap">
                :(
              </span>
            </div>
          </div>

          <div
            class="mt-5"
            id="kt_quick_panel_logs"
            v-if="aplicacoesDisponiveis.length > 0"
          >
            <div class="navi navi-hover navi-icon-circle navi-spacer-x-0">
              <template v-for="(app, index) in aplicacoesDisponiveis">
                <!--begin::Item-->
                <div
                  class="navi-separator"
                  v-bind:key="`blue-vue-template-${app.id}`"
                  v-if="index !== 0"
                ></div>
                <a
                  href="javascript:void(0)"
                  class="navi-item"
                  v-bind:key="app.id"
                  v-on:click="carregarAplicacao(app)"
                >
                  <div class="navi-link px-5">
                    <!-- <div class="symbol symbol-50 mr-3">
                      <div class="symbol-label">
                        <i class="icon-lg" v-bind:class="item.icon" />
                      </div>
                    </div> -->
                    <div class="navi-text">
                      <div class="font-size-lg">
                        {{ app.displayName }}
                      </div>
                      <div class="text-muted">
                        {{ app.description }}
                      </div>
                    </div>
                    <span class="py-1 font-size-sm">
                      <i class="las la-angle-right"></i>
                    </span>
                  </div>
                </a>
                <!--end::Item-->
              </template>
            </div>
          </div>
        </div>
      </div>
    </div>
    <!-- end::Quick Panel -->
  </div>
</template>

<script>
import KTLayoutQuickPanel from "@/assets/js/layout/extended/quick-panel.js";

export default {
  name: "KTQuickPanel",
  data() {
    return {
      tabIndex: 0,
    };
  },
  computed: {
    aplicacoesDisponiveis: () => {
      var aplicacoes = [];

      if (!window.blue) {
        return [];
      }

      window.blue.apps.forEach((app) => {
        var roles = window.blue.auth
          .getRoles()
          .filter((item) => item.startsWith(app.rolePrefix));
        if (roles.length > 0) {
          aplicacoes.push(app);
        }
      });
      return aplicacoes;
    },
  },
  mounted() {
    // Init Quick Offcanvas Panel
    KTLayoutQuickPanel.init(this.$refs["kt_quick_panel"]);
  },
  methods: {
    /**
     * Set current active on click
     * @param event
     */
    setActiveTab(event) {
      const tab = event.target.closest('[role="tablist"]');
      const links = tab.querySelectorAll(".nav-link");
      // remove active tab links
      for (let i = 0; i < links.length; i++) {
        links[i].classList.remove("active");
      }

      // set clicked tab index to bootstrap tab
      this.tabIndex = parseInt(event.target.getAttribute("data-tab"));

      // set current active tab
      event.target.classList.add("active");
    },
    carregarAplicacao: function (app) {
      window.blue.router.redirect(app);
    },
  },
};
</script>
