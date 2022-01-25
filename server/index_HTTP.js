const restify = require("restify");
const morgan = require("morgan");
const droneController = require("./src/api/drone/drone.controller");

const mongoose = require("mongoose");
mongoose.connect("mongodb://localhost:27017/HTTPDrone", {
  useNewUrlParser: true,
  useUnifiedTopology: true,
});

var server = restify.createServer();
server.use(restify.plugins.bodyParser());
server.use(morgan("tiny"));

server.get("/drones", droneController.getAll);

server.get("/drones/:id", droneController.getOne);

server.get("/drones/:id/action", droneController.getAction);

server.post("/drones/:id/action", droneController.postAction);

server.post("/drones", droneController.post);

server.on("restifyError", function (req, res, err, callback) {
  // this will get fired first, as it's the most relevant listener
  return callback();
});

server.listen(8011, function () {
  console.log("%s listening at %s", server.name, server.url);
});
