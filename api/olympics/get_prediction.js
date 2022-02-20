const { exec } = require("child_process");
const fs = require("fs");
const path = require("path");

const postPrediction = (req, res) => {
  const { params } = req.body;
  params = JSON.parse(params);
  let cmd = "python3 normalizer.py ";
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
    res.json({ results: JSON.stringify(results) });
  });
  res.send();
};

module.exports = { postPrediction };
