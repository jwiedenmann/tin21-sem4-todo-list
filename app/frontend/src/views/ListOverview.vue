<template>
    <div class="lists">
        <h3>Todos for me</h3>
        <div class="list-group">
            <div v-for="todo in content.todoDataList" v-bind:key="todo.id">
                <a :href="'/list/' + todo.id"  class="list-group-item list-group-item-action flex-column align-items-start">
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
<script>
export default {
  data() {
    return {
        content: {
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

  }
}
</script>