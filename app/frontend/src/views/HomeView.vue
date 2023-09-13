<template>
  <div class="home">
    <img alt="Vue logo" src="../assets/logo.png">
    <br>
    <button class="btn btn-primary" data-bs-target="#collapseTarget" data-bs-toggle="collapse">
      Bootstrap collapse
    </button>
    <div class="collapse py-2" id="collapseTarget">
      This is the toggle-able content!
    </div>
    <br>
    <h2>{{ users }}</h2>
    <HelloWorld msg="Welcome to Your Vue.js App" />
  </div>
</template>

<script>
// @ is an alias to /src
import HelloWorld from '@/components/HelloWorld.vue'
import { todo_get } from '@/todoclient';
import routes from '@/constants/todoroutes'

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
    await todo_get(routes.LIST, { listId: 1 })
  }

}
</script>
