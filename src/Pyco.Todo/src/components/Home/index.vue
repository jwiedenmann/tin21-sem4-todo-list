<template>
    <div class="text-center justify-content-center">
        <div class="cover-container d-flex h-100 p-3 mx-auto flex-column">
            <div class="align-middle">
                <div v-if="userSession" class="text-right">
                    <h1 class="cover-heading">Hello, {{userSession.displayName}}!</h1>
                    <p class="lead">Create a new To-Do List or check out the lists that you are assigned to!</p>
                    <div class="card-deck">
                        <div class="row">
                            <div class="col-lg-4 d-flex align-items-stretch">
                                <div class="card">
                                    <img class="card-img-top" :src="userSession.profileUrl"/>
                                    <div class="card-body">
                                        <h5 class="card-title">My profile</h5>
                                        <p class="card-text">Manage your account settings and more.</p>
                                        <link-button href="/profile" sm primary>To my profile</link-button>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-4 d-flex align-items-stretch">
                                <div class="card">
                                    <img class="card-img-top" :src="userSession.profileUrl" />
                                    <div class="card-body">
                                        <h5 class="card-title">My To-Do Lists</h5>
                                        <p class="card-text">Get an overview of al of your lists and start finishing the tasks!</p>
                                        <link-button href="/lists" sm primary>My To Dos</link-button>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-4 d-flex align-items-stretch">
                                <div class="card">
                                    <img class="card-img-top" :src="userSession.profileUrl" />
                                    <div class="card-body">
                                        <h5 class="card-title">Sign Out</h5>
                                        <p class="card-text">Did you finished your tasks today? Great, now you can log out and come back tomorrow!</p>
                                        <link-button @click="signout" sm primary>Sign Out</link-button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div v-else class="text-right">
                    <h1 class="cover-heading">Welcome to Pyco.Todo</h1>
                    <p class="lead">Create new To-Do-Lists and share them with others! Register now and start your Pyco journey today!</p>
                    <p class="lead">
                        <link-button href="/signin" sm primary>Sign In</link-button>
                        <link-button href="/signup" sm outline-secondary class="ms-2">Register New User</link-button>
                    </p>
                    <p class="pt-3">You are not authenticated.</p>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator';
import { store, signout } from '../../shared';


export class Home extends Vue {
  @Prop({ default: '' }) name: string;
  signout = signout;

  get userSession() {
    return store.userSession;
  }

}
export default Home;
</script>
