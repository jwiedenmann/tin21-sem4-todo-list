<script>
import { ref } from 'vue';
import store from '@/store';
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
      loggedInUser: store.state.user.username,
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
    //this.realTodoList = await todo_get(routes.LIST)
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
  <div class="row flex-grow-1 h-75 mb-4 rounded-3" style="background-color: white;">
    <div class="container-fluid flex-column d-flex">
      <div class="row flex-grow-1">
        <div class="col-3">
          <div class="row border-end border-bottom rounded-top-3 rounded-end-0 p-3 text-light"
            style="background-color: #54B4D3;">{{ loggedInUser }}</div>
          <div class="row p-4 todo-list-element" id="1" @click="selectTodoList()">Meine tolle TODO Liste</div>
          <div class="row">blub</div>
          <div class="row">blub</div>
        </div>
        <div class="col-md-auto flex-grow-1">
          <ListAdminVue v-if="showAdminView" :key="componentKey" :list-title="listTitle" :list-users="listUsers"/>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.todo-list-element:hover {
  background-color: #f5f5f5;;
}
</style>