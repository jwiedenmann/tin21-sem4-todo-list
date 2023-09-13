<script setup>
// ----- IMPORTS -----

import { onMounted, ref } from 'vue'
import { todo_get } from '@/todoclient'
import routes from '@/constants/todoroutes'
import store from '@/store';

// ----- CONSTS -----

let TaskId = 0
let UserId = 0
const currentUser = ref('')
const task = ref('')
let editedTaskId = null

const TodoName = ref('')

const Tasks = ref([])

const Users = ref([
    { id: UserId++, name: 'Donile', role: 'admin', done: true },
    { id: UserId++, name: 'Maxüüüüüü', role: 'user', done: false },
    { id: UserId++, name: 'Jonaaaas', role: 'user', done: false },
])

onMounted(async () => {
    currentUser.value = store.state.user.username
    let newList = await todo_get(routes.LIST, { listId: 1 })
    let lis = newList.listItems
    let lisUsers = newList.listUsers
    TodoName.value = newList.title

    for (let i = 0; i < lis.length; i++) {
        console.log(lis[i].content);
        Tasks.value.push({
            id: TaskId++,
            text: lis[i].content
        })
    }

    for (let i = 0; i < lisUsers.length; i++) {
        console.log(lisUsers[i].username);
        Users.value.push({
            id: UserId++,
            name: lisUsers[i].username,
            role: 'user',
            done: true
        })
    }
})

function CreateTask() {
    console.log(task.value)
    if (task.value.length === 0) return;

    if (editedTaskId === null) {
        Tasks.value.push({
            id: TaskId++,
            text: task.value
        })
    }
    else {
        Tasks.value[editedTaskId].name = task;
        editedTaskId = null;
    }

    task.value = '';
}

function DeleteTask(taskText) {
    Tasks.value = Tasks.value.filter(tasks => tasks.text != taskText)

    // Tasks.value.splice(taskId, 1);
}

function EditTask(taskId) {
    task.value = Tasks.value[taskId].text;
    editedTaskId = taskId;
}

</script>

<template>
    <div class="container">
        <h2>{{ TodoName }}</h2>

        <!-- Add Task -->
        <div class="d-flex mt-5 mb-5">
            <input v-model="task" type="text" placeholder="Neues Todo hinzufügen" class="form-control">
            <button @click="CreateTask" class="btn btn-primary">Hinzufügen</button>
        </div>
        <hr>
        <div class="row row-cols-2 m-5">

            <!-- List of Tasks -->
            <div class="col-9">
                <ul class="list-group  list-group-hover">
                    <li class="list-group-item" style="font-weight: bold; background: lightgray;">Tasks:</li>
                    <li v-for="todo in Tasks" :key="todo.id" class="list-group-item">
                        <!-- Task value -->
                        {{ todo.text }}

                        <button @click="EditTask(todo.id)" class="btn btn-secondary"
                            style="font-size: x-small; padding: 0.3%; margin-right: 3px; margin-left: 20px;">Edit</button>
                        <!-- Delete Button -->
                        <button @click="DeleteTask(todo.text)" class="btn btn-danger"
                            style="font-size: x-small; padding: 0.3%;">Entfernen</button>
                    </li>
                    <li class="list-group-item" v-show="Tasks.length === 0">Keine Tasks vorhanden</li>
                </ul>
            </div>

            <!-- List of completed Tasks -->
            <div class="col-2">
                <ul class="list-group">
                    <li class="list-group-item" style="font-weight: bold; background: lightgray">{{currentUser}}</li>
                    <li v-for="n in Tasks.length" :key="n.id" class="list-group-item">
                        <input type="checkbox" checked>
                    </li>
                </ul>
                <button type="button" class="btn btn-success btn-lg mt-2" data-bs-toggle="modal"
                    data-bs-target="#exampleModal">
                    Alle anzeigen
                </button>
            </div>
        </div>

        <!-- Modal -->
        <div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-xl">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLabel">Modal title</h5>
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
                                        {{ todo.text }}

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
                                    <li class="list-group-item" style="font-weight: bold; background: lightgray; font-size: smaller;">"Username"
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