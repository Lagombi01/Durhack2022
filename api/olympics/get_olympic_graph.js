const { exec } = require("child_process");
const fs = require("fs");
const getOlympicGraph = (req, res) => {
  exec("python3 ..\\dummyPhoto.py", (err, stdout, stderr) => {
    if (err) {
      console.error(`exec error: ${err}`);
      return;
    }

    console.log(`Output ${stdout.trim()}`);
    res.writeHead(200, { "Content-Type": "image/png" });
    fs.createReadStream(stdout.trim()).pipe(res);
  });
};

module.exports = { getOlympicGraph };
