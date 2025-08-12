import { randomUUID } from 'crypto';
import express from 'express';
import path from 'path';
import tar from 'tar-stream';
import Docker from 'dockerode';
import { unlinkSync, writeFileSync, readFileSync } from 'fs';
import { PassThrough } from 'stream';
import EventEmitter from 'events';

const myEmitter = new EventEmitter();
const docker = new Docker({ socketPath: '/var/run/docker.sock' });
const MAIN_CONTANER_NAME = "/progchess_sandbox";
const LIVING_TIME = 120 // 2 minutes
const MAX_CONTAINER = 2;
const queue = [];

const app = express();
app.use(express.json());

function clearContainer() {
  docker.listContainers({ all: true}, async function (err, containers) {
    if (err) {
      console.error('Error while listing container', err);
      return;
    }
    
    const now = Math.floor(Date.now() / 1000);

    for (const container of containers) {      
      const containerAge = now - container.Created;
      if (containerAge > LIVING_TIME) {
        
        if (container.Names[0] === MAIN_CONTANER_NAME) continue;
        
        try {
          const containerToRemove = docker.getContainer(container.Id);
          if (container.State === "running") {
            await containerToRemove.stop();
          }
          await containerToRemove.remove();
        } catch (error) {
          console.error('Error as occured on container deletion', error);
          
        }
      }
    }

  });
}

CreateCustomEvent()
setInterval(clearContainer, 200 * 1000);

app.get('/', function(req, res) {
    console.log("That is a GET Request");
    res.send("Helloo")
});

app.post('/run', async (req, res) => {
  return await processExecution(req, res);
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

async function CreateContainer(req, res) {
  const image = 'node:23-slim';
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
    return res.json({ IsSuccess: false, Error: error.toString() });
  } finally {    
    if (auxContainer) {
      await auxContainer.remove();
    }
    unlinkSync(filename);
  }
}

async function processExecution(req, res) {
  const size = (await docker.listContainers({all: true})).length;
  if (size < MAX_CONTAINER) {  
    myEmitter.emit('newContainer', req, res);
  } else {
    queue.push({ req, res });
    setTimeout(() => {
      if (!res.headersSent) {
        res.json({ IsSuccess: false, Error: 'Serveur plein, réessayer sous peu !' });
      }
      queue.shift();
      return;
    }, 20000);
  }
}

function CreateCustomEvent() {
  myEmitter.on('newContainer', async (req, res) => {    
    await CreateContainer(req, res);
    if (queue.length > 0) {
      const value = queue.shift();
      myEmitter.emit('newContainer', value.req, value.res);
    }    
  });
}