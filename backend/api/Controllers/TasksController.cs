using api.Models;
using api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = api.Models.Task;

namespace api.Controllers
{
    [Route("api/[controller]")] // Route for the Task api
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITasks<Task, int> taskRepository; // DB contex for manipulations

        public TasksController(ITasks<Task, int> taskRepository)
        {
            this.taskRepository = taskRepository; // Setting up the context
        }

        [HttpGet] // RETURNING LIST OF TASKS
        public IActionResult Get()
        {
            try
            {
                return Ok(taskRepository.GetAll()); // Getting list of task
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet("{id}", Name = "GetById")]  // RETURNING A TASK
        public IActionResult GetById(int id)
        {
            try {

                var task = taskRepository.GetById(id); // Find task by id

                if (task == null)
                {
                    return NotFound();
                }

                return Ok(task);

                // return task == null? NotFound() : Ok(task);  For newer version

            }
            catch
            {
                return StatusCode(500);
            }

        }

        [HttpPost] // ADD TASK
        public IActionResult Post([FromBody] Task task)
        {
            if (ModelState.IsValid && task != null)
            {

                try
                {
                    taskRepository.Insert(task); // Add task to Database
                } 
                catch
                {
                    return StatusCode(500);
                }

                return new CreatedAtRouteResult("GetById", new { id = task.id }, task);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")] // UPDATE TASK
        public IActionResult Put([FromBody] Task task, int id)
        {
            if (ModelState.IsValid && task != null && task.id == id)
            {
                try
                {
                    if (!taskRepository.Any(id)) // Exists?
                    {
                        return NotFound();
                    }

                    taskRepository.Update(task); // Modify Task
                }
                catch
                {
                    return StatusCode(500);
                }

                return new CreatedAtRouteResult("GetById", new { id = task.id }, task);
            }

            return BadRequest();
        }

        [HttpDelete("{id}")] // DELETE TASK
        public IActionResult Delete(int id)
        {
            try
            {
                if (!taskRepository.Any(id)) // Exists?
                {
                    return NotFound();
                }

                var task = taskRepository.GetById(id);

                taskRepository.Delete(task);
            } 
            catch
            {
                return StatusCode(500);
            }

            return Ok();

        }
    }
}
