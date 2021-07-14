using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Repositories;
using api.Controllers;
using Moq;

namespace TaskControllerTest
{
    public class GetById
    {
        private Mock<ITasks<Task, int>> taskRepo;

        [SetUp]
        public void Setup()
        {
            this.taskRepo = new Mock<ITasks<Task, int>>();
        }

        [Test]
        public void Test_Get_A_Task()
        {
            // Testing getting an array of tasks

            Task testTask = new Task() { id = 1, description = "Descripcion", pending = true }; // Task

            //Moq
            this.taskRepo.Setup(t => t.GetById( It.IsAny<int>() )).Returns((int id)=> { 
                                                                            if(id == testTask.id) { return testTask; }
                                                                            else { return null; }
                                                                       });

            TasksController controller = new TasksController(this.taskRepo.Object); // Controller

            var actionResult = controller.GetById(1); // Action to test
            var result = actionResult as OkObjectResult;

            // Assert
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.Value, testTask);

            this.taskRepo.Verify(t => t.GetById(1));
        }

        [Test]
        public void Test_Get_A_Task_NotFound()
        {
            // Testing getting an array of tasks

            Task testTask = new Task() { id = 1, description = "Descripcion", pending = true }; // Task

            //Moq
            this.taskRepo.Setup(t => t.GetById(It.IsAny<int>())).Returns((int id) => {
                if (id == testTask.id) { return testTask; }
                else { return null; }
            });

            TasksController controller = new TasksController(this.taskRepo.Object); // Controller

            var actionResult = controller.GetById(2); // Action to test
            var result = actionResult as NotFoundResult;

            // Assert
            Assert.AreEqual(result.StatusCode, 404);

            this.taskRepo.Verify(t => t.GetById(2));
        }
    }
}