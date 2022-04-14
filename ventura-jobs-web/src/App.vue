<template>
  <router-view></router-view>
</template>

<style lang="scss">
// 3rd party plugins css
@import "https://fonts.googleapis.com/css?family=Poppins:300,400,500,600,700|Roboto:300,400,500,600,700|Material+Icons";

@import "~bootstrap-vue/dist/bootstrap-vue.css";
@import "~perfect-scrollbar/css/perfect-scrollbar.css";
@import "~animate.css";

@import "https://static-blue.ativainvestimentos.com.br/v2/plugins/socicon/css/socicon.css";
@import "https://static-blue.ativainvestimentos.com.br/v2/plugins/fontawesome/css/all.css";
@import "https://static-blue.ativainvestimentos.com.br/v2/plugins/line-awesome/css/line-awesome.css";
@import "https://static-blue.ativainvestimentos.com.br/v2/plugins/flaticon/flaticon.css";
@import "https://static-blue.ativainvestimentos.com.br/v2/plugins/flaticon2/flaticon.css";
@import "https://static-blue.ativainvestimentos.com.br/v2/plugins/keenthemes-icons/font/ki.css";

// Main demo style scss
@import "assets/sass/style.vue";

@import "assets/sass/whitelabel";

</style>

<script>
import { OVERRIDE_LAYOUT_CONFIG } from "@/core/store/config.module";

export default {
  mounted() {
    /**
     * this is to override the layout config using saved data from localStorage
     * remove this to use config only from static json (@/core/config/layout.config.json)
     */
    this.$store.dispatch(OVERRIDE_LAYOUT_CONFIG);
  }
};
</script>
