import React from 'react'

export default function ListItem({task, toggleTask}) {
    const icon = task.pending?'fas fa-check text-success':'fas fa-undo text-warning';

    const handleToggleTask = () => { // Toggle task funtion
        toggleTask(task.id);
    }

    return (
        <>
            <li className="list-group-item"><button className="btn" onClick={handleToggleTask}><i className={icon}></i></button> {task.description}</li>
        </>
    )
}
