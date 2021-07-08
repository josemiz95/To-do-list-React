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
        public IEnumerable<Task> Get()
        {
            return taskRepository.GetAll(); // Getting list of task
        }

        [HttpGet("{id}", Name = "GetById")]  // RETURNING A TASK
        public IActionResult GetById(int id)
        {
            var task = taskRepository.GetById(id); // Find task by id

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [HttpPost] // ADD TASK
        public IActionResult Post([FromBody] Task task)
        {

            if (ModelState.IsValid)
            {
                task.id = 0; // Prevent id duplicated
                task.date = null; // Prevent date

                taskRepository.Insert(task); // Add task to Database

                try
                {
                    taskRepository.Save();
                } 
                catch (DbUpdateConcurrencyException)
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

            if (!taskRepository.Any(id))
            {
                return NotFound();
            }

            if (ModelState.IsValid && task.id == id)
            {
                // task.date = !task.pending ? DateTime.Now : null;  For newer version

                if (task.pending) // Set time stamp
                {
                    task.date = null;
                }
                else
                {
                    task.date = DateTime.Now;
                }

                taskRepository.Update(task); // Modify Task

                try
                {
                    taskRepository.Save();
                }
                catch (DbUpdateConcurrencyException)
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
            return StatusCode(500);
        }
    }
}
