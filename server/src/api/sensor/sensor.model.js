const Sensor = require('./sensor.schema');

module.exports.create = async (sensorData) => {
    const sensor = await Sensor.create(sensorData)
    return sensor
}

module.exports.list = async (query) => {
    const filter = query || {}
    const sensors = await Sensor.find(filter)
    return sensors
}
