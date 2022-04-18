<template>
  <v-container fluid >
    <v-row dense>
      <v-col v-for="job in jobs" :key="job.id" :cols="flex">
        <v-card max-width="500" min-height="170" max-height="250" class="card card__utils" :to="`jobs/${job.id}`">
          <v-card-title>{{ job.title.slice(0, 20) }}</v-card-title>
          <v-card-text>{{ job.body.slice(0, 35) }}...</v-card-text>
        </v-card>
      </v-col>
    </v-row>
    <v-pagination v-model="page" :length="page_number" total-visible="5" @input="onChange"></v-pagination>
  </v-container>
</template>

<script>
import {GET_ALL_JOBS} from "~/store/jobs/mutationTypes"
import {mapActions, mapState} from 'vuex'

export default {
  name: 'jobs',
  head() {
    return {
      title: "Vagas"
    }
  },
  computed: mapState({
    jobs: state => state.jobs.jobs
  }),
  beforeMount() {
    this.getJobs()
  },
  data() {
    return {
      flex: 6,
      page: parseInt(this.$route.query.page, 10) || 1,
      page_number: null,
      loader: null,
    }
  },
  mounted() {
    this.unsub = this.$store.subscribe((mutation, state) => {
      if (mutation.type === `jobs/${GET_ALL_JOBS}`) {
        this.page_number = Math.round(this.jobs.length)
      }
    })
  },
  methods: {
    onChange() {
      this.$router.push({query: {page: this.page}});
      this.getJobs()
    },
    getJobs() {
      this.$store.dispatch({type: "jobs/getAllJobs", page: this.page})
    },
    ...mapActions(
      'jobs', [GET_ALL_JOBS]
    )
  },
  beforeDestroy() {
    this.unsub()
  },
  getJob(id) {
    this.$router.push({path: `/jobs/${id}`})
  }
}
</script>
<style>
.card__utils:hover {
  text-decoration: none !important;
  transform: scale(1.05, 1.05);
  transition: 1s all;
}

.card__utils {
  transition: 1s all;
  cursor: pointer;
  margin-bottom: 10px;
}
</style>
