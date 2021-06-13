﻿using api.Models;
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
            var taskFind = context.Tasks.FirstOrDefault(t => t.id == task.id); // Find task by id to prevent duplicate key

            if (ModelState.IsValid && taskFind == null)
            {
                context.Tasks.Add(task); // Add task to Database
                context.SaveChanges();

                return new CreatedAtRouteResult("GetById", new { id = task.id }, task);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromBody] Task task, int id)
        {
            var taskFind = context.Tasks.FirstOrDefault(Task => Task.id == id); // Find task by id

            if (taskFind == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid && task.id == id)
            {
                context.Entry(task).State = EntityState.Modified; // Modify Task
                context.SaveChanges();

                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var task = context.Tasks.FirstOrDefault(x => x.id == id); // Find task by id to delete

            if (task == null)
            {
                return NotFound();
            }

            context.Tasks.Remove(task); // Deleting from Database
            context.SaveChanges();

            return Ok(task);
        }
    }
}
