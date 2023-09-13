<script setup>
import { ref, onMounted } from 'vue';
import store from '@/store';
import ListAdminVue from '../components/ListAdmin.vue'
import TodoListComponent from '@/components/TodoListComponent.vue';
import { todo_get } from '@/todoclient';
import routes from '@/constants/todoroutes'

const dataReady = ref(false);
let showAdminView = ref(false);
let showTodoView = ref(false)
let adminComponentKey = ref(0)
let todoComponentKey = ref(0)
const loggedInUser = ref(store.state.user.username)
let listTitle = ref('')
let todoListId = ref(null)
let listUsers = ref([])
let listItems = ref([])
let createView = ref(false)
let todoList = ref([])

onMounted( async ()=> {
  todoList.value = await todo_get(routes.LIST_USER)
  
  if(todoList.value.length){
        for(let list in todoList.value){
            let date = formatDate(todoList.value[list].creationDate)
            todoList.value[list].creationDate = date
        }
    }
  console.log(todoList)
  dataReady.value = true;
})

async function openAdminView(listId){
  if(listId){
        let list = await todo_get(routes.LIST, { listId: listId })

        listTitle.value = list.title
        listUsers.value = list.listUsers
        todoListId.value = listId
        createView.value = false
      }else {
        let user = store.state.user
        listUsers.value = []
        listUsers.value.push({ id: user.id, username: user.username, listUserRole: 1 });
        listTitle.value = "New Todo List"
        createView.value = true
      }
      showAdminView.value = true;
      forceRerenderer(adminComponentKey)
}

function forceRerenderer(key){
  key.value += 1
}

async function openTodoList(listId){
  console.log('We are opening')
  let list = await todo_get(routes.LIST, { listId: listId })
  if(list){
    listTitle.value = list.title
    listUsers.value = list.listUsers
    listItems.value = list.listItems
    todoListId.value = listId
    showAdminView.value = false
    showTodoView.value = true
    forceRerenderer(todoComponentKey)
  }
  console.log(list)
}

function formatDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2) 
        month = '0' + month;
    if (day.length < 2) 
        day = '0' + day;

    return [year, month, day].join('-');
}
</script>
<template>
  <div class="row flex-grow-1 h-75 mb-4 rounded-3" style="background-color: white;">
    <div class="container-fluid flex-column d-flex">
      <div class="row flex-grow-1">
        <div class="col-4">
          <div class="row border-end border-bottom rounded-top-3 rounded-end-0 p-3 text-light"
            style="background-color: #54B4D3;">{{ loggedInUser }}</div>
          <div  v-if="dataReady">
            <ul class="list-group">
              <li v-for="todo in todoList" v-bind:key="todo.id">
                <div class="row p-4 list-group-item flex-column align-items-start rounded-3 list-overview-item" @click="openTodoList(todo.id)">
                  <div class="d-flex justify-content-between">
                    <h5 class="mb-1">{{ todo.title }}</h5>
                    <button type="button" class="btn btn-outline-secondary p-2 w-25 float-right" @click.stop="openAdminView(todo.id)"><i class="fa-solid fa-gear"></i></button>
                  </div>
                  <small>{{ todo.creationDate }}</small>
                </div>               
              </li>
            </ul>
          </div>
          <hr />
          <button type="button" class="btn btn-success mb-4 w-100" @click="openAdminView(null)">Neue Todo Liste erstellen</button>
        </div>
        <div class="col-md-auto flex-grow-1 pd-2">
          <ListAdminVue v-if="showAdminView" :key="adminComponentKey" :list-title="listTitle" :list-users="listUsers" :new-list="createView" :list-id="todoListId"/>
          <TodoListComponent v-else-if="showTodoView" :key="todoComponentKey" :list-title="listTitle" :list-users="listUsers" :list-items="listItems" :list-id="todoListId"/>
          <div v-else class="row d-flex align-items-center justify-content-center h-100">
            <div>
              <div class="row align-items-center justify-content-center">
                <h1 class="p-2 m-2 rounded-2">Willkommen, {{ loggedInUser }}!</h1>
                <button id="newListBig" class="p-2 m-2 rounded-2 w-25 btn btn-light" @click="openAdminView(null)"><i class="fa-solid fa-list-check" style="font-size: 10em;"></i><p>Create a new Todo List!</p></button>
                <button id="newListBig" class="p-2 m-2 rounded-2 w-25 btn btn-light"><i class="fa-solid fa-right-from-bracket" style="font-size: 10em;"></i><p>Logout</p></button>
              </div>       
            </div>         
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
ul {
  list-style-type: none;
}
.list-overview-item:hover {
  background-color:rgb(226, 226, 226);
  cursor: pointer;
  color: #494949 !important;
  border: 0.1em solid;
  border-radius: 50px;
  border-color: #494949 !important;
  transition: all 0.3s ease 0s;
}


</style>