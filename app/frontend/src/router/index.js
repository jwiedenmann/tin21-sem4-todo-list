import { createRouter, createWebHashHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'

const routes = [
  {
    path: '/',
    name: 'home',
    component: HomeView
  },
  {
    path: '/about',
    name: 'about',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/AboutView.vue')
  },
  {
    path: '/lists',
    name: 'lists',
    component: () => import(/* webpackChunkName: "about" */ '../views/ListOverview.vue')
  },
  {
    path: '/ListDetal',
    name: 'ListDetal',
    component: () => import(/* webpackChunkName: "about" */ '../components/TodoListComponent.vue')
  }
]

const router = createRouter({
  history: createWebHashHistory(),
  routes
})

export default router
