<template>
  <!-- begin::Page loader -->
  <div class="page-loader page-loader-logo">
    <div class="spinner" v-bind:class="spinnerClass || 'spinner-primary'"></div>
  </div>
  <!-- end::Page Loader -->
</template>

<script>
export default {
  name: "Loader",
  props: {
    spinnerClass: String
  }
};
</script>
