import { db } from "../../database.mjs";

// Route: /:id/phase/
export default (req, res) => {
  const id = req.params.id;
  const phase = req.query.phase;
  
  console.log(`Setting phase for ${id} to ${phase}`);
    db.update({ clientId: id }, { $set: { phase: phase } }, {}, (err, updated) => {
      if (err) {
        res.status(500).json({ success: false, message: err });
      }

      res.json({success: true});
    });
}