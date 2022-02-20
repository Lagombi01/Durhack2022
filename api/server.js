const express = require("express");
const multer = require("multer");
const { getOlympicData } = require("./olympics/get_olympic_data");
const { postOlympicData } = require("./olympics/post_olympic_data");
const { getOlympicGraph } = require("./olympics/get_olympic_graph");
const path = require("path");
const app = express();

const bodyParser = require("body-parser");

//use bodyParser() to let us get the data from a POST
app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());

const storage = multer.diskStorage({
  destination: (req, f, cb) => {
    const dest = req.params.dest;
    cb(null, `./data/`);
  },
  filename: (req, file, cb) => {
    console.log(file, "called filename");
    if (file.mimetype === "text/csv") {
      cb(null, file.originalname);
    } else {
      cb(null, false);
    }
  },
});

app.use(express.static("data"));

const maxSize = 50 * 1000000;
const upload = multer({
  storage: storage,
  limits: { fileSize: maxSize },
});

app.get("/", (req, res) => res.send("hiii"));
app.get("/datatype/olympics/:filename", getOlympicData);
app.get("/datatype/olympics/", getOlympicData);
app.post("/datatype/olympics/", upload.single("file"), postOlympicData);

app.get("/graphtype/olympics/", getOlympicGraph);
app.post("/sentimental/poll", (req, res) => {
  console.log(req.body);
});

app.post("/probability/query", (req, res) => {
  console.log(req.body);
});

app.listen(8080);
