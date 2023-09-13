<script setup>
// ----- IMPORTS -----

import { onMounted, ref, defineProps } from 'vue'
import { todo_get } from '@/todoclient'
import routes from '@/constants/todoroutes'
import store from '@/store';

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

let TaskId = 0
let UserId = 0
const currentUser = ref('')
const currentUserId = ref(0)
const task = ref('')
let editedTaskId = null

const TodoName = ref('')

const Tasks = ref([])
const Users = ref([])
let numbers = [3, 4, 3, 2, 3, 3, 7]

onMounted(async () => {
    currentUser.value = store.state.user.username
    currentUserId.value = store.state.user.id
    //let newList = await todo_get(routes.LIST, { listId: 1 })
    //let lis = newList.listItems
    //let checkedUsers = lis[0].checkedByUserIds
    let lisUsers = props.listUsers
    let todoListItems = props.listItems
    TodoName.value = props.listTitle
    
    //let nasd = lis[0].checkedByUserIds.length

    //if (lis[0].checkedByUserIds.includes(3)) nasd = 3;

    for (let i = 0; i < todoListItems.length; i++) {
        let checkedByCurrentUser = false
        if (todoListItems[i].checkedByUserIds.includes(currentUserId)) checkedByCurrentUser = true;

        console.log(todoListItems[i].content);
        Tasks.value.push({
            id: TaskId++,
            text: todoListItems[i].content,
            checkedSum: todoListItems[i].checkedByUserIds.length,
            isCheckedByCurrentUser: checkedByCurrentUser
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
            text: task.value,
            checkedSum: 0,
            isCheckedByCurrentUser: false
        })
    }
    else {
        Tasks.value[editedTaskId].text = task.value;
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
    <div class="container mt-4">
        <h1>{{ TodoName }}</h1>

        <!-- Add Task -->
        <div class="d-flex mt-5 mb-5">
            <input v-model="task" type="text" placeholder="Neues Todo hinzufügen" class="form-control">
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
                    <li class="list-group-item" style="font-weight: bold; background: lightgray">{{ currentUser }}</li>
                    <li v-for="todos in Tasks" :key="todos.id" class="list-group-item">
                        <input v-if="todos.checkedByCurrentUser" type="checkbox" checked>
                        <input v-else type="checkbox">
                    </li>
                </ul>
                <button type="button" class="btn btn-success btn-lg mt-2" data-bs-toggle="modal"
                    data-bs-target="#exampleModal">
                    Alle anzeigen
                </button>
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
                                    <li class="list-group-item" style="font-weight: bold; background: lightgray; font-size: smaller;">
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