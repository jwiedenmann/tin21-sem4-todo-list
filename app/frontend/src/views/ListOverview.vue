<template>
  <div class="list-overview">
  <!-- <div class="lists">
    <h3>Todos for me</h3>
    <div class="list-group">
      <div v-for="todo in todoDataList" v-bind:key="todo.id">
        <a :href="'/list/' + todo.id" class="list-group-item list-group-item-action flex-column align-items-start">
          <div class="d-flex w-100 justify-content-between">
            <h5 class="mb-1">{{ todo.title }}</h5>
            <small>{{ todo.creationDate }}</small>
          </div>
          <small>{{ todo.role }}</small>
        </a>
      </div>
    </div>
  </div> -->
  <div class="row flex-grow-1 h-75 mb-4 rounded-3" style="background-color: white;">
    <div class="container-fluid flex-column d-flex">
      <div class="row flex-grow-1">
        <div class="col-3">
          <div class="row border-end border-bottom rounded-top-3 rounded-end-0 p-3 text-light"
            style="background-color: #54B4D3;">{{ username }}</div>
          <div class="row p-4 todo-list-element" id="1" @click="selectTodoList()">Meine tolle TODO Liste</div>
          <div class="row">blub</div>
          <div class="row">blub</div>
        </div>
        <div class="col-md-auto flex-grow-1">

        </div>
      </div>
    </div>
  </div>

  <footer class="row justify-content-center pt-1 text-body-secondary border-top">
    <p>
      &copy; 2023
    </p>
  </footer>
</div>
</template>

<style scoped>
.todo-list-element:hover {
  background-color: #f5f5f5;;
}
</style>

<script setup>
import { ref } from 'vue';
import store from '@/store';

const username = ref(store.state.user.username)

function selectTodoList(listId) {
  
}

</script>