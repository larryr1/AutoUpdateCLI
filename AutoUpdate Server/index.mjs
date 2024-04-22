import express from 'express';
const app = express();

app.use(express.json());

app.get("/", (req, res) => {
  res.send("OK!");
});

import { router as apiRouter } from './routers/api.mjs';
app.use("/api", apiRouter);

app.listen(8000);