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
    <div class="container" style="max-width: 90%;">
        <div class="row">
            <div class="col lg-6">
                <ul class="list-group list-group-horizontal justify-content-center">
                    <li class="list-group-item">
                        <input v-model="TodoName" type="text"
                            style="border: none; border-color: transparent; text-align: center;" />
                    </li>
                </ul>
            </div>
            <div class="col lg-6">
                <!-- <ul class="list-group list-group-horizontal">
                <li v-for="user in Users" :key="user.id" class="list-group-item">
                    <input v-model="user.name" type="text"
                        style="border: none; border-color: transparent; text-align: center;" />
                </li>
            </ul> -->
            </div>
        </div>
        <hr>
        <div class="row">
            <div class="col lg-6">

            </div>
            <div class="col lg-6">
                <ul class="list-group list-group-horizontal">
                    <li v-for="user in Users" :key="user.id" class="flex-fill list-group-item">
                        <input v-model="user.name" type="text"
                            style="border: none; border-color: transparent; text-align: center;" />
                    </li>
                </ul>
            </div>
        </div>
        <div class="row">
            <div class="col lg-6">
                <ul class="list-group">
                    <li v-for="todo in Todos" :key="todo.id" class="list-group-item">
                        <!-- <input v-model="todo.text" type="text"
                        style="border: none; border-color: transparent; text-align: center; height: min-content" /> -->
                        {{ todo.text }}
                    </li>
                </ul>
            </div>
            <div class="col lg-6">
                <div class="row">
                    <div class="col">
                        <ul class="list-group">
                            <li v-for="n in 8" :key="n.id" class="list-group-item">
                                <input type="checkbox" checked>
                            </li>
                        </ul>
                    </div>
                    <div class="col">
                        <ul class="list-group">
                            <li v-for="n in 8" :key="n.id" class="list-group-item">
                                <input type="checkbox">
                            </li>
                        </ul>
                    </div>
                    <div class="col">
                        <ul class="list-group">
                            <li v-for="n in 8" :key="n.id" class="list-group-item">
                                <input type="checkbox" checked style="height: min-content;">
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <div class="container">
        <div class="row">

        </div>
    </div>
    </div>
    
</template>