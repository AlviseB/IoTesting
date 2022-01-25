const sensorModel = require("./src/api/sensor/sensor.model");
const droneModel = require("./src/api/drone/drone.model");

let subscriptions = [];

const mongoose = require("mongoose");
mongoose.connect("mongodb://localhost:27017/HTTPDrone", {
    useNewUrlParser: true,
    useUnifiedTopology: true,
});

const amqp = require('amqplib/callback_api');

amqp.connect(process.env.AMQP, function (error0, connection) {
    if (error0) {
        throw error0;
    }
    connection.createChannel(function (error1, channel) {
        if (error1) {
            throw error1;
        }
        //topic exchange for drones
        let droneExchange = "AMQP-DRONE";
        //drone queue example
        let queue = "dr1_42"

        channel.assertExchange(droneExchange, 'topic')
        channel.assertQueue(queue);
        channel.bindQueue(queue, droneExchange, `${queue}.#`);


        //direct exchange for server
        let serverExchange = "AMQP-SERVER"
        //server queue
        let serverQueue = "server"
        channel.assertQueue(serverQueue);
        channel.assertExchange(serverExchange, 'direct')
        channel.bindQueue(serverQueue, serverExchange);


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