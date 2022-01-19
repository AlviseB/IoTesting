const sensorModel = require("./src/api/sensor/sensor.model");
const droneModel = require("./src/api/drone/drone.model");

let subscriptions = [];

const mongoose = require("mongoose");
mongoose.connect("mongodb://localhost:27017/HTTPDrone", {
    useNewUrlParser: true,
    useUnifiedTopology: true,
});

const amqp = require('amqplib/callback_api');

amqp.connect(/* look at environment */'', function (error0, connection) {
    if (error0) {
        throw error0;
    }
    connection.createChannel(function (error1, channel) {
        if (error1) {
            throw error1;
        }

        exchange = "AMQP-DRONE";
        queue = "dr1_42"

        channel.assertExchange(exchange, 'topic')
        channel.assertQueue(queue);
        channel.bindQueue(queue, exchange, `${queue}.#`);

        channel.assertQueue("server");
        channel.assertExchange("AMQP-SERVER", 'direct')
        channel.bindQueue("server", "AMQP-SERVER");


        setInterval(() => {
            const drone = "dr1_42"
            channel.publish("AMQP-DRONE", `${drone}.command`, Buffer.from('{"command": "hold"}'))
        }, 5000);

            console.log(" [*] Waiting for messages in %s. To exit press CTRL+C", queue);
        channel.consume("server", function (msg) {
            drone = JSON.parse(msg.content);
            droneModel.create(drone).then((v) => console.log(v))
            console.log(" [x] Received %s", drone);
        }, {
            noAck: true
        });
    });
});

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

