const mongoose = require('mongoose');

const DroneSchema = mongoose.Schema({
    ID: String,
    altitude: Number,
    speed: Number,
    battery: Number,
    gps: {
        latitude: Number,
        longitude: Number
    },
    orientation: [Number],
    timestamp: Date
});

module.exports = mongoose.model('Drone', DroneSchema);
