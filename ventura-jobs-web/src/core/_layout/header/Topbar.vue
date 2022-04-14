<template>
  <!-- begin:: Header Topbar -->
  <div class="topbar">
    <!--begin: Search -->
    <b-dropdown
      v-if="searchDisplay"
      size="sm"
      id="kt_quick_search_toggle"
      variant="link"
      toggle-class="topbar-item text-decoration-none"
      no-caret
      right
      no-flip
    >
      <template v-slot:button-content>
        <div
          class="btn btn-icon btn-hover-transparent-white btn-lg btn-dropdown mr-1"
        >
          <span class="svg-icon svg-icon-xl">
            <inline-svg
              src="https://static-blue.ativainvestimentos.com.br/v2/media/svg/icons/General/Search.svg"
            />
          </span>
        </div>
      </template>
      <b-dropdown-text tag="div" class="min-w-md-350px">
        <SearchDefault></SearchDefault>
      </b-dropdown-text>
    </b-dropdown>
    <!--end: Search -->

    <div class="d-flex" id="app-host-toolbar">
      <!--begin: Quick panel toggle -->
      <MenuAplicacoes></MenuAplicacoes>
      <!--end: Quick panel toggle -->

      <!--begin: User Menu -->
      <b-dropdown
        size="sm"
        variant="menu"
        toggle-class="topbar-item text-decoration-none"
        no-caret
        right
        no-flip
      >
        <template v-slot:button-content>
          <div class="btn btn-icon btn-hover-transparent-white d-flex align-items-center btn-lg px-md-2 w-md-auto">
            <span class="symbol symbol-35">
              <span class="symbol-label text-white font-size-h5 font-weight-bold bg-white-o-30">
                {{ currentUserProfile.initials }}
              </span>
            </span>
          </div>
        </template>
        <b-dropdown-text tag="div" class="min-w-md-350px">
          <form>
            <DropdownUsuario></DropdownUsuario>
          </form>
        </b-dropdown-text>
      </b-dropdown>
      <!--end: User Menu -->
    </div>


  </div>
  <!-- end:: Header Topbar -->
</template>

<style lang="scss">
.topbar {
  .dropdown-toggle {
    padding: 0;
    &:hover {
      text-decoration: none;
    }

    &.dropdown-toggle-no-caret {
      &:after {
        content: none;
      }
    }
  }

  .dropdown-menu {
    margin: 0;
    padding: 0;
    outline: none;
    .b-dropdown-text {
      padding: 0;
    }
  }
}
</style>

<script>
import { mapGetters } from 'vuex'
import SearchDefault from "@/app/_defaultComponents/SearchDefault.vue";
import DropdownUsuario from "@/core/_layout/extras/dropdown/DropdownUsuario.vue";
import MenuAplicacoes from "@/core/_layout/extras/offcanvas/MenuAplicacoes.vue";

export default {
  name: "KTTopbar",
  components: {
    SearchDefault,
    DropdownUsuario,
    MenuAplicacoes
  },
  computed: {
    ...mapGetters([
      'currentUserProfile',
      'layoutConfig'
    ]),
    searchDisplay() {
      return !!this.layoutConfig("search.display");
    },
  }
};
</script>
