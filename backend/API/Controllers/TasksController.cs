using API.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Repository.Contracts;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository; // DB contex for manipulations
        private readonly IMapper _mapper;


        public TasksController(ITaskRepository taskRepository, IMapper mapper)
        {
            this._taskRepository = taskRepository; // Setting up the context
            this._mapper = mapper;
        }

        [HttpGet] // RETURNING LIST OF TASKS
        public IActionResult Get()
        {
            try
            {
                var tasksList = _taskRepository.All();
                var model = this._mapper.Map<List<Task>, List<TaskVM>>(tasksList.ToList());
                return Ok(model); // Getting list of task
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("{id}", Name = "GetById")]  // RETURNING A TASK
        public IActionResult GetById(int id)
        {
            try
            {

                var task = _taskRepository.Find(id); // Find task by id

                if (task == null)
                {
                    return NotFound();
                }

                var model = this._mapper.Map<Task, TaskVM>(task);

                return Ok(model);
            }
            catch
            {
                return StatusCode(500);
            }

        }

        [HttpPost] // ADD TASK
        public IActionResult Post([FromBody] TaskVM task)
        {
            if (ModelState.IsValid && task != null)
            {
                try
                {
                    var model = this._mapper.Map<TaskVM, Task>(task);

                    var createdTask = _taskRepository.Create(model); // Add task to Database

                    var taskModel = this._mapper.Map<Task, TaskVM>(createdTask);

                    return new CreatedAtRouteResult("GetById", new { id = taskModel.id }, taskModel);
                }
                catch
                {
                    return StatusCode(500);
                }
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
                    if (!_taskRepository.Exists(id)) // Exists?
                    {
                        return NotFound();
                    }

                    _taskRepository.Update(task); // Modify Task
                }
                catch
                {
                    return StatusCode(500);
                }

                return new CreatedAtRouteResult("GetById", new { id = task.id }, task);
            }

            return BadRequest();
        }

        [HttpPatch("{id}/done")] // UPDATE TASK
        public IActionResult PatchDone([FromBody] Task task, int id)
        {
            if (ModelState.IsValid && task != null && task.id == id)
            {
                try
                {
                    if (!_taskRepository.Exists(id)) // Exists?
                    {
                        return NotFound();
                    }

                    task.pending = false;
                    task.date = DateTime.Now;

                    _taskRepository.Update(task); // Modify Task
                }
                catch
                {
                    return StatusCode(500);
                }

                return new CreatedAtRouteResult("GetById", new { id = task.id }, task);
            }

            return BadRequest();
        }

        [HttpPatch("{id}/pending")] // UPDATE TASK
        public IActionResult PatchPending([FromBody] Task task, int id)
        {
            if (ModelState.IsValid && task != null && task.id == id)
            {
                try
                {
                    if (!_taskRepository.Exists(id)) // Exists?
                    {
                        return NotFound();
                    }

                    task.pending = true;
                    task.date = null;

                    _taskRepository.Update(task); // Modify Task
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
                if (!_taskRepository.Exists(id)) // Exists?
                {
                    return NotFound();
                }

                var task = _taskRepository.Find(id);

                _taskRepository.Delete(task);
            }
            catch
            {
                return StatusCode(500);
            }

            return Ok();

        }
    }
}
