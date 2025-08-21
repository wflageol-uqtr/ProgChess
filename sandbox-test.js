const taskCodes = [
    "console.log('Hello from task 1!');",
    "setTimeout(() => console.log('This runs after 1s'), 1000);",
    "console.log('2 + 2 =', 2 + 2);",
    "setTimeout(() => console.log('This runs after 5s'), 5000);",
    "[...Array(3)].forEach((_,i)=>console.log('Item',i));",
    "console.log('Hello from task 6!');",
    "setTimeout(() => console.log('This runs after 6s'), 6000);",
];

const executeAsyncTask = async (code) => {
    const res = await fetch("http://localhost:3000/run", { 
        method: "POST",
        headers: { "Content-Type": "application/json" }, 
        body: JSON.stringify({ code })
    });
    return await res.json();
}

const executeAllAsyncTasks = async () => {
    const responses = await Promise.all(
        taskCodes.map((code) => executeAsyncTask(code))
    );
    console.log(responses);
};

executeAllAsyncTasks();
