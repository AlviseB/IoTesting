const express = require('express');
const router = express.Router();
const droneController = require('./drone.controller')

//definizione dele api relative ai log
router.post('/', droneController.create)

router.get('/', droneController.list)

router.get('/:id', droneController.getOne)

module.exports = router;