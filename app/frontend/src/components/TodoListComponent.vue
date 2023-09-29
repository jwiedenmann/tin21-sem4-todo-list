<script setup>
// ----- IMPORTS -----

import { onMounted, ref, defineProps, defineEmits, onBeforeUnmount } from 'vue'
import { todo_get, todo_post, todo_put } from '@/todoclient'
import routes from '@/constants/todoroutes'
import store from '@/store';
import topics from '@/constants/todotopics'
import debounce from "lodash.debounce";

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

// task is the value of the input bar "Task hinzufügen"
const task = ref('')
const currentUser = ref('')
const currentUserId = ref(0)
const currentListId = ref(0)
const currentRole = ref(0)
let editedTaskId = null
let editedTaskIdinDB = null
let TaskIdCunt = 0
let componentKey = ref(0)
const TodoName = ref('')

const firstPos = ref(true)
const isInsert = ref(true)
const insertedChars = ref([])
const currentPosition = ref(0)
const startPosition = ref(0)
const changeLength = ref(0)
const changeValue = ref('')
const sentChanges = ref({})
const pendingChanges = ref([])
const taskInEdit = ref(null)
const Tasks = ref([])
const Users = ref([])
const editFieldContent = ref('')
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
//initialize clientStateInfos for operational transformations
//let stateInfos = new clientStateInfos(0)
client.on('connect', function () {
  // subscribe to a topic to receive message from it
  client.subscribe(topics.LIST_TOPIC + props.listId, function (err) {
    if (!err) {
      console.log('Connected and subscribed to topic: ' + topics.LIST_TOPIC + props.listId)
    }
  })
  client.subscribe(topics.SERVER_ACK, function(err){
    if (!err) {
      console.log('Connected and subscribed to topic: ' + topics.SERVER_ACK)
    }else {
        console.log(err)
    }
  })
})

// What should happen if I recieve a message?
client.on('message', function (topic, message) {
  // message is Buffer
  console.log("Received:" + message.toString() + " from Topic: " + topic.toString())
  if(topic.toString() === topics.LIST_TOPIC + props.listId){
    console.log('Reload Todos')
    emit('reloadTodos', props.listId)
  }else if(topic.toString() === topics.SERVER_ACK){
    console.log('im ServerACK')
    sentChanges.value = {}
    let messageObj = JSON.parse(message.toString())
    if(editFieldContent.value && editedTaskIdinDB === messageObj.ListItemClientUpdate.ListItemId){
        Tasks.value.find((el) => el.id == messageObj.ListItemClientUpdate.ListItemId).Content = editFieldContent.value
    }
    if(messageObj.ListItemClientUpdate.UserId != parseInt(store.state.user.id)){
        //update from another user, apply changes
        applyChanges(Tasks.value.find((el) => el.id == messageObj.ListItemClientUpdate.ListItemId), messageObj.ListItemClientUpdate)
    } 
    Tasks.value.find((el) => el.id == messageObj.ListItemClientUpdate.ListItemId).lastSyncedRevision = messageObj.NewRevisionId
    console.log('Die neue Rev ID vom ServerACK: ', Tasks.value.find((el) => el.id == messageObj.ListItemClientUpdate.ListItemId).lastSyncedRevision)
    if(pendingChanges.value.length){
        let clientUpdate = pendingChanges.value.shift()
        console.log('Sending this to server from the pending changes Queue:', clientUpdate)
        console.log('Before: ', pendingChanges.value)
        doPublish(clientUpdate)
        console.log('After: ', pendingChanges.value)
    }
  }
  // to end the connection:
  //client.end()
})

function doPublish(clientUpdate) {
    let publication = {
        topic: topics.CLIENT_UPDATE,
        qos: 1,
        payload: JSON.stringify(clientUpdate)
    }
  const { topic, qos, payload} = publication
  console.log(publication)
  client.publish(topic, payload, qos, error => {
    if (error) {
      console.log('Publish error', error)
    }else{
        sentChanges.value = clientUpdate
        console.log('Successfully published this: ' + payload + ' to topic ' + topic)
    }
  })
}

