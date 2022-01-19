const sensorModel = require("./src/api/sensor/sensor.model");
const droneModel = require("./src/api/drone/drone.model");

let subscriptions = [];

const mongoose = require("mongoose");
mongoose.connect("mongodb://localhost:27017/HTTPDrone", {
    useNewUrlParser: true,
    useUnifiedTopology: true,
});

const coap = require('coap')
const bl = require('bl')

coap.createServer((req, res) => {
    console.log("fuck me")
    path = req.url.split("/");
    [_, root, droneID, endpoint] = path;
    method = req.method
    if (root == "drones")
        if (method == "POST") {
            req.pipe(bl((err, data) => {
                if (err != null) {
                    process.exit(1)
                } else {
                    const json = JSON.parse(data)
                    drone = droneModel.create(json)
                    console.log("created ", drone)
                }
            }))
            res.end()
        }
    if (method == "GET" && endpoint == "action") {
        res.end('{"command": "hold"}')
    }
}).listen(() => console.log("Running")/*{
    coap
        .request({
            pathname: '/Matteo/otherpath',
            options: {
                Accept: 'application/json'
            }
        })
        .on('response', (res) => {
            console.log('response code', res.code)
            if (res.code !== '2.05') {
                return process.exit(1)
            }

            res.pipe(bl((err, data) => {
                if (err != null) {
                    process.exit(1)
                } else {
                    const json = JSON.parse(data)
                    console.log(json)
                    process.exit(0)
                }
            }))
        })
        .end()
}*/)

/*
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

}, 1000);*/

