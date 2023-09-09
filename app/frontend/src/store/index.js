import { createStore } from 'vuex'
import * as jose from 'jose'
import axios from 'axios'

export default createStore({
  state: {
    jwtToken: '',
    user: {
      id: 0,
      username: ''
    }
  },
  getters: {
  },
  mutations: {
    updateJwtToken(state, newJwtToken) {
      state.jwtToken = newJwtToken;

      try {
        const claims = jose.decodeJwt(state.jwtToken);
        state.user.id = claims.id;
        state.user.username = claims.username;
        axios.defaults.headers.common['Authorization'] = `Bearer ${newJwtToken}`;
      } catch (e) {
        console.log(e);
      }
    }
  },
  actions: {
  },
  modules: {
  }
})
