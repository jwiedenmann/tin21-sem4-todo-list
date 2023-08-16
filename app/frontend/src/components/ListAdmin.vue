<script setup>
import { ref, reactive, onMounted } from 'vue'
import { Modal } from'bootstrap'

// give each user a unique id
let id = 0
let searchInput = ref('')
const title = ref('New Todo List')
const users = ref([])
let duplicateUser = ref('')

const fooUsers = ["Maxi", "John Cena", "DerUser42069"]

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

function filteredList(){
    return fooUsers.filter((userName)=>
        userName.toLowerCase().includes(searchInput.value.toLowerCase())
    );
}

function addUser(userName) {
    if(userName && !users.value.some(u => u.name === userName)){
        users.value.push({ id: id++, name: userName });
        console.log(users)
    }else{
        openModal();
        duplicateUser.value = userName
    }
  
}

function removeUser(user) {
  users.value = users.value.filter((u) => u !== user)
}
</script>

<template>
<div class="listAdmin">
    <h1>{{ title }}</h1>
    <form>
        <div class="form-group row">
            <label for="titleHelp" class="col-sm-2 col-form-label">Title</label>
            <div class="col-sm-10">
                <input type="text" class="form-control" id="ListTitle" aria-describedby="titleHelp" v-model="title" required>
            </div> 
        </div>
        <div class="form-group-row">
            <div class="input-group">
                <label for="addUser" class="col-sm-2 col-form-label">Add User</label>
                <div class=" form-control col-sm-10">
                    <div class="search-bar">
                        <input id="addUser" type="text" v-model="searchInput" placeholder="Search for user to add..." />
                        <i class="fa-solid fa-magnifying-glass"></i>
                    </div>                 
                    <ul class="list-group">
                        <li  v-for="user in filteredList()" :key="user" class="list-group-item">
                        {{ user }}
                        <button class="button alert pull-right"  @click="addUser(user)"><i class="fa-solid fa-plus"></i></button>
                        </li>
                    </ul>
                    <div class="item error" v-if="searchInput&&!filteredList().length">
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
        <div class="form-group row">          
            <ul class="list-group">
                <li v-for="user in users" :key="user.id" class="list-group-item">
                {{ user.name }}
                <label for="userRole">Role</label>
                <select id="userRole">
                    <option selected>Admin</option>
                    <option>User</option>
                    <option>Read only</option>
                </select>
                <button lass="button alert pull-right"  @click="removeUser(user)">Remove</button>
                </li>
            </ul>
        </div>
        <button type="submit" class="btn btn-primary">Submit</button>
    </form>
    
</div>
 
</template>