import React from 'react'

export default function TaskForm() {
    return (
        <div>
            <form>
                <div className="input-group mb-3">
                    <input type="text" className="form-control" placeholder="Task description"/>
                    <button className="btn btn-success" type="submit" id="button-addon2">Add Task</button>
                </div>
            </form>
        </div>
    )
}
