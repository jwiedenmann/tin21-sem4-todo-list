<script setup>
// ----- IMPORTS -----

import { ref } from 'vue'

// ----- CONSTS -----

let TaskId = 0
let UserId = 0
let task = ''
let editedTaskId = null

let TodoName = 'Test-TODO Name'

const Tasks = ref([
    { id: TaskId++, text: 'Learn HTML', state: 'done' },
    { id: TaskId++, text: 'Learn JavaScript', state: 'new' },
    { id: TaskId++, text: 'Learn Vue', state: 'inProgress' },
    { id: TaskId++, text: 'Do things in Vue', state: 'inProgress' },
    { id: TaskId++, text: 'Machste Sport', state: 'inProgress' },
    { id: TaskId++, text: 'Clean Mojo Dojo Casa House', state: 'inProgress' },
    { id: TaskId++, text: 'Buy Humvee', state: 'new' },
    { id: TaskId++, text: 'Your Kenough', state: 'new' },
])

const Users = ref([
    { id: UserId++, name: 'Donile', role: 'admin', done: true },
    { id: UserId++, name: 'Maxüüüüüü', role: 'user', done: false },
    { id: UserId++, name: 'Jonaaaas', role: 'user', done: false },
])

function CreateTask() {
    console.log(task)
    if (task === 0) return;

    if (editedTaskId === null) {
        Tasks.value.push({
            id: TaskId++,
            text: task,
            state: 'new'
        })
    }
    else {
        Tasks.value[editedTaskId].name = task;
        editedTaskId = null;
    }

    task = '';
}

function DeleteTask(taskId) {
    Tasks.value.splice(taskId, 1);
}

function EditTask(taskId) {
    task = Tasks.value[taskId].text;
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

                        <button @click="EditTask(todo.id)" class="btn btn-secondary" style="font-size: x-small; padding: 0.3%; margin-right: 3px; margin-left: 20px;">Edit</button>
                        <!-- Delete Button -->
                        <button @click="DeleteTask(todo.id)" class="btn btn-danger" style="font-size: x-small; padding: 0.3%;">Entfernen</button>
                    </li>
                </ul>
            </div>

            <!-- List of completed Tasks -->
            <div class="col-2">
                <ul class="list-group">
                    <li class="list-group-item" style="font-weight: bold; background: lightgray;">"Username"</li>
                    <li v-for="n in Tasks.length" :key="n.id" class="list-group-item">
                        <input type="checkbox" checked>
                    </li>
                </ul>
                <button class="btn btn-success btn-lg mt-2">Alle anzeigen</button>
            </div>
        </div>
    </div>
</template>

<style>
.list-group-hover .list-group-item:hover {
    background-color: #f5f5f5;
}
</style>