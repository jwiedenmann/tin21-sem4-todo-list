<script>
import ListAdminVue from '../components/ListAdmin.vue'
import { todo_get } from '@/todoclient';
import routes from '@/constants/todoroutes'

export default {
  name: 'ListDetail',
  components: {
    ListAdminVue
  },
  data() {
    return {
      dataReady: false,
      showAdminView: false,
      componentKey: 0,
      listTitle: '',
      listUsers: [],
      realTodoList: [],
      todoDataList: [
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
    };
  },
  async mounted() {
    this.realTodoList = await todo_get(routes.LIST, { listId: 1 })
    console.log(this.realTodoList)
    this.dataReady = true;
  }, methods: {
    async openAdminView(listId){   
      if(listId){
        let list = await todo_get(routes.LIST, { listId: listId })

        this.listTitle = list.title
        this.listUsers = list.listUsers
        console.log(this.listUsers)
      }else {
        this.listUsers = []
        //hier noch den aktuell angemeldeten Nutzer als ListUser hinzufügen
        this.listTitle = "New Todo List"
      }
      this.showAdminView = true;
      this.forceRerenderer()
    },
    forceRerenderer(){
      this.componentKey += 1
    }
  }

}
</script>
<template>
    <div v-if="dataReady" class="detail">
     <div class="row">
        <div class="col-md-4">
          <ul class="list-group">
          <li v-for="todo in todoDataList" v-bind:key="todo.id">
            <div class="list-group-item flex-column align-items-start" >
              <div class="d-flex justify-content-between">
                <h5 class="mb-1">{{ todo.title }}</h5>
                <small>{{ todo.creationDate }}</small>
              </div>
              <!--<small>{{ todo.role }}</small>-->
            </div>
          </li>
          <li>
            <div class="list-group-item flex-column align-items-start" >
              <div class="d-flex justify-content-between">
                <h5 class="mb-1">{{ realTodoList.title }}</h5>
                <small>{{ realTodoList.creationDate }}</small>
                <button type="button" class="btn btn-outline-secondary"  @click="openAdminView(realTodoList.id)"><i class="fa-solid fa-gear"></i></button>
              </div>
              <small>{{ realTodoList.id }}</small>
            </div>
          </li>
        </ul>
        <button type="button" class="btn btn-success" @click="openAdminView()"><i class="fa-solid fa-plus"></i></button>
      </div>
      <div class="col-md-8">
        <ListAdminVue v-if="showAdminView" :key="componentKey" :list-title="listTitle" :list-users="listUsers"/>
      </div>
     </div>
    </div>
    <div v-else>
      Loading...
    </div>
  </template>
  
<style scoped>
ul {
  list-style-type: none;
}
</style>
