using api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ApplicationDbContext context; // DB contex for manipulations

        public TasksController(ApplicationDbContext context)
        {
            this.context = context; // Setting up the context
        }

        [HttpGet]
        public IEnumerable<Task> Get()
        {
            return context.Tasks.ToList(); // Getting list of task
        }

        [HttpGet("{id}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var task = context.Tasks.FirstOrDefault(Task => Task.id == id); // Find task by id

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Task task)
        {
            if (ModelState.IsValid)
            {
                context.Tasks.Add(task);
                context.SaveChanges();

                return new CreatedAtRouteResult("GetById", new { id = task.id }, task);
            }

            return BadRequest(ModelState);
        }
    }
}
