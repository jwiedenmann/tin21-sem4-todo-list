<script setup>
import { ref, onMounted } from 'vue';
import store from '@/store';
import ListAdminVue from '../components/ListAdmin.vue'
import TodoListComponent from '@/components/TodoListComponent.vue';
import { todo_get } from '@/todoclient';
import routes from '@/constants/todoroutes'
import topics from '@/constants/todotopics'
const mqtt = require("precompiled-mqtt");
const dataReady = ref(false);
const statusMsg = ref('');
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

//mqtt connection to broker via websocket
let connection = {
        protocol: "ws",
        host: "localhost",
        // ws: 9001; mqtt: 1883
        port: 9001,
        endpoint: "/mqtt",
        // for more options, please refer to https://github.com/mqttjs/MQTT.js#mqttclientstreambuilder-options
        clean: true,
        connectTimeout: 30 * 1000, // ms
        reconnectPeriod: 4000, // ms
        clientId: "emqx_vue_" + Math.random().toString(16).substring(2, 8),
        // auth
        username: "user1",
        password: "1234",
}
const { protocol, host, port, endpoint, ...options } = connection;
        const connectUrl = `${protocol}://${host}:${port}${endpoint}`;
console.log(connectUrl)
let client = mqtt.connect(connectUrl, options)

client.on('connect', function () {
  // subscribe to a topic to receive message from it
  client.subscribe(topics.USER_TOPIC + store.state.user.id, function (err) {
    if (!err) {
      console.log('Connected and subscribed to topic: ' + topics.USER_TOPIC + store.state.user.id)
      // after connection is success - send hello:
      //client.publish(topics.USER_TOPIC + store.state.user.id, 'Hello mqtt from user:' + store.state.user.id)
    }
  })
})

// What should happen if I recive a message?
client.on('message', function (topic, message) {
  // message is Buffer
  console.log("Received:" + message.toString())
  if(topic.startsWith(topics.USER_TOPIC)){
    window.location.reload()
  }
  // to end the connection:
  //client.end()
})

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
  let role = 0
  if(listId){
        let list = await todo_get(routes.LIST, { listId: listId })

        listTitle.value = list.title
        listUsers.value = list.listUsers
        todoListId.value = listId
        createView.value = false

        var resultIsAdmin = list.listUsers.find(user => user.id === parseInt(store.state.user.id))
        role = resultIsAdmin.listUserRole
      }else {
        let user = store.state.user
        listUsers.value = []
        listUsers.value.push({ id: user.id, username: user.username, listUserRole: 1 });
        listTitle.value = "New Todo List"
        createView.value = true
      }
      if(role == 1 || listId == null){
        showAdminView.value = true;
        forceRerenderer(adminComponentKey)
        if(screen.width < 1400){
          const element = document.getElementById('admin-element');
          console.log("Screen Width: " + screen.width)
          element.scrollIntoView({ behavior: 'smooth' });
        }
      }else{
        alert("Only an admin has the permission to to this")
      }
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
    if(screen.width < 1400){
      const element = document.getElementById('admin-element');
      console.log("Screen Width: " + screen.width)
      element.scrollIntoView({ behavior: 'smooth' });
    }
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
  <div class="row flex-grow-1 h-75 mb-4 rounded-4" style="background-color: white;">
    <div v-if="statusMsg" class="alert alert-primary" role="alert">
      {{ statusMsg }}
    </div>
    <div class="container-fluid flex-column d-flex">
      <div class="row flex-grow-1">
        <div class="col-xxl-4">
          <div class="row border-end border-bottom rounded-3 p-3 text-light"
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
          <hr class="d-md-none"/>
        </div>
        <div class="col-auto d-flex flex-grow-1 flex-column pd-1 justify-content-center" id="admin-element">
          <ListAdminVue v-if="showAdminView" :key="adminComponentKey" :list-title="listTitle" :list-users="listUsers" :new-list="createView" :list-id="todoListId"/>
          <TodoListComponent v-else-if="showTodoView" :key="todoComponentKey" :list-title="listTitle" :list-users="listUsers" :list-items="listItems" :list-id="todoListId" @reload-todos="openTodoList" id="todoElement"/>
          <div v-else class="box row d-flex align-items-center justify-content-center">              
              <div class="align-items-center justify-content-center pb-1 mb-1">
                <h2 class="p-2 m-2 text-wrap">Willkommen, {{ loggedInUser }}!</h2>
                <div class="row row-cols-1 row-cols-md-2 g-4">
                  <div v-if="todoList.length" class="col">
                    <div class="card h-100">
                      <button class="card-body rounded-2 btn btn-light" @click="openAdminView(todoList[0].id)"><i class="fa-solid fa-list-check start-icon"></i><p>Open your first list</p></button>
                    </div>
                  </div>
                  <div v-if="todoList.length" class="col">
                    <div class="card h-100">
                      <button class="card-body rounded-2 btn btn-light" @click="openAdminView(todoList[todoList.length -1].id)"><i class="fa-regular fa-clock start-icon"></i><p>Open your most recent list</p></button>
                    </div>
                  </div>
                  <div class="col">
                    <div class="card h-100">
                      <button  class=" rounded-2 btn btn-light" @click="openAdminView(null)"><i class="fa-solid fa-plus start-icon"></i><p>Create a new Todo List!</p></button>
                    </div>
                  </div>
                  <div class="col">
                    <div class="card h-100">
                      <a href="http://localhost:5000/" class="rounded-2 btn btn-light"><i class="fa-solid fa-right-from-bracket start-icon"></i><p>Logout</p></a>
                    </div>
                  </div>
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

.start-icon{
  font-size: 5em;
}
</style>