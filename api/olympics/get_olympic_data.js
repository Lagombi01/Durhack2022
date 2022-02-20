const fs = require("fs");
const path = require("path");
const getOlympicData = (req, res) => {
  try {
    const filename = req.params.filename;
    const extension = "../data/";
    if (filename) {
      const data = fs.readFileSync(path.join(__dirname, extension + filename));
      res.send(data);
    } else {
      res.send(getAllFilesInDirectory(path.join(__dirname, extension)));
    }
  } catch (err) {
    // console.log();
    console.log(err);
  }
};
const getAllFilesInDirectory = (directory) => {
  try {
    const files = fs.readdirSync(directory);
    let html = "<h1>Olympics Data </h1>";
    for (let file of files) {
      html += `<a href='${file}'>${file}</a><br>`;
    }
    return html;
  } catch (err) {
    console.error(err);
  }
};
module.exports = {
  getOlympicData,
};
