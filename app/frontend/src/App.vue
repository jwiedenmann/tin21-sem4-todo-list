<script setup>
import store from './store';

let jwtToken = window.$cookies.get('jwt');
store.commit('updateJwtToken', jwtToken);
</script>

<template>
  <div class="container min-vh-100 d-flex flex-column">
    <header class="row pb-3 mb-4 border-bottom">
      <a href="/" class="d-flex align-items-center text-body-emphasis text-decoration-none">
        <span class="fs-4">PYCO TODO</span>
      </a>
    </header>
    <router-view />
  </div>
</template>

<style>
body,
html {
  background-color: snow;
}

#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  background-color: snow;
}

nav {
  padding: 30px;
}

nav a {
  font-weight: bold;
  color: #2c3e50;
}

nav a.router-link-exact-active {
  color: #42b983;
}
</style>
