import { db } from "../../database.mjs";

// Route: POST /:id/set_updates/
export default (req, res) => {
  const id = req.params.id;

  console.log("UPDATE CLEARED FOR " + id);
  res.json({success: true});
    /*db.update({ clientId: id }, { $set: { phase: phase } }, {}, (err, updated) => {
      if (err) {
        res.status(500).json({ success: false, message: err });
      }

      res.json({success: true});
    });*/
}