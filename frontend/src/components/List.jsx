import React from 'react'

// Components
import ListItem from './ListItem';

export default function List({tasks, toggleTask}) {
    return (
        <>
            <ul className="list-group">
                {
                    tasks.map((task)=>{
                        return  <ListItem key={task.id} task={task} toggleTask={toggleTask}/>
                    })
                }
            </ul>
        </>
    )
}
