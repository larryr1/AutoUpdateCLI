import { db } from "../../database.mjs";

// Route: /:id/deregister/
export default (req, res) => {
  db.remove({ clientId: req.params.id }, (err, record) => {
    if (record === 0) {
      res.status(404).json({ success: false, message: "The specified client was not found." });
      return;
    }

    if (err) {
      res.status(500).json({ success: false, message: err });
      return;
    }

    res.status(200).json({ success: true });
    return;
  });
}