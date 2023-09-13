import { createRouter, createWebHashHistory } from 'vue-router'

const routes = [
  {
    path: '/',
    name: 'todo',
    component: () => import(/* webpackChunkName: "about" */ '../views/ListOverview.vue')
  },
  {
    path: '/about',
    name: 'about',
    component: () => import(/* webpackChunkName: "about" */ '../views/AboutView.vue')
  },  
  {
    path: '/details',
    name: 'details',
    component: () => import(/* webpackChunkName: "about" */ '../views/ListDetail.vue')
  },
  {
    path: '/todoList',
    name: 'todoList',
    component: () => import(/* webpackChunkName: "about" */ '../components/TodoListComponent.vue')
  }
]

const router = createRouter({
  history: createWebHashHistory(),
  routes
})

export default router
