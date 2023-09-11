<script setup>
import { ref, defineProps, defineEmits } from 'vue'

const props = defineProps({
    userRole: String,
    userId: Number,
    userName: String
})

const emit = defineEmits(['removeUser', 'updateRole'])
const selected = ref(props.userRole)

function updateRole(){
    emit('updateRole', props.userId, selected.value)
}
</script>

<template>
    <span class="w-100 row list-group-item d-flex justify-content-between align-items-center listUserComponent">     
        <i v-if="selected === 'ListUser'" class="fa-solid fa-user col-sm-1 role-icon"></i>
        <i v-else-if="selected === 'ListAdmin'" class="fa-solid fa-user-gear col-sm-1 role-icon"></i>
        <i v-else class="fa-solid fa-glasses col-sm-1 role-icon"></i> 
        <span class="col-sm-3">    
            <select v-model="selected" class="form-select" @change="updateRole()" aria-label="Select role for user" name="userRole">
                <option value="ListAdmin">Admin</option>
                <option value="ListUser">User</option>
                <option value="ReadOnly">Read only</option>
            </select>
        </span>
        <span class="col-sm-4" id="userName">{{ userName }}</span>
        <button type="button" class="btn btn-danger col-sm-1"  @click="emit('removeUser', userId)"><i class="fa-solid fa-trash-can"></i></button>
    </span>
</template>

<style scoped>
.role-icon{
    font-size: 2em;
}

#userName{
    font-size: 1.25em;
}
</style>