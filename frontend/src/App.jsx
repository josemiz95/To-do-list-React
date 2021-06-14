import React, {useState, useEffect} from "react";
// Components
import List from "./components/List";
import TaskForm from "./components/TaskForm";

const app_url = "https://localhost:44396"; // App base url where is the api

export default function App() {
  const [showingPending, setShowingPending] = useState(true); // True show pending task, False show completed task
  const [tasks, setTasks] = useState([ ]);

  /*
  * Notes:
  * On fetch functions when the petition return error, it's possible to show an error
   */

  // At start
  useEffect( () => {

    async function getData(){ // Get data from server
      const options = { // Petition options
        method: 'GET',
        headers: {'Content-Type':'application/json'}
      }
      
      await fetch(app_url+'/api/tasks',options) // Petition
        .then(response => response.json())
        .then(data =>{ // Adding tasks to List
  
          setTasks(data);
  
        }).catch(data =>  false); // Error
  
        return true;
    }
    getData();

  }, [])

  const toggleTaskList = () => {
    setShowingPending(!showingPending); // Action for toggle list
  }

  const list = tasks.filter((task) => task.pending === showingPending ); // List of showing tasks

  const addTask = async (description) => { // Funtion to add task
    const newTask = {description,pending:true}
    
    const options = { // Petition options
      method: 'POST',
      headers: {'Content-Type':'application/json'},
      body: JSON.stringify(newTask)
    }

    await fetch(app_url+'/api/tasks',options) // Petition
      .then(response => response.json())
      .then(data =>{ // Adding task to List

        setTasks((prevTasks) =>  [...prevTasks, data] );

      }).catch(data => false); // Error

      return true;
    
  }

  const toggleTask = async (id) => { 
    // Toggle pending task (True|False)
    const updatedTask = [...tasks];
    const toggledTask = updatedTask.find((task)=> task.id === id );
    toggledTask.pending = !toggledTask.pending; // Toggle peding attribute

    const options = { // Petition options
      method: 'PUT',
      headers: {'Content-Type':'application/json'},
      body: JSON.stringify(toggledTask)
    }

    await fetch(app_url+'/api/tasks/'+id,options) // Petition
      .then(response => response.json())
      .then(data =>{ // Toggle task

        setTasks(updatedTask);

      }).catch(data => false); // Error

      return true;
  }

  return (
    <div className="container">
      <h1 className="mb-3">To Do List</h1>
      <TaskForm addTask={addTask}/>
      <button className="btn btn-primary mb-3" onClick={toggleTaskList}>{showingPending?'Toggle to completed task':'Toggle to Pending task'}</button>
      <List tasks={list} toggleTask={toggleTask}/>
    </div>
  );
}
