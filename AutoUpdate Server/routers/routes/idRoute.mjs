import { db } from "../../database.mjs";

// Route: /:id/
export default (req, res) => {
  db.findOne({ clientId: req.params.id }, (err, record) => {

    if (err) {
      res.status(500).json({ success: false, message: err });
      return;
    }

    if (record) {
      res.status(200).json(record);
      return;
    }

    res.status(404).json({ success: false, message: "The requested client is not registered."});
  });
}