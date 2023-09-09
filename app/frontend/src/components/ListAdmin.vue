<script setup>
import { ref, reactive, onMounted } from 'vue'
import { Modal } from'bootstrap'
import { todo_get } from '@/todoclient';
import routes from '@/constants/todoroutes'
import ListUserComponentVue from './ListUserComponent.vue'


let searchInput = ref('')
const title = ref('New Todo List')
const users = ref([])
let duplicateUser = ref('')
let showMsg = false

const searchUserResults = ref([])

const state = reactive({
    modal_error: null,
})

onMounted(() => {
    state.modal_error = new Modal('#errorModal', {})
})

function openModal()
{
    state.modal_error.show()
}

function closeModal()
{
    state.modal_error.hide()
}

async function searchUser(searchTerm){
    //call UserController
    if(searchTerm){
        searchUserResults.value = await todo_get(routes.USER_SEARCH, {searchTerm})  
        showMsg = true
        console.log(searchUserResults.value)
    } 
}

function addUser(user) {
    if(user.username && !users.value.some(u => u.username === user.username)){
        users.value.push({ id: user.id, username: user.username, role: "ListAdmin" });
        console.log(users)
    }else{
        openModal();
        duplicateUser.value = user.username
    }
  
}

function removeUser(userId) {
  users.value = users.value.filter((u) => u.id !== userId)
}

</script>

<template>
<div class="listAdmin">
    <h1>{{ title }}</h1>
    <form id="editTodoListForm">
        <div class="form-group">
            <div class="input-group">
                <div class="form-control col-sm-10 control-no-border">
                    <div class="titleInput">
                        <div class="input-group">
                            <label for="listTitle" class="col-sm-2 col-form-label">Title</label>
                            <div class="col-sm-10 control-no-border">
                                <input type="text" class="form-control rounded" id="listTitle" aria-describedby="listTitle" v-model="title" required>
                            </div> 
                        </div>
                    </div>
                </div>
            </div>
            <div class="input-group">
                <div class=" form-control col-sm-10 control-no-border">
                    <div class="search-bar">
                        <div class="input-group">
                            <label for="addUser" class="col-sm-2 col-form-label">Add User</label>
                            <input  id="addUser" type="search"  v-model="searchInput" class="form-control rounded" placeholder="Search for user to add..." aria-label="Search" aria-describedby="addUser"/>
                            <button type="button" class="btn btn-outline-primary" @click="searchUser(searchInput)"><i class="fa-solid fa-magnifying-glass"></i></button>
                        </div>
                    </div>           
                    <ul class="list-group" id="searchResultList">
                        <li v-for="user in searchUserResults" :key="user.id" class="list-group-item d-flex justify-content-between align-items-center">
                        {{ user.username }}
                        <button type="button" class="btn btn-outline-success"  @click="addUser(user)"><i class="fa-solid fa-plus"></i></button>
                        </li>
                    </ul>                    
                    <div class="item error" v-if="showMsg&&!searchUserResults.length">
                        <p>No results found!</p>
                    </div>
                    
                    <!-- Modal -->
                    <div class="modal fade"  id="errorModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
                        <div class="modal-dialog modal-dialog-centered">
                            <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="exampleModalLabel">User already added!</h5>
                                <button type="button" class="btn-close" aria-label="Close" @click="closeModal()"></button>
                            </div>
                            <div class="modal-body">
                                The user {{ duplicateUser }} was already added to the list. Please add a new user to your Todos! 
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-secondary" @click="closeModal()">Ok</button>
                            </div>
                            </div>
                        </div>
                    </div>
                </div>                     
            </div>          
        </div>  
        <hr />
        <h3>Shared users</h3>
        <div class="form-group row d-flex justify-content-center align-items-center">          
            <ul class="list-group col-sm-8">
                <li v-for="user in users" :key="user.id" class="suListItem" >
                    <ListUserComponentVue :userId="user.id" :userRole="user.role" :userName="user.username" @remove-user="removeUser"/>
                </li>
            </ul>
        </div>
        <hr />
        <button type="submit" class="btn btn-primary">Save Changes</button>
    </form>
    
</div>
 
</template>
<style scoped>
#editTodoListForm {
    padding: 2em;
}

.suListItem{
    list-style-type: none;
}
.control-no-border{
    border: none;
}

#searchResultList{
    margin-top: 1em;
}

.search-bar {
    margin-top: 2em;
}
</style>