
<template>
  <div>
    <!--begin::Header-->
    <div class="d-flex align-items-center py-10 px-8 bgi-size-cover bgi-no-repeat rounded-top" :style="{ backgroundImage: backgroundImage }">
      <div class="symbol symbol-45 symbol-light mr-4">
        <span class="font-size-h3 symbol-label font-weight-boldest">
          {{ currentUserProfile.initials }}
        </span>
        <!-- <i class="symbol-badge bg-success"></i> -->
      </div>
      <h4 class="text-white m-0 flex-grow-1 mr-3">{{ currentUserProfile.fullName }}</h4>
      <!-- <button type="button" class="btn btn-success btn-sm">2 Items</button> -->
    </div>
    <!--end::Header-->

    <div class="p-8">
      <p class="my-0 font-weight-bold"> {{ currentUserProfile.username }} </p>
      <p class="my-0"> {{ currentUserProfile.email }} </p>
    </div>

    <!--begin::Nav-->
    <div class="navi">
      <!--begin::Footer-->
      <div class="navi-separator"></div>
      <div class="navi-footer px-8 py-5">
        <a href="#" class="btn btn-light-primary font-weight-bold" @click="onLogout">
          Sair
        </a>
      </div>
      <!--end::Footer-->
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import { LOGOUT } from "@/core/store/auth.module";

export default {
  name: "KTDropdownUser",
  methods: {
    onLogout() {
      this.$store
        .dispatch(LOGOUT);
    }
  },
  computed: {
    ...mapGetters([
      'currentUserProfile'
    ]),
    backgroundImage() {
      return "var(--user-menu-header-bg, url(https://static-blue.ativainvestimentos.com.br/v2/media/misc/bg-1.jpg))";
    }
  }
};
</script>
