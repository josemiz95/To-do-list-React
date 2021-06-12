import React, {useRef} from 'react'

export default function TaskForm({addTask}) {
    const newTask = useRef();

    const handleSubmit = (e) => {
        e.preventDefault();
        const task = newTask.current.value;
        if(task.trim !== ''){
            addTask(task);
            newTask.current.value = null;
        }
    }

    return (
        <div>
            <form onSubmit={handleSubmit}>
                <div className="input-group mb-3">
                    <input ref={newTask} type="text" className="form-control" placeholder="Task description"/>
                    <button className="btn btn-success" type="submit" id="button-addon2">Add Task</button>
                </div>
            </form>
        </div>
    )
}
