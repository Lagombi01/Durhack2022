const fs = require("fs");
const path = require("path");
const getOlympicData = (req, res) => {
    try{
    const filename = req.params.filename;
    
    if (filename){
        const data = fs.readFileSync(filename);
        res.send(data);
    }
    else{
        res.send(getAllFilesInDirectory(path.join(__dirname,"/data")))
    }
    }catch(err){
        console.log(err)
    }
};
const getAllFilesInDirectory = (directory) => {
    try{
         const files = fs.readdirSync(directory);
        let html = "<h1>Olympics Data </h1>";
        for(let file of files){
            html += `<a href='${file}'>${file}</a>`;
        };
        return html;
    }
    catch(err){
        console.error(err);
    };
};
module.exports = {
    getOlympicData
}