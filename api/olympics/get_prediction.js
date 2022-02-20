const { exec } = require("child_process");
const fs = require("fs");
const path = require("path");

const postPrediction = (req, res) => {
  let { params } = req.body;
  console.log(req.body);
  console.log(params);
  params = JSON.parse(params);
  let cmd = "python ..\\Backend\\normalizer.py ";
  for (let param of params) {
    cmd += ` ${param}`;
  }
  exec(cmd, (err, stdout, stderr) => {
    if (err) {
      console.error("err", err);
      return;
    }

    console.log("normalizer output");
    const results = fs.readFileSync(
      path.join(__dirname, "../../Backend/results.txt")
    );
    res.status(200).json({ results: JSON.stringify(results) });
  });
};

module.exports = { postPrediction };
