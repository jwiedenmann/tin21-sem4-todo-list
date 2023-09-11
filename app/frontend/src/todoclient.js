import store from '@/store';
import routes from '@/constants/todoroutes'
import axios from 'axios'

export async function todo_get(route, parameters) {
    return request(route, parameters, null, 0);
}

export async function todo_post(route, parameters, body) {
    return request(route, parameters, body, 1);
}

export async function todo_put(route, parameters, body) {
    return request(route, parameters, body, 2);
}

async function request(route, parameters, body, httpMethod) {
    let counter = 0;

    while (counter < 2) {
        let result;

        switch (httpMethod) {
            case 0:
                result = await get(route, parameters);
                break;
            case 1:
                result = await post(route, parameters, body);
                break;
            case 2:
                result = await put(route, parameters, body);
                break;
            default:
                break;
        }

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

async function get(route, parameters) {
    try {
        let resp = await axios.get(route, { params: parameters });
        return resp;
    } catch (error) {
        return error.response;
    }
}

async function post(route, body, parameters) {
    try {
        let resp = await axios.post(route, body, { params: parameters, withCredentials: true });
        return resp;
    } catch (error) {
        return error.response;
    }
}

async function put(route, body, parameters) {
    try {
        let resp = await axios.put(route, body, { params: parameters, withCredentials: true });
        return resp;
    } catch (error) {
        return error.response;
    }
}