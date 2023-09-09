import store from '@/store';
import routes from '@/constants/todoroutes'
import axios from 'axios'

export async function todo_get(route) {
    let counter = 0;

    while (counter < 2) {
        let result = await get(route);

        if (result.status == 200) {
            return result.data;
        } else if (result.status != 401) {
            console.log(result);
            throw Error(result);
        }

        result = await post(routes.AUTH_REFRESH);

        if (result.status == 200) {
            store.commit('updateJwtToken', result.data.jwtToken);
        } else {
            console.log(result);
            throw Error(result);
        }

        counter++;
    }
}

async function get(route) {
    try {
        let resp = await axios.get(route);
        return resp;
    } catch (error) {
        return error.response;
    }
}

async function post(route, body) {
    try {
        let resp = await axios.post(route, body, { withCredentials: true });
        return resp;
    } catch (error) {
        return error.response;
    }
}