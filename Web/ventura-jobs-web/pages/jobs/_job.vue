<template>
  <v-container>
    <v-card>
      <div>
        <Nuxt-Link to="/jobs" class="arrow__utils">
            <v-btn icon height="50" width="50" text="oi">
              <v-icon>mdi-chevron-left</v-icon>
            </v-btn>
        </Nuxt-Link>
      </div>
      <v-card-title>{{ title }}</v-card-title>
      <v-card-text>{{ body }}</v-card-text>
    </v-card>

  </v-container>
</template>

<script>

export default {
  name: 'job',
  async asyncData({$axios, route}) {
    const id = route.params.job
    let data = await $axios.$get(`/posts/${id}`)
    return {title: data.title, body: data.body, userId: id}
  }
}

</script>

<style scoped>
.arrow__utils:hover {
  text-decoration: none !important;
}
</style>
