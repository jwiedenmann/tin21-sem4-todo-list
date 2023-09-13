const AUTH = "/api/v1/Authentication"
const AUTH_REFRESH = "/api/v1/Authentication/refresh"
const AUTH_REVOKE = "/api/v1/Authentication/revoke"

const LIST = "/api/v1/List"
const LIST_USER = "api/v1/List/user"

const LIST_ITEM = "/api/v1/ListItem"
const LIST_ITEM_DELETE = "/api/v1/ListItem/delete"
const LIST_ITEM_CHECK = "/api/v1/ListItem/check"
const LIST_ITEM_UNCHECK = "/api/v1/ListItem/uncheck"

const USER = "/api/v1/User"
const USER_SEARCH = "/api/v1/User/search"
const USER_EXISTS = "/api/v1/User/exists"

export default {
  AUTH: AUTH,
  AUTH_REFRESH: AUTH_REFRESH,
  AUTH_REVOKE: AUTH_REVOKE,
  LIST: LIST,
  LIST_USER: LIST_USER,
  USER: USER,
  USER_SEARCH: USER_SEARCH,
  USER_EXISTS: USER_EXISTS
}