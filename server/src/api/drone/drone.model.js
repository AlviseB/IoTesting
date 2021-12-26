const Drone = require('./drone.schema');

module.exports.create = async (data) => {
    const drone = await Drone.create(data)
    return drone
}

module.exports.list = async (query) => {
    const filter = {}
    const drones = await Drone.find(filter)
    return drones
}

module.exports.drone = async (ID) => {
    const drone = await Drone.findOne({ ID: ID }).sort({ timestamp: -1 })
    return drone
}
