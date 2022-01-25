const sensorModel = require("./src/api/sensor/sensor.model");
const droneModel = require("./src/api/drone/drone.model");

const mongoose = require("mongoose");
mongoose.connect("mongodb://localhost:27017/HTTPDrone", {
    useNewUrlParser: true,
    useUnifiedTopology: true,
});

const coap = require('coap')
const bl = require('bl')

coap.createServer((req, res) => {
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
        console.log("sent command to drone:", droneID)
        res.end('{"command": "hold"}')
    }
}).listen(() => console.log("Running"))
