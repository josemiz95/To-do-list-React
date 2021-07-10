using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Repositories;
using api.Controllers;
using Moq;

namespace TestNunit
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_Create_Task()
        {
            
            Mock<ITasks<Task, int>> taskRepo = new Mock<ITasks<Task, int>>();

            taskRepo.Setup(t => t.Insert( It.IsAny<Task>() )).Returns( (Task task) => { return task; } );

            TasksController controller = new TasksController(taskRepo.Object);

            Task testTask = new Task()
            {
                id = 1,
                description = "Descripcion",
                pending = true
            };

            var result = controller.Post(testTask);

            IActionResult excepted = new CreatedAtRouteResult("GetById", new { id = testTask.id }, testTask);

            Assert.AreEqual(result, excepted);

            taskRepo.Verify(t => t.Insert(testTask));

        }
    }
}