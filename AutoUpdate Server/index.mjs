import express from 'express';
const app = express();

app.get("/", (req, res) => {
  res.send("OK!");
});

import { router as apiRouter } from './routers/api.mjs';
app.use("/api", apiRouter);

app.listen(8000);