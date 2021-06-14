import React, {useRef, useState} from 'react'

export default function TaskForm({addTask}) {
    const newTask = useRef();
    const [disabled, setDisabled] = useState(false); // disable the form
    // This option is for prevent multiple sending

    const handleSubmit = async (e) => {
        e.preventDefault();
        const task = newTask.current.value;
        if(task.trim !== '' && !disabled){
            setDisabled(true);
            if(await addTask(task)){
                newTask.current.value = null;
            } else {
                // Return error
            }
            setDisabled(false);
        }
    }

    return (
        <div>
            <form onSubmit={handleSubmit}>
                <div className="input-group mb-3">
                    <input ref={newTask} type="text" className="form-control" placeholder="Task description" disabled={disabled}/>
                    <button className="btn btn-success" type="submit" id="button-addon2">Add Task</button>
                </div>
            </form>
        </div>
    )
}
