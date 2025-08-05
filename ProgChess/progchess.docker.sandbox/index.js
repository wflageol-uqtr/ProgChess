import { randomUUID } from 'crypto';
import express from 'express';
import path from 'path';
import tar from 'tar-stream';
import Docker from 'dockerode';
import { unlinkSync, writeFileSync, readFileSync } from 'fs';
import { PassThrough } from 'stream';

const docker = new Docker({ socketPath: '/var/run/docker.sock' });

const app = express();
app.use(express.json());

app.get('/', function(req, res) {
    console.log("That is a GET Request");
    res.send("Helloo")
});

app.post('/run', async (req, res) => {
  const image = 'node:18-slim';
  const file = `${randomUUID()}.mjs`;
  const filename = path.join('/app/tmp', file)
  writeFileSync(filename, req.body.code);

  let auxContainer;

  try {
    await pullImage(image);

    const container = await docker.createContainer({
      Image: image,
      Cmd: ['node', `/tmp/script.mjs`],
      Tty: false,
      AttachStdout: true,
      AttachStderr: true,
    });
    auxContainer = container;

    const pack = tar.pack();
    pack.entry({ name: 'script.mjs'}, readFileSync(filename));
    pack.finalize();

    await container.putArchive(pack, { path: '/tmp' })

    const stdoutStream = new PassThrough();
    const stderrStream = new PassThrough();

    let stdoutData = '';
    stdoutStream.on('data', chunk => {
      stdoutData += chunk.toString();
    });

    let stderrData = '';
    stderrStream.on('data', chunk => {
      stderrData += chunk.toString();
    });

    container.attach({ stream: true, stdout: true, stderr: true }, function (err, stream) {
      container.modem.demuxStream(stream, stdoutStream, stderrStream);
    });

    await container.start();
    await container.wait();   

    if (stderrData) {
      return res.json({ IsSuccess: false, Error: stderrData});
    }
    return res.json({ IsSuccess: true, Output: stdoutData});
  } catch (error) {
    res.json({ IsSuccess: false, Error: error.toString() });
  } finally {
    if (auxContainer) {
      await auxContainer.remove();
    }
    unlinkSync(filename);
  }
});

app.post('/piston', async (req, res) => {
  const dataToSend = {
    language: "javascript",
    version: "18.15.0",
    files: [
      {
        name:`${randomUUID()}.mjs`,
        content: req.body.code
      }
    ]
  }
  const response = await fetch("https://emkc.org/api/v2/piston/execute", {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(dataToSend)
  });  
  const json = await response.json();

  if (json.run.stderr) {
    return res.send({ success: false, result: json.run.stderr });
  }

  res.send({ success: true, result: json.run.stdout });
});

app.listen(3000, () => {
  console.log('Listening on http://localhost:3000');
});

async function pullImage(imageName) {
  return new Promise((resolve, reject) => {
    docker.pull(imageName, (err, stream) => {
      if (err) return reject(err);

      docker.modem.followProgress(stream, onFinished, onProgress);

      function onFinished(err, output) {
        if (err) return reject(err);
        resolve(output);
      }

      function onProgress(event) {
        if (event.status) {
          console.log(`[Docker Pull] ${event.status}`);
        }
      }
    });
  });
}