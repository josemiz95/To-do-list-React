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
            return context.Tasks.ToList();
        }
    }
}