function getCursor(event) {
    if(firstPos.value){
        startPosition.value = event.target.selectionStart
        console.log('Starting Pos: ', startPosition.value)
        firstPos.value = false
    }
    if(event.inputType === 'insertText'){
        insertedChars.value.push(event.data)
        console.log(insertedChars.value)
    }else if(event.inputType === 'deleteContentBackward'){
        console.log('This is the delete event: ', event)
        isInsert.value = false
        //pendingChanges.value.push(currentChange)
        sendUpdate(taskInEdit.value, Tasks.value[editedTaskId].lastSyncedRevision, false, startPosition.value +2, 1, null)
    }else{
        console.log(event.inputType)
        isInsert.value = false
        sendUpdate(taskInEdit.value, Tasks.value[editedTaskId].lastSyncedRevision, false, startPosition.value +2, 1, null)
    }
    currentPosition.value =  event.target.selectionStart  
    console.log('Caret at: ', currentPosition.value)
}

function sendUpdate(listItemId, lsr, isInsert, position, length, value){
    let userId = parseInt(store.state.user.id);
    let currentChange = {
    "listItemId": listItemId,
    "lastSyncedRevision": lsr,
    "isInsert": isInsert,
    "position": position - 1,
    "userId" : userId,
    "length": length,
    "value": value
  }
  pendingChanges.value.push(currentChange)
  console.log('Pushed this to update queue: ', currentChange)

  //send update to server if possible
  if(pendingChanges.value.length && Object.keys(sentChanges.value).length === 0){
    let clientUpdate = pendingChanges.value.shift()
    console.log('Sending this to server:', clientUpdate)
    console.log('Before: ', pendingChanges.value)
    doPublish(clientUpdate)
    console.log('After: ', pendingChanges.value)
  }
  //reset parameters
  firstPos.value = true
  insertedChars.value = []
}

const debouncedHandler = debounce(event => {
  console.log('New document value:', event.target.value);
  changeLength.value = currentPosition.value - (startPosition.value - 1)
  console.log('Length: ', changeLength.value)
  //get Inserted Text
  changeValue.value = insertedChars.value.join('')
  console.log(changeValue.value)
  if(taskInEdit.value != null && changeValue.value){
    sendUpdate(taskInEdit.value, Tasks.value[editedTaskId].lastSyncedRevision, true, startPosition.value, changeLength.value, changeValue.value)
  }else{
    console.log('Delete one thing')
  }
}, 500);

function applyChanges(listItem, serverUpdate){
    console.log('We apply it:', listItem)
    if(typeof listItem.Content === "undefined"){
        console.log('Something went wrong, reload todo elements')
        emit('reloadTodos', props.listId)
        return
    }else if(serverUpdate.IsInsert){
        listItem.Content = listItem.Content.slice(0, serverUpdate.Position) + serverUpdate.Value + listItem.Content.slice(serverUpdate.Position)
    }else {
        let startIndex = serverUpdate.Position - serverUpdate.Length
        listItem.Content = listItem.Content.slice(0, startIndex) + listItem.Content.slice(serverUpdate.Position)
    }
    if(editedTaskIdinDB === listItem.id){
        console.log('Update edit ref')
        editFieldContent.value = listItem.Content
        //Tasks.value.find((el) => el.id == editedTaskIdinDB).Content = listItem.Content
    }
}
onBeforeUnmount(() => {
  debouncedHandler.cancel();
});

