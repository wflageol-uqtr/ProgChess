import { exec } from 'child_process';


exec("cd /app", (error, stdout, stderr) => {
  if (error) {
    console.error("erreur de dans le cd");
    return;
  }
  exec("ls -al", (error, stdout, stderr) => {
    if (error) {
      console.error("erreur de dans le cd");
      return;
    }
    console.log(stdout);
  })
});

// import { exec } from 'child_process';


// exec("cd /app", (error, stdout, stderr) => {
//   if (error) {
//     console.error("erreur de dans le cd");
//     return;
//   }
//   console.log(`stdout: ${stdout}`);
//   exec("ls -al", (error, stdout, stderr) => {
//     if (error) {
//       console.error("erreur de dans le cd");
//       return;
//     }
//     exec('rm index.js', (error, stdout, stderr) => {
//     if (error) {
//       console.error("erreur de dans le cd");
//       return;
//     }
//     console.log('stdout', stdout);
//      exec("ls -al", (error, stdout, stderr) => {
//       if (error) {
//       console.error("erreur de dans le cd");
//       return;
//     }
//     console.log(stdout);
    
//      })
//   })
//   })
// });