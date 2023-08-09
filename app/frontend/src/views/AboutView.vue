<template>
  <div class="about">
    <h1>MQTT Over WebSockets</h1>
    <h2>Publisher</h2>
    <button @click="createConnection()">Connect</button>
    <div class="line"></div>
    <ul class="wrapper">
      <li class="form-row">
        <label for="topic">Topic</label>
        <input type="text" id="topic" />
      </li>
      <li class="form-row">
        <label for="message">Message</label>
        <input type="text" id="message" />
      </li>
      <li class="form-row">
        <button type="button" class="publish" @click="publishMessage()">Publish</button>
      </li>
    </ul>
    <div id="sub">
      <h2>Subscriber</h2>
    <div class="line"></div>
    <ul class="wrapper">
      <li class="form-row">
        <label for="topic">Topic</label>
        <input type="text" id="topic" />
      </li>
      <li class="form-row">
        <label for="status">Status</label>
        <input type="text" id="status" readonly />
      </li>
      <li class="form-row">
        <div class="btn-container">
          <button type="button" id="subscribe" @click="subscribeToTopic()">Subscribe</button>
          <button type="button" id="unsubscribe" @click="unsubscribeToTopic()">Unsubscribe</button>
        </div>
      </li>
    </ul>
    </div>
  </div>
</template>
<script>
const mqtt = require("precompiled-mqtt");
console.log(mqtt)
export default {
  data() {
    return {
      connection: {
        protocol: "ws",
        host: "localhost",
        // ws: 9001; mqtt: 1883
        port: 9001,
        endpoint: "/mqtt",
        // for more options, please refer to https://github.com/mqttjs/MQTT.js#mqttclientstreambuilder-options
        clean: true,
        connectTimeout: 30 * 1000, // ms
        reconnectPeriod: 4000, // ms
        clientId: "emqx_vue_" + Math.random().toString(16).substring(2, 8),
        // auth
        username: "user1",
        password: "1234",
      },
      subscription: {
        topic: "topic/mqttx",
        qos: 0,
      },
      publish: {
        topic: "topic/browser",
        qos: 0,
        payload: '{ "msg": "Hello, I am browser." }',
      },
      receiveNews: "",
      qosList: [0, 1, 2],
      client: {
        connected: false,
      },
      subscribeSuccess: false,
      connecting: false,
      retryTimes: 0,
    };
  },

  methods: {
    initData() {
      this.client = {
        connected: false,
      }
      this.retryTimes = 0
      this.connecting = false
      this.subscribeSuccess = false
    },
    handleOnReConnect() {
      this.retryTimes += 1;
      if (this.retryTimes > 5) {
        try {
          this.client.end();
          this.initData();
          this.$message.error("Connection maxReconnectTimes limit, stop retry");
        } catch (error) {
          this.$message.error(error.toString());
        }
      }
    },
    createConnection() {
      try {
        this.connecting = true;
        const { protocol, host, port, endpoint, ...options } = this.connection;
        const connectUrl = `${protocol}://${host}:${port}${endpoint}`;
        this.client = mqtt.connect(connectUrl, options);
        if (this.client.on) {
          this.client.on("connect", () => {
            this.connecting = false;
            console.log("Connection succeeded!");
          });
          this.client.on("reconnect", this.handleOnReConnect);
          this.client.on("error", (error) => {
            console.log("Connection failed", error);
          });
          this.client.on("message", (topic, message) => {
            this.receiveNews = this.receiveNews.concat(message);
            console.log(`Received message ${message} from topic ${topic}`);
          });
        }
        console.log('Connection succeeded')
      } catch (error) {
        this.connecting = false;
        console.log("mqtt.connect error", error);
      }
    },
    publishMessage(){
      const messageInput = document.querySelector("#message");

      const topic = document.querySelector("#topic").value.trim();
      const message = messageInput.value.trim();

      console.log(`Sending Topic: ${topic}, Message: ${message}`);

      this.client.publish(topic, message, {
        qos: 0,
        retain: false,
      });
      messageInput.value = "";
    },
    subscribeToTopic(){
      const status = document.querySelector("#status");
      const topic = document.querySelector("#topic").value.trim();
      console.log(`Subscribing to Topic: ${topic}`);

      this.client.subscribe(topic, { qos: 0 });
      status.style.color = "green";
      status.value = "SUBSCRIBED";
    },
    // subscribe topic
    // https://github.com/mqttjs/MQTT.js#mqttclientsubscribetopictopic-arraytopic-object-options-callback
    doSubscribe() {
      const { topic, qos } = this.subscription
      this.client.subscribe(topic, { qos }, (error, res) => {
        if (error) {
          console.log('Subscribe to topics error', error)
          return
        }
        this.subscribeSuccess = true
        console.log('Subscribe to topics res', res)
      })
    },
    unsubscribeToTopic(){
      const status = document.querySelector("#status");
      const topic = document.querySelector("#topic").value.trim();
      console.log(`Unsubscribing to Topic: ${topic}`);

      this.client.unsubscribe(topic, { qos: 0 });
      status.style.color = "red";
      status.value = "UNSUBSCRIBED";
    },
    // unsubscribe topic
    // https://github.com/mqttjs/MQTT.js#mqttclientunsubscribetopictopic-array-options-callback
    doUnSubscribe() {
      const { topic } = this.subscription
      this.client.unsubscribe(topic, error => {
        if (error) {
          console.log('Unsubscribe error', error)
        }
      })
    },
    // publish message
    // https://github.com/mqttjs/MQTT.js#mqttclientpublishtopic-message-options-callback
    doPublish() {
      const { topic, qos, payload } = this.publish
      this.client.publish(topic, payload, { qos }, error => {
        if (error) {
          console.log('Publish error', error)
        }
      })
    },
    // disconnect
    // https://github.com/mqttjs/MQTT.js#mqttclientendforce-options-callback
    destroyConnection() {
      if (this.client.connected) {
        try {
          this.client.end(false, () => {
            this.initData()
            console.log('Successfully disconnected!')
          })
        } catch (error) {
          console.log('Disconnect failed', error.toString())
        }
      }
    },
    handleProtocolChange(value) {
      this.connection.port = value === 'wss' ? '8084' : '8083'
    },
  },
};
</script>

