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

        [HttpGet] // RETURNING LIST OF TASKS
        public IEnumerable<Task> Get()
        {
            return context.Tasks.ToList(); // Getting list of task
        }

        [HttpGet("{id}", Name = "GetById")]  // RETURNING A TASK
        public IActionResult GetById(int id)
        {
            var task = context.Tasks.FirstOrDefault(Task => Task.id == id); // Find task by id

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

                context.Tasks.Add(task); // Add task to Database

                try
                {
                    context.SaveChanges();
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

            if (!TaskExists(id))
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

                context.Entry(task).State = EntityState.Modified; // Modify Task

                try
                {
                    context.SaveChanges();
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
            var task = context.Tasks.FirstOrDefault(x => x.id == id); // Find task by id to delete

            if (task == null)
            {
                return NotFound();
            }

            context.Tasks.Remove(task); // Deleting from Database

            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500);
            }

            return Ok(task);
        }

        private bool TaskExists(int id)
        {
            return context.Tasks.Any(e => e.id == id);
        }
    }
}
