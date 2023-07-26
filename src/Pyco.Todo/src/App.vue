<template>
<div>
    <nav class="navbar navbar-expand-lg navbar-dark">
        <div class="container">
            <router-link class="navbar-brand" to="/" exact>
                <span class="align-middle" id="PycoLogo">Pyco.Todo</span>
            </router-link>
            <navbar :items="navItems" :attributes="store.userAttributes" />
        </div>
    </nav>

    <div id="content" class="container mt-4">
      <router-view></router-view>
    </div>
</div>
</template>

<script lang="ts">
import Vue from 'vue';
import { Component, Prop } from 'vue-property-decorator';
import { bus, store } from './shared';

const navItems = [
  { "href":"/",         "label": "Home",     "exact":true },
  { "href":"/signin",   "label": "Sign In",  "hide":"auth" },
  { "href": "/profile", "label": "Profile", "show": "auth" },
  { "href": "/lists", "label": "My Todos", "show": "auth" },
  { "href": "/admin", "label": "Admin", "show": "role:Admin" },
  { "href": "/about", "label": "About" }
]

@Component
export class App extends Vue {
  get store() { return store }
  get navItems() { return navItems }
}
export default App
</script>
