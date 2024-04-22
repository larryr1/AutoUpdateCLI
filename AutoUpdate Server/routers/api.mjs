import express from 'express';
import { db } from '../database.mjs';
import idRoute from './routes/idRoute.mjs';
import idDeregisterRoute from './routes/idDeregisterRoute.mjs';
import idRegisterRoute from './routes/idRegisterRoute.mjs';
import idPhase from './routes/idPhase.mjs';
import idSetUpdates from './routes/idSetUpdates.mjs';
import idClearUpdates from './routes/idClearUpdates.mjs';

export const router = express.Router();

// Database

router.use((req, res, next) => {
  if (req.params.id) {
    console.log("Got API request from client ID " + req.params.id);
  } else {
    console.log("Got API request from client.");
  }

  return next();
});

router.get("/clients", (req, res) => {
  db.find({ clientId: {$exists: true} }, (err, records) => {
    if (err) {
      res.status(500).json({ success: false, message: err });
      return;
    }

    res.status(200).json(records);
  });
});

router.get("/client/:id/", idRoute);
router.get("/client/:id/register", idRegisterRoute);

router.use("/client/:id/*", (req, res, next) => {
  console.log("Params are " + JSON.stringify(req.params));
  db.findOne({ clientId: req.params.id }, (err, record) => {
    if (err) {
      res.status(500).json({ success: false, message: err });
      return;
    }

    if (!record) {
      res.status(404).json({success: false, message: "The specified client ID is not registered."})
      return;
    }

    return next();
  });
});

router.get("/client/:id/deregister", idDeregisterRoute);
router.get("/client/:id/phase", idPhase);
router.post("/client/:id/set_updates", idSetUpdates);
router.get("/client/clear_updates", idClearUpdates)