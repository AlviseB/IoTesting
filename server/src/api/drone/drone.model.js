const Drone = require('./drone.schema');

module.exports.create = async (data) => {
    const drone = await Drone.create(data)
    return drone
}

module.exports.list = async (query) => {
    const filter = {}
    /* if (query.inizio || query.fine) {
        filter.data = {}
        if (query.inizio)
            filter.data.$gte = query.inizio
        if (query.fine)
            filter.data.$lte = query.fine
    }
    if (query.commessa)
        filter.commessa = query.commessa
    if (query.stato)
        filter.stato = query.stato */
    const drones = await Drone.find(filter)
    return drones
}

module.exports.drone = async (ID) => {
    const drone = await Drone.findOne({ ID: ID }).sort({ timestamp: -1 })
    return drone
}
