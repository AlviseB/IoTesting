const mongoose = require('mongoose');

const SensorSchema = mongoose.Schema({
    droneID: String,
    sensorType: String,
    value: Object,
    timestamp: Date
});

module.exports = mongoose.model('Sensor', SensorSchema);
