import { db } from "../../database.mjs";

// Route: /:id/register/
export default (req, res) => {
  const id = req.params.id;
  // Check for existing client
  db.findOne({ clientId: id }, (err, record) => {
    if (record) {
      res.status(409).json({success: false, message: "The specified client ID is already registered."})
      return;
    }

    console.log("Creating client with ID " + req.params.id);
    db.insert({
      clientId: id,
      clientHostname: req.query.clientHostname,
      clientDomain: req.query.clientDomain,
    });
    res.status(201).json({success: true});
  });
}