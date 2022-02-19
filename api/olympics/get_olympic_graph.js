
const { exec } = require('child_process');
const getOlympicGraph = (req,res) =>{
    

exec('find . -type f', (err, stdout, stderr) => {
  if (err) {
    console.error(`exec error: ${err}`);
    return;
  }

  console.log(`Number of files ${stdout}`);
});
}