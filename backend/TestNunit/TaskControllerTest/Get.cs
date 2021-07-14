using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Repositories;
using api.Controllers;
using Moq;

namespace TaskControllerTest
{
    public class Get
    {
        private Mock<ITasks<Task, int>> taskRepo;

        [SetUp]
        public void Setup()
        {
            this.taskRepo = new Mock<ITasks<Task, int>>();
        }


        [Test]
        public void Test_Get_All_Task()
        {
            // Testing getting an array of tasks

            Task[] testTasks = new Task[] { // Array of task
                 new Task() { id = 1, description = "Descripcion", pending = true },
                 new Task() { id = 2, description = "Descripcion 2", pending = false }
            };

            //Moq
            this.taskRepo.Setup(t => t.GetAll()).Returns(testTasks);

            TasksController controller = new TasksController(this.taskRepo.Object); // Controller

            var actionResult = controller.Get(); // Action to test
            var result = actionResult as OkObjectResult;

            // Assert
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.Value, testTasks);

            this.taskRepo.Verify(t => t.GetAll());
        }

    }
}