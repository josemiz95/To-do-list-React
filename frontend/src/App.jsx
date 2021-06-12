import React, {useState} from "react";
import { v4 as uuid } from "uuid";
// Components
import List from "./components/List";
import TaskForm from "./components/TaskForm";


export default function App() {
  const [showingPending, setShowingPending] = useState(true); // True show pending task, False show completed task
  const [tasks, setTasks] = useState([ ]);

  const toggleTaskList = () => {
    setShowingPending(!showingPending); // Action for toggle list
  }

  const list = tasks.filter((task)=>{ // List of showing tasks
    return task.pending === showingPending;
  });

  const addTask = (task) => { // Funtion to add task
    setTasks((prevTasks)=>{
      const newTask = {id:uuid(),description:task,pending:true}
      return [...prevTasks, newTask];
    })
  }

  return (
    <div className="container">
      <h1 className="mb-3">To Do List</h1>
      <TaskForm addTask={addTask}/>
      <button className="btn btn-primary mb-3" onClick={toggleTaskList}>{showingPending?'Toggle to completed task':'Toggle to Pending task'}</button>
      <List tasks={list}/>
    </div>
  );
}
