<script setup>
import { ref, reactive, onMounted, defineProps } from 'vue'
import { Modal } from 'bootstrap'
import { todo_get, todo_put } from '@/todoclient'
import { todo_post } from '@/todoclient'
import routes from '@/constants/todoroutes'
import ListUserComponentVue from './ListUserComponent.vue'

//Information about list passed by ListOverview.vue
const props = defineProps({
    listTitle: {
        type: String,
        required: true
    },
    listId: Number,
    listUsers: Array,
    newList: Boolean
})

//define refs
let searchInput = ref('')
const title = ref(props.listTitle)
const users = ref([])
const modalHeader = ref('Error!')
const modalMsg = ref('')
let showMsg = false
let redirectToHome = ref(false)
const searchUserResults = ref([])

const UserRoles = {
    "None": 0,
    "ListAdmin": 1,
    "ListUser": 2,
    "ReadOnly": 3
}
const state = reactive({
    modal_error: null,
})

//render list of shared users when opening the component
onMounted(() => {
    state.modal_error = new Modal('#errorModal', {})
    console.log(users.value)
    let sharedUsers = props.listUsers

    if (!users.value.length && sharedUsers.length) {
        for (let i = 0; i < props.listUsers.length; i++) {
            users.value.push({ Id: sharedUsers[i].id, Username: sharedUsers[i].username, ListUserRole: parseInt(sharedUsers[i].listUserRole) });
        }
    }
})

//functions to open and close Modal messages
function openModal() {
    state.modal_error.show()
}

function closeModal() {
    state.modal_error.hide()
    if(redirectToHome.value){
        redirectToHome.value = false
        //window.location.reload()
    }
}

//executed after clicking the search button. Sets the searchUserResults value to the list of users returned by the backend
async function searchUser(searchTerm) {
    //call UserController
    if (searchTerm) {
        searchUserResults.value = await todo_get(routes.USER_SEARCH, { searchTerm })
        showMsg = true
        console.log(searchUserResults.value)
    } else {
        searchUserResults.value = []
    }
}

//check if user is already added and if not, add user to the shared users
function addUser(user) {
    if (user.username && !users.value.some(u => u.Username === user.username)) {
        users.value.push({ Id: user.id, Username: user.username, ListUserRole: UserRoles["ListAdmin"] });
        console.log(users)
    } else {
        modalHeader.value = "User already added!"
        modalMsg.value = "The user " + user.username + " was already added to the list. Please add a new user to your Todos!"
        openModal();
    }

}

function removeUser(userId) {
    users.value = users.value.filter((u) => u.Id !== userId)
}

//submit function when creating a new list. Validate inputs and send a POST to the backend api
async function createNewList() {
    let list = { Title: title.value, ListUsers: users.value }
    if (!list.Title) {
        modalHeader.value = "Missing title."
        modalMsg.value = "Please give your Todo list a title."
        openModal();
    } else if (!list.ListUsers.length) {
        modalHeader.value = "Who is this for?"
        modalMsg.value = "Please make sure to select at least one user that has access to the Todo list."
        openModal();
    }else {
        await todo_post(routes.LIST, null, list)
        modalHeader.value = "List was created"
        modalMsg.value = "The list \"" + list.Title + " \" was successfully created. You can open the list in the menu to the left."
        redirectToHome.value = true
        openModal();
    }
}

//submit function when updating an existing list. Validate inputs and send a PUT to the backend api
async function updateList(){
    console.log('Time for an update')
    let list = { Id: props.listId, Title: title.value, ListUsers: users.value }
    if (!list.Title) {
        modalHeader.value = "Missing title."
        modalMsg.value = "Please give your Todo list a title."
        openModal();
    } else if (!list.ListUsers.length) {
        modalHeader.value = "Who is this for?"
        modalMsg.value = "Please make sure to select at least one user that has access to the Todo list."
        openModal();
    }else {
        await todo_put(routes.LIST, null, list)
        modalHeader.value = "List settings has been updated"
        modalMsg.value = "The list \"" + list.Title + " \" was successfully edited. You can open the list in the menu to the left."
        redirectToHome.value = true
        openModal();
    }
}

//update Role ID for user in shared users
function updateRole(userId, newRole) {
    console.log(userId)
    console.log(newRole)
    if (userId && newRole) {
        let userIndex = users.value.findIndex(u => u.Id == userId)
        users.value[userIndex].ListUserRole = parseInt(newRole)
    }
}

</script>

<template>
    <div class="listAdmin mt-4">
        <h1>{{ title }}</h1>
        <form id="editTodoListForm">
            <div class="form-group">
                <div class="input-group">
                    <div class="form-control col control-no-border">
                        <div class="titleInput">
                            <div class="input-group row justify-content-md-center">
                                <label for="listTitle" class="col col-lg-2 col-form-label">Title</label>
                                <div class="col-md-auto col-lg-8 control-no-border">
                                    <input type="text" class="form-control rounded" id="listTitle"
                                        aria-describedby="listTitle" v-model="title" required>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="input-group">
                    <div class=" form-control col control-no-border">
                        <div class="search-bar">
                            <div class="input-group row justify-content-md-center">
                                <label for="addUser" class="col col-lg-2 col-form-label">Add User</label>
                                <div class="col-md-auto col-lg-8 d-flex">
                                    <input id="addUser" type="search" v-model="searchInput" class="form-control rounded"
                                    placeholder="Search for user to add..." aria-label="Search"
                                    aria-describedby="addUser" />
                                    <button type="button" class="btn btn-outline-primary" @click="searchUser(searchInput)"><i
                                        class="fa-solid fa-magnifying-glass"></i></button>
                                </div>
                               
                            </div>
                        </div>
                        <ul class="list-group" id="searchResultList">
                            <li v-for="user in searchUserResults" :key="user.id"
                                class="list-group-item d-flex justify-content-between align-items-center">
                                {{ user.username }}
                                <button type="button" class="btn btn-outline-success" @click="addUser(user)"><i
                                        class="fa-solid fa-plus"></i></button>
                            </li>
                        </ul>
                        <div class="item error" v-if="showMsg && !searchUserResults.length">
                            <p>No results found!</p>
                        </div>

                        <!-- Modal -->
                        <div class="modal fade" id="errorModal" tabindex="-1" aria-labelledby="exampleModalLabel"
                            aria-hidden="true">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h5 class="modal-title" id="exampleModalLabel">{{ modalHeader }}</h5>
                                        <button type="button" class="btn-close" aria-label="Close"
                                            @click="closeModal()"></button>
                                    </div>
                                    <div class="modal-body">
                                        {{ modalMsg }}
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
                <ul class="list-group col-md-8">
                    <li v-for="user in users" :key="user.Id" class="suListItem">
                        <ListUserComponentVue :userId="user.Id" :userRole="user.ListUserRole" :userName="user.Username"
                            @remove-user="removeUser" @update-role="updateRole" />
                    </li>
                </ul>
            </div>
            <hr />
            <button v-if="props.newList" type="button" class="btn btn-primary"  @click="createNewList()">Create New List</button>
            <button v-else type="button" class="btn btn-primary"  @click="updateList()">Save Changes</button>
        </form>

    </div>
</template>
<style scoped>
#editTodoListForm {
    padding: 2em;
}

.suListItem {
    list-style-type: none;
}

.control-no-border {
    border: none;
}

#searchResultList {
    margin-top: 1em;
}

.search-bar {
    margin-top: 2em;
}</style>