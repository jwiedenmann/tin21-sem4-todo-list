<template>
    <div id="listOverview">
        <h3>Todos for {{userSession.displayName}}</h3>
        <div class="list-group">
            <div v-for="todo in todoDataList">
                <a :href="'/list/' + todo.id" class="list-group-item list-group-item-action flex-column align-items-start">
                    <div class="d-flex w-100 justify-content-between">
                        <h5 class="mb-1">{{todo.title}}</h5>
                        <small>{{todo.creationDate}}</small>
                    </div>
                    <small>{{todo.role}}</small>
                </a>
            </div>
        </div>
    </div>
</template>

<style scoped>

</style>
<script lang="ts">
import { store, signout } from '../../shared';
import { Component, Prop, Vue } from 'vue-property-decorator';

@Component
export class ListOverview extends Vue {
  data() {
    return {
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
      }

  }
 
  get userSession() {
    return store.userSession;
  }

}
export default ListOverview
</script>