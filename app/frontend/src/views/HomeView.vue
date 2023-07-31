<template>
  <div class="home">
    <img alt="Vue logo" src="../assets/logo.png">
    <h2>{{ users }}</h2>
    <HelloWorld msg="Welcome to Your Vue.js App" />
  </div>
</template>

<script>
// @ is an alias to /src
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
    const { data } = await this.axios.get(
      "https://localhost:7098/api/v1/Test"
    ).then((r) => {
      console.log(r.data);
      this.albero = r.data;
    }).catch((e) => {
      console.log(e);
    });;
    this.users = data;
  }
}
</script>
