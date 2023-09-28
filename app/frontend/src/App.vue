<script setup>
import store from './store';

let jwtToken = window.$cookies.get('jwt');
store.commit('updateJwtToken', jwtToken);
</script>

<template>
  <div class="container min-vh-100 d-flex flex-column">
    <header class="row pb-3 mb-4 col border-bottom mt-4 navbar justify-content-between m-1">
      <a href="/" class="d-flex align-items-center navbar-brand text-body-emphasis text-decoration-none d-inline w-25">
        <i class="fa-regular fa-square-check mr-4 pr-4" style="font-size: 2em;"></i>
        <span class="fs-4">&nbsp;&nbsp;PYCO TODO</span>
      </a>
      <a href="http://localhost:5000/" class="btn btn-danger text-center text-wrap d-inline col-2">Log Out</a>
    </header>
    <router-view class="m-1 p-1"/>
    <footer class="text-center pt-3 text-body-secondary border-top">
    <p>
      &copy; 2023
    </p>
  </footer>
  </div>
</template>

<style>
body,
html {
  background-color: #EEEEEE;
}

#siteTitle{
  color: #EEEEEE;
}
.pycobar{
  background-color: #757575;
}
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  background-color: #EEEEEE;
  background-image: url('assets/space-bg.png');
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
