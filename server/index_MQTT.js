const sensorModel = require("./src/api/sensor/sensor.model");

let subscriptions = [];

const mongoose = require("mongoose");
mongoose.connect("mongodb://localhost:27017/HTTPDrone", {
  useNewUrlParser: true,
  useUnifiedTopology: true,
});

const mqtt = require('mqtt')
const client = mqtt.connect('mqtt://test.mosquitto.org')

const root = "iot2021";
const version = "v1";
const zone = "thiene";

client.on('connect', function () {
  let topic = `${root}/${version}/${zone}/+/status/#`;
  client.subscribe(topic, function (err) {
    subscriptions.push(topic)
    console.log("subscribed to #");
  })
})

client.on('message', async function (topic, message) {
  // message is Buffer
  let [root, version, zone, droneID, status, sensorType] = topic.split("/");
  console.log(message.toString());
  data = JSON.parse(message.toString());
  sensorData = {};
  sensorData.droneID = droneID;
  sensorData.timestamp = (new Date()).toUTCString();
  sensorData.sensorType = sensorType;
  sensorData.value = data[sensorType];
  console.log(sensorData);
  res = await sensorModel.create(sensorData);
  console.log("Added: ", res);
})

setInterval(() => {
  const drone = "dr1_42"
  client.publish(`${root}/${version}/${zone}/${drone}/command`, '{"command": "none"}')

}, 1000);


process.on('SIGINT', function () {
  console.log('Unsubscribing...');
  console.log(subscriptions.length);
  subscriptions.forEach((sub) => client.unsubscribe(sub))
  client.end();
  console.log("Exited.");
  process.exit();
});
