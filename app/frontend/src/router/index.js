import { createRouter, createWebHashHistory } from 'vue-router'

const routes = [
  {
    path: '/',
    name: 'todo',
    component: () => import(/* webpackChunkName: "about" */ '../views/ListOverview.vue')
  }
]

const router = createRouter({
  history: createWebHashHistory(),
  routes
})

export default router
