import React, {useState} from "react";
import { v4 as uuid } from "uuid";
// Components
import List from "./components/List";


export default function App() {
  const [showingPending, setShowingPending] = useState(true); // True show pending task, False show completed task
  const [tasks, setTasks] = useState([
    {id:uuid(), description:'Something to do', done:false}
  ]);

  const toggleTaskList = () => {
    setShowingPending(!showingPending); // Action for toggle list
  }

  return (
    <div className="container">
      <h1 className="mb-3">To Do List</h1>
      <button className="btn btn-primary mb-3" onClick={toggleTaskList}>{showingPending?'Toggle to completed task':'Toggle to Pending task'}</button>
      <List tasks={tasks}/>
    </div>
  );
}
