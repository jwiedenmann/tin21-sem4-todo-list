<script setup>
// ----- IMPORTS -----

import { onMounted, ref, defineProps, defineEmits } from 'vue'
import { todo_get, todo_post, todo_put } from '@/todoclient'
import routes from '@/constants/todoroutes'
import store from '@/store';
import topics from '@/constants/todotopics'
const mqtt = require("precompiled-mqtt");

// ----- CONSTS -----

const props = defineProps({
    listTitle: {
        type: String,
        required: true
    },
    listId: Number,
    listUsers: Array,
    listItems: Array
})

const currentUser = ref('')
const currentUserId = ref(0)
const currentListId = ref(0)
const currentRole = ref(0)
const task = ref('')
let editedTaskId = null
let editedTaskIdinDB = null
let TaskIdCunt = 0
let componentKey = ref(0)
const TodoName = ref('')

const Tasks = ref([])
const Users = ref([])

const emit = defineEmits(['reloadTodos'])

//mqtt stuff
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
  client.subscribe(topics.LIST_TOPIC + props.listId, function (err) {
    if (!err) {
      console.log('Connected and subscribed to topic: ' + topics.LIST_TOPIC + props.listId,)
    }
  })
  client.subscribe(topics.SERVER_ACK.replace('{listID}', props.listId), function(err){
    if (!err) {
      console.log('Connected and subscribed to topic: ' + topics.LIST_TOPIC + props.listId,)
    }else {
        console.log(err)
    }
  })
})

// What should happen if I recive a message?
client.on('message', function (topic, message) {
  // message is Buffer
  console.log("Received:" + message.toString())
  if(topic.startsWith(topics.LIST_TOPIC)){
    emit('reloadTodos', props.listId)
  }
  // to end the connection:
  //client.end()
})

const forceRerender = () => {
    console.log('Rerender')
  componentKey.value += 1;
};


onMounted(async () => {
    currentUser.value = store.state.user.username
    currentUserId.value = parseInt(store.state.user.id)
    currentListId.value = props.listId
    let todoListItems = props.listItems
    TodoName.value = props.listTitle
    let todoListUsers = props.listUsers
    var resultIsAdmin = props.listUsers.find(user => user.id === currentUserId.value)
    currentRole.value = resultIsAdmin.listUserRole

    for (let i = 0; i < todoListItems.length; i++) {
        let checkedByCurrentUser = false
        if (todoListItems[i].checkedByUserIds.indexOf(currentUserId.value) >= 0) checkedByCurrentUser = true;

        console.log(todoListItems[i].content);
        Tasks.value.push({
            id: todoListItems[i].id,
            idInFrontendList: TaskIdCunt++,
            Content: todoListItems[i].content,
            checkedSum: todoListItems[i].checkedByUserIds.length,
            isCheckedByCurrentUser: checkedByCurrentUser
        })
    }

    for (let i = 0; i < props.listUsers.length; i++) {
        console.log(props.listUsers[i].username);
        Users.value.push({
            id: props.listUsers[i].id,
            name: props.listUsers[i].username,
            role: props.listUsers[i].listUserRole
        })
    }
})

async function CreateTask() {
    if (currentRole.value != 1) {
        task.value = "You can not add Taks, cuz your not a Admin lol"
        return;
    }

    console.log(task.value)
    if (task.value.length === 0) return;

    if (editedTaskId === null) {
        let createdTask = {
            ListId: props.listId,
            TypeId: 1,
            Content: task.value,
        }

        let taskId = await todo_post(routes.LIST_ITEM, null, createdTask)

        Tasks.value.push({
            id: taskId,
            idInFrontendList: TaskIdCunt++,
            Content: task.value,
            checkedSum: 0,
            isCheckedByCurrentUser: false
        })
    }
    else {
        Tasks.value[editedTaskId].Content = task.value;

        let editedTask = {
            Id: editedTaskIdinDB,
            ListId: props.listId,
            TypeId: 1,
            Content: task.value,
        }

        await todo_put(routes.LIST_ITEM, null, editedTask)

        editedTaskId = null;
        editedTaskIdinDB = null;
    }

    task.value = ''
}

async function DeleteTask(taskContent, taskId) {
    if (currentRole.value != 1) return;
    Tasks.value = Tasks.value.filter(tasks => tasks.Content != taskContent)
    await todo_put(routes.LIST_ITEM_DELETE, null, { ListId: props.listId, Id: taskId })
}

async function EditTask(taskId, taskIdInDB) {
    if (currentRole.value != 1) return;
    task.value = Tasks.value[taskId].Content;
    editedTaskId = taskId;
    editedTaskIdinDB = taskIdInDB;
}

async function OnCheck(taskId, taskIdInDB) {
    if (currentRole.value == 3) {
        task.value = "You can't to this because you're not allowed to."
        return;
    }

    let checkedTask = {
        Id: taskIdInDB,
        ListId: props.listId,
        TypeId: 1
    }

    await todo_put(routes.LIST_ITEM_CHECK, null, checkedTask)

    Tasks.value[taskId].isCheckedByCurrentUser = true
    Tasks.value[taskId].checkedSum++
}