// onMounted gets called every time the view-component is beeing called (open TODO-List, relode webpage)
// It assigns the data from the store and the loaded data to local variables 
// creates two lists. One for the tasks in the current list and one for the active users in the list.
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
        let lsr = todoListItems[i].revisionId
        console.log(todoListItems[i].content);
        Tasks.value.push({
            id: todoListItems[i].id,
            idInFrontendList: TaskIdCunt++,
            Content: todoListItems[i].content,
            checkedSum: todoListItems[i].checkedByUserIds.length,
            isCheckedByCurrentUser: checkedByCurrentUser,
            lastSyncedRevision: lsr
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

// CreateTask is called if a user clicks the "Hinzufügen" button
// Checks user role (has to be admin) and checks length of task content (0 length == not allowed).
// adds the new task to Tasks list and posts it wie todo_post to the database
// if editedTask is not null: That means, that a Task is beeing edited. Task with id = "editedTaskId" is beeing updated
// and Task with Id = "editedTaskIdinDB" is beeing updated to.
// editedTaskIdinDB and editedTaskId are set to null
async function CreateTask() {
    if (currentRole.value != 1) {
        task.value = "You can not add Taks, cuz your not a Admin lol"
        return;
    }
    console.log(task.value.selectionStart)
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

// DeleteTask is called when a user clicks the delete button of a task
// checks if user ist admin
// deletes Task of list
// calls todo_put to "delete" the task out of database
// Parameters: taskContent = content of deleted task, taskIdInDB = Id of the task in database
async function DeleteTask(taskContent, taskIdInDB) {
    if (currentRole.value != 1) return;
    // nicht perfekt, da der Task nach content und nicht nach id gelöscht wird. ist vermerkt
    Tasks.value = Tasks.value.filter(tasks => tasks.Content != taskContent)
    await todo_put(routes.LIST_ITEM_DELETE, null, { ListId: props.listId, Id: taskIdInDB })
}

// EditedTaks is called when a user clicks the edit button of a task
// value of task is beeing set to the content of the task which is beeing edited
// editedTaskIdinDB and editedTaskId are set to the Ids of the task which is beeig edited
async function EditTask(taskId, taskIdInDB) {
    taskInEdit.value = taskIdInDB
    //frontendTaskId.value = taskId
    if (currentRole.value != 1) return;
    task.value = Tasks.value[taskId].Content;
    editedTaskId = taskId;
    editedTaskIdinDB = taskIdInDB;
    editFieldContent.value = Tasks.value[editedTaskId].Content
}

// OnCheck is called when a user checks a task
// Checks whether a user is allowed to check a task
// updates the database at the corrosponding dataset
// sets the isCheckedByCurrentUser ture for the corrosponding Task
// Parameters: taskId = Id of the task in frontend list, taskIdInDB = Id of task in database
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

// OnUnCheck is called when a user unchecks a task
// updates the database at the corrosponding dataset
// sets the isCheckedByCurrentUser false for the corrosponding Task
// Parameters: taskId = Id of the task in frontend list, taskIdInDB = Id of task in database
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
            <input v-if="editedTaskId != null" v-model="editFieldContent" type="Content" v-on:input="debouncedHandler" id="taskField" class="form-control" @input="getCursor($event)">
            <input v-else v-model="task" type="Content" id="taskField" placeholder="Neues Todo hinzufügen" class="form-control">
            <button v-if="editedTaskId != null" @click="CreateTask" class="btn btn-primary">Edit</button>
            <button v-else @click="CreateTask" class="btn btn-primary">Hinzufügen</button>
        </div>
        <hr>
        <div class="container">
            <div class="row justify-content-md-center">
                <!-- List of Tasks -->
                <div class="col col-lg-8 mb-2">
                    <ul class="list-group list-group-hover">
                        <li class="list-group-item" style="font-weight: bold; background: lightgray;">Tasks:</li>
                        <li v-for="todo in Tasks" :key="todo.id" class="list-group-item">
                                 <!-- Task value -->
                                <span class="d-sm-none col-sm-1">{{ todo.idInFrontendList + 1 }}:&nbsp;</span>
                                <span v-if="editedTaskId === todo.idInFrontendList">{{ editFieldContent }}</span>
                                <span v-else>{{ todo.Content }}</span>
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
                <div class="col-sm-auto mb-2">
                    <ul class="list-group">
                        <li class="list-group-item" style="font-weight: bold; background: lightgray">{{ currentUser }}</li>
                        <li v-for="todos in Tasks" :key="todos.id" class="list-group-item">
                            <span class="d-sm-none col-sm-1">{{ todos.idInFrontendList + 1 }}:&nbsp;</span>
                            <button @click="OnUnCheck(todos.idInFrontendList, todos.id)" v-if="todos.isCheckedByCurrentUser"
                                class="btn btn-success" style="font-size: x-small; padding: 0.3%;">Done</button>
                            <button @click="OnCheck(todos.idInFrontendList, todos.id)" v-else class="btn btn-primary"
                                style="font-size: x-small; padding: 0.3%;">Abhaken</button>
                        </li>
                    </ul>
                </div>

                <div class="col-sm-auto mb-2">
                    <ul class="list-group">
                        <li class="list-group-item" style="font-weight: bold; background: lightgray">Done by</li>
                        <li v-for="task in Tasks" :key="task.id" class="list-group-item">
                            <span class="d-sm-none col-sm-1">{{ task.idInFrontendList + 1 }}:&nbsp;</span>
                            {{ task.checkedSum }} / {{ Users.length }}
                        </li>
                        <li class="list-group-item" style="font-weight: bold; background: lightgray">users</li>
                    </ul>
                </div>
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