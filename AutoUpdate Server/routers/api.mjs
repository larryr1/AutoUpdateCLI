import express from 'express';
import { db } from '../database.mjs';
import idRoute from './routes/idRoute.mjs';
import idDeregisterRoute from './routes/idDeregisterRoute.mjs';
import idRegisterRoute from './routes/idRegisterRoute.mjs';

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

router.get("/:id/", idRoute);
router.get("/:id/register", idRegisterRoute);
router.get("/:id/deregister", idDeregisterRoute);