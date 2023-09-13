<script setup>
import { ref, onMounted } from 'vue';
import store from '@/store';
import ListAdminVue from '../components/ListAdmin.vue'
import { todo_get } from '@/todoclient';
import routes from '@/constants/todoroutes'

const dataReady = ref(false);
let showAdminView = ref(false);
let componentKey = ref(0)
const loggedInUser = ref(store.state.user.username)
let listTitle = ref('')
let listUsers = ref([])
let todoDataList = ref([
          {
            "id": 1,
            "title": "Meine erste Todo",
            "creationDate": "2022-04-01",
            "role": "admin"
          },
          {
            "id": 2,
            "title": "Noch eine Todo",
            "creationDate": "2022-07-01",
            "role": "admin"
          },
          {
            "id": 3,
            "title": "Hausaufgaben",
            "creationDate": "2023-04-01",
            "role": "user"
          },
          {
            "id": 4,
            "title": "Bucket List",
            "creationDate": "2023-09-10",
            "role": "admin"
          },
          {
            "id": 5,
            "title": "Einkaufsliste",
            "creationDate": "2012-04-01",
            "role": "readOnly"
          }
        ])
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
        console.log(listUsers)
      }else {
        let user = store.state.user
        listUsers.value = []
        listUsers.value.push({ id: user.id, username: user.username, listUserRole: 1 });
        listTitle.value = "New Todo List"
      }
      showAdminView.value = true;
      forceRerenderer()
}

function forceRerenderer(){
  componentKey.value += 1
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
                <div class="row p-4 list-group-item flex-column align-items-start rounded-3" >
                  <div class="d-flex justify-content-between">
                    <h5 class="mb-1">{{ todo.title }}</h5>
                    <small>{{ todo.creationDate }}</small>
                    <button type="button" class="btn btn-outline-secondary p-2 w-25 float-right" @click="openAdminView(todo.id)"><i class="fa-solid fa-gear"></i></button>
                  </div>
                </div>               
              </li>
            </ul>
          </div>
          <hr />
          <button type="button" class="btn btn-success mb-4 w-100" @click="openAdminView(null)">Neue Todo Liste erstellen</button>
        </div>
        <div class="col-md-auto flex-grow-1 pd-2">
          <ListAdminVue v-if="showAdminView" :key="componentKey" :list-title="listTitle" :list-users="listUsers"/>
          <div v-else class="row d-flex align-items-center justify-content-center h-100">
            <div>
              <h1 class="p-2 m-2 rounded-2">Willkommen, {{ loggedInUser }}!</h1>
              <button id="newListBig" class="p-2 m-2 rounded-2 w-25 btn btn-light" @click="openAdminView(null)"><i class="fa-solid fa-list-check" style="font-size: 10em;"></i><p>Create a new Todo List!</p></button>
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
</style>