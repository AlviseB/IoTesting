const droneModel = require('./drone.model');
const moment = require('moment')

let actions = {
    "dr1_42": [
        {
            "command": "idle"
        },
        {
            "command": "move",
        }]
}

module.exports.post = async (req, res, next) => {
    try {
        const newDrone = await droneModel.create(req.body)
        res.status(201)
        res.json(newDrone)
        next()
    }
    catch (err) {
        next(err)
    }
}

module.exports.getAll = async (req, res, next) => {
    try {
        const query = {}
        /* if (req.query.commessa)
            query.commessa = req.query.commessa
        if (req.query.stato)
            query.stato = req.query.stato
        if (req.query.inizio)
            query.inizio = moment(req.query.inizio).hours(0).minutes(0).seconds(0).milliseconds(0)
        if (req.query.fine)
            query.fine = moment(req.query.fine).add(1, 'days').hours(0).minutes(0).seconds(0).milliseconds(0) */
        const drones = await droneModel.list(query)
        res.status(200)
        res.json(drones)
        //res.send("hello world")
        next()
    }
    catch (err) {
        next(err)
    }
}

module.exports.getOne = async (req, res, next) => {
    try {
        let id = req.params.id
        const drone = await droneModel.drone(id)
        if (drone == null)
            throw new Error("Not Found")
        res.status(200)
        res.json(drone)
    }
    catch (err) {
        next(err)
    }
}

module.exports.getAction = async (req, res, next) => {
    try {
        let id = req.params.id
        if (actions[id].length > 0) {
            let action = actions[id].shift()
            res.status(200)
            res.json(action)
        }
        else {
            res.status(200)
            res.json({ "command": "no-action" })
        }
    }
    catch (err) {
        next(err)
    }
}

module.exports.postAction = async (req, res, next) => {
    try {
        let id = req.params.id
        let action = req.body
        actions[id].push(action)
        res.status(201)
        res.json(action)
    }
    catch (err) {
        next(err)
    }
}