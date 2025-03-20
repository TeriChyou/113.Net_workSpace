async function fetchTasks() {
    let response = await fetch('/api/TaskAPI');
    let data = await response.json();
    document.getElementById("output").innerText = JSON.stringify(data, null, 2);
}

async function addTask() {
    let newTask = prompt("Enter a new task:");
    if (!newTask) return;

    let response = await fetch('/api/TaskAPI', {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(newTask)
    });

    let result = await response.json();
    alert(result.message);
    fetchTasks(); // Refresh task list
}

async function removeTask() {
    let index = prompt("Enter the index of task to remove.");
    if (index === null || index === "") return; // Handle cancel

    let response = await fetch(`/api/TaskAPI/${index - 1}`, { // ✅ Correct URL with ID
        method: "DELETE"
    });

    let result = await response.json();
    alert(result.message); // Show success or error message
    fetchTasks(); // Refresh task list
}