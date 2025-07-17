import { exec } from 'child_process';
import { randomUUID } from 'crypto';
import express from 'express';
import { unlinkSync, writeFileSync } from 'fs';
import path from 'path';
const app = express();
app.use(express.json());

app.get('/', function(req, res) {
    console.log("That is a GET Request");
    res.send("Helloo")
});

app.post('/run', (req, res) => {  
  const filename = path.join('/tmp', `${randomUUID()}.mjs`);
  writeFileSync(filename, "import { exec } from 'child_process';exec(\'cd /app\', (error, stdout, stderr) => { if (error) { console.error(\'erreur de dans le cd\'); return; } console.log(`stdout: ${stdout}`); exec(\'ls -al\', (error, stdout, stderr) => { if (error) { console.error(\'erreur de dans le cd\'); return; } exec(\'rm index.js\', (error, stdout, stderr) => { if (error) { console.error(\'erreur de dans le cd\'); return; } console.log(\'stdout\', stdout); exec(\'ls -al\', (error, stdout, stderr) => { if (error) { console.error(\'erreur de dans le cd\'); return; } console.log(stdout); }); }); }); });");
  // writeFileSync(filename, req.body.code);
  exec(`node --experimental-modules ${filename}`, (error, stdout, stderr) => {
    unlinkSync(filename);
    console.log(stdout);
    
    if (error) {
      res.json({ success: false, error: error }); 
    } else {
      res.json({ success: true, result: stdout });
    }
  })
});

app.listen(3000, function() {
    console.log("Listen on port 3000");
});