async function OnUnCheck(taskId, taskIdInDB) {
    let unCheckedTask = {
        Id: taskIdInDB,
        ListId: props.listId,
        TypeId: 1
    }

    await todo_put(routes.LIST_ITEM_UNCHECK, null, unCheckedTask)

    Tasks.value[taskId].isCheckedByCurrentUser = false
    Tasks.value[taskId].checkedSum--
}

</script>

<template>
    <div class="container mt-4" :key="componentKey">
        <h1>{{ TodoName }}</h1>
        <!-- Add Task -->
        <div class="d-flex mt-5 mb-5">
            <input v-model="task" type="Content" placeholder="Neues Todo hinzufügen" class="form-control">
            <button v-if="editedTaskId != null" @click="CreateTask" class="btn btn-primary">Edit</button>
            <button v-else @click="CreateTask" class="btn btn-primary">Hinzufügen</button>
        </div>
        <hr>
        <div class="row row-cols-2 m-5">

            <!-- List of Tasks -->
            <div class="col-8">
                <ul class="list-group  list-group-hover">
                    <li class="list-group-item" style="font-weight: bold; background: lightgray;">Tasks:</li>
                    <li v-for="todo in Tasks" :key="todo.id" class="list-group-item">
                        <!-- Task value -->
                        {{ todo.Content }}

                        <button v-if="currentRole === 1" @click="EditTask(todo.idInFrontendList, todo.id)" class="btn btn-secondary"
                            style="font-size: x-small; padding: 0.3%; margin-right: 3px; margin-left: 20px;">Edit</button>
                        <!-- Delete Button -->
                        <button v-if="currentRole === 1" @click="DeleteTask(todo.Content, todo.id)" class="btn btn-danger"
                            style="font-size: x-small; padding: 0.3%;">Entfernen</button>
                    </li>
                    <li class="list-group-item" v-show="Tasks.length === 0">Keine Tasks vorhanden</li>
                </ul>
            </div>

            <!-- List of completed Tasks -->
            <div class="col-2">
                <ul class="list-group">
                    <li class="list-group-item" style="font-weight: bold; background: lightgray">{{ currentUser }}</li>
                    <li v-for="todos in Tasks" :key="todos.id" class="list-group-item">
                        <button @click="OnUnCheck(todos.idInFrontendList, todos.id)" v-if="todos.isCheckedByCurrentUser"
                            class="btn btn-success" style="font-size: x-small; padding: 0.3%;">Done</button>
                        <button @click="OnCheck(todos.idInFrontendList, todos.id)" v-else class="btn btn-primary"
                            style="font-size: x-small; padding: 0.3%;">Abhaken</button>
                    </li>
                </ul>
                <!-- <button type="button" class="btn btn-success btn-lg mt-2" data-bs-toggle="modal"
                    data-bs-target="#exampleModal">
                    Alle anzeigen
                </button> -->
            </div>

            <div class="col-2">
                <ul class="list-group">
                    <li class="list-group-item" style="font-weight: bold; background: lightgray">Done by</li>
                    <li v-for="task in Tasks" :key="task.id" class="list-group-item">
                        {{ task.checkedSum }} / {{ Users.length }}
                    </li>
                    <li class="list-group-item" style="font-weight: bold; background: lightgray">users</li>
                </ul>
            </div>
        </div>

        <!-- Modal -->
        <div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-xl">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLabel">Details to {{ TodoName }}</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <div class="row row-cols-2 m-5">

                            <!-- List of Tasks -->
                            <div class="col-7">
                                <ul class="list-group  list-group-hover">
                                    <li class="list-group-item" style="font-weight: bold; background: lightgray;">Tasks:
                                    </li>
                                    <li v-for="todo in Tasks" :key="todo.id" class="list-group-item">
                                        <!-- Task value -->
                                        {{ todo.Content }}

                                        <button @click="EditTask(todo.id)" class="btn btn-secondary"
                                            style="font-size: x-small; padding: 0.3%; margin-right: 3px; margin-left: 20px;">Edit</button>
                                        <!-- Delete Button -->
                                        <button @click="DeleteTask(todo.id)" class="btn btn-danger"
                                            style="font-size: x-small; padding: 0.3%;">Entfernen</button>
                                    </li>
                                </ul>
                            </div>

                            <!-- List of completed Tasks -->
                            <div class="col-2">
                                <ul class="list-group">
                                    <li class="list-group-item"
                                        style="font-weight: bold; background: lightgray; font-size: smaller;">
                                        {{ currentUser }}
                                    </li>
                                    <li v-for="n in Tasks.length" :key="n.id" class="list-group-item">
                                        <input type="checkbox" checked>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary" data-bs-dismiss="modal">Okay</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style>
.list-group-hover .list-group-item:hover {
    background-color: #f5f5f5;
}
</style>