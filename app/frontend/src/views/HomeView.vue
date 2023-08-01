<template>
  <div class="home">
    <img alt="Vue logo" src="../assets/logo.png">
    <h2>{{ users }}</h2>
    <HelloWorld msg="Welcome to Your Vue.js App" />
  </div>
</template>

<script>
// @ is an alias to /src
import { inject } from 'vue'
import HelloWorld from '@/components/HelloWorld.vue'

export default {
  name: 'HomeView',
  components: {
    HelloWorld
  },
  data() {
    return {
      users: 0,
    };
  },
  async mounted() {
    const axios = inject('axios');
    axios
      .get("https://localhost:7098/api/v1/Test")
      .then((response) => {
        this.users = response.data;
      })
      .catch((error) => {
        console.log(error);
      });
  }
}
</script>
