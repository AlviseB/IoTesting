const droneController = require("./src/api/drone/drone.controller");

let subscriptions = [];

const mongoose = require("mongoose");
mongoose.connect("mongodb://localhost:27017/HTTPDrone", {
  useNewUrlParser: true,
  useUnifiedTopology: true,
});

const mqtt = require('mqtt')
const client = mqtt.connect('mqtt://test.mosquitto.org')

const root = "iot2021"
const luogo = "thiene"

client.on('connect', function () {
  let topic = `${root}/${luogo}/#`;
  client.subscribe(topic, function (err) {
    subscriptions.push(topic)
    console.log("subscribed to #");
  })
})

client.on('message', function (topic, message) {
  // message is Buffer
  console.log(`Topic: ${topic} type: ${typeof(topic)}`)
  console.log(`Message: ${message} type: ${typeof(message)}`)
})

setInterval(() => {
  const drone = "dr1_42"
  client.publish(`${root}/${luogo}/${drone}/comando`, "esempio comando drone")

}, 1000);


process.on('SIGINT', function () {
  console.log('FanculoSecondo...');
  console.log(subscriptions.length);
  subscriptions.forEach((sub) => client.unsubscribe(sub))
  client.end();
  process.exit();
});
