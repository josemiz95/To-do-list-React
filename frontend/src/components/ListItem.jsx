import React from 'react'

export default function ListItem({task}) {
    return (
        <>
            <li className="list-group-item"><i className="fas fa-check"></i> {task.description}</li>
        </>
    )
}
