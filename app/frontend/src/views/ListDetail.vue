<script setup>
import ListAdminVue from '../components/ListAdmin.vue'
import { ref } from 'vue'

const todoDataList = [
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
        ]

let showAdmin = ref(false);
const todoTitle = ref('New Todo List')
// set a boolean ref value as false. we will be using the openModal function to trigger its change


// create the openModal function and set showModal to true
const openAdmin = (title) => {
  showAdmin.value = true;
  todoTitle.value = title
  // console.log(showModal);
};

</script>
<template>
    <div class="detail">
     <div class="row">
        <div class="col-md-4">
          <ul class="list-group">
          <li v-for="todo in todoDataList" v-bind:key="todo.id">
            <div class="list-group-item flex-column align-items-start" @click="openAdmin(todo.title)">
              <div class="d-flex justify-content-between">
                <h5 class="mb-1">{{ todo.title }}</h5>
                <small>{{ todo.creationDate }}</small>
              </div>
              <small>{{ todo.role }}</small>
            </div>
          </li>
        </ul>
        <div class="list-group">
            <div class="list-group-item" @click="openAdmin()">
                Test
            </div>
            <div class="list-group-item">
                Test Nr 2
            </div>
        </div>
      </div>
      <div class="col-md-8">
        <ListAdminVue v-if="showAdmin" :title="todoTitle"/>
      </div>
     </div>
    </div>
  </template>
  
<style scoped></style>
