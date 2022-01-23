using Microsoft.AspNetCore.Mvc;
using Repository.Contracts;
using Repository.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository taskRepository; // DB contex for manipulations

        public TasksController(ITaskRepository taskRepository)
        {
            this.taskRepository = taskRepository; // Setting up the context
        }

        [HttpGet] // RETURNING LIST OF TASKS
        public IActionResult Get()
        {
            try
            {
                return Ok(taskRepository.All()); // Getting list of task
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet("{id}", Name = "GetById")]  // RETURNING A TASK
        public IActionResult GetById(int id)
        {
            try
            {

                var task = taskRepository.Find(id); // Find task by id

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
                    taskRepository.Create(task); // Add task to Database
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
                    if (!taskRepository.Exists(id)) // Exists?
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
                if (!taskRepository.Exists(id)) // Exists?
                {
                    return NotFound();
                }

                var task = taskRepository.Find(id);

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
