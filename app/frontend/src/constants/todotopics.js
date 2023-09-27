const USER_TOPIC = "/todo/user/"
const LIST_TOPIC = "/todo/list/"
const CLIENT_UPDATE = "/todo/list/{listID}/update"
const SERVER_ACK = "/todo/list/{listID}/update/ack"

export default {
  USER_TOPIC: USER_TOPIC,
  LIST_TOPIC: LIST_TOPIC,
  CLIENT_UPDATE: CLIENT_UPDATE,
  SERVER_ACK: SERVER_ACK
}