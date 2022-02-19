const express = require("express");
const { getOlympicData } = require("./olympics/get_olympic_data");
const app = express();

app.use(express.static("data"));

app.get("/", (req, res) => res.send("hiii"));
app.get("/datatype/olympics/:filename", getOlympicData);
app.get("/datatype/olympics/", getOlympicData);

app.listen(8080);
