<script setup>
// ----- IMPORTS -----

import { ref } from 'vue'

// ----- CONSTS -----

let TodoId = 0
let UserId = 0

let TodoName = 'Test-TODO Name'

const Todos = ref([
    { id: TodoId++, text: 'Learn HTML' },
    { id: TodoId++, text: 'Learn JavaScript' },
    { id: TodoId++, text: 'Learn Vue' },
    { id: TodoId++, text: 'Do things in Vue' },
    { id: TodoId++, text: 'Machste Sport' },
    { id: TodoId++, text: 'Clean Mojo Dojo Casa House' },
    { id: TodoId++, text: 'Buy Humvee' },
    { id: TodoId++, text: 'Your Kenough' },
])

const Users = ref([
    { id: UserId++, name: 'Donile', role: 'admin', done: true },
    { id: UserId++, name: 'Maxüüüüüü', role: 'user', done: false },
    { id: UserId++, name: 'Jonaaaas', role: 'user', done: false },
])

</script>

<template>
    <div class="container">
        <h2>{{ TodoName }}</h2>

        <div class="d-flex mt-5 mb-5">
            <input type="text" placeholder="Neues Todo hinzufügen" class="form-control">
            <button class="btn btn-primary">Hinzufügen</button>
        </div>
        <hr>
        <div class="row row-cols-2 m-5">
            <div class="col-10">
                <ul class="list-group  list-group-hover">
                    <li class="list-group-item" style="font-weight: bold; background: lightgray;">TODOS:</li>
                    <li v-for="todo in Todos" :key="todo.id" class="list-group-item">
                        <!-- <input v-model="todo.text" type="text"
                        style="border: none; border-color: transparent; text-align: center; height: min-content" /> -->
                        {{ todo.text }}
                    </li>
                </ul>
            </div>
            <div class="col-2">
                <ul class="list-group">
                    <li class="list-group-item" style="font-weight: bold; background: lightgray;">"Username"</li>
                    <li v-for="n in 8" :key="n.id" class="list-group-item">
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