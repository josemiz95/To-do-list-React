using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Repositories;
using api.Controllers;
using Moq;
using TestNunit.TaskControllerTest;

namespace TaskControllerTest
{
    public class Post
    {
        private Mock<ITasks<Task, int>> taskRepo;

        [SetUp]
        public void Setup()
        {
            this.taskRepo = new Mock<ITasks<Task, int>>();
        }

        // Post FUNCTIONS
        [Test]
        public void Test_Create_Task()
        {
            // Testing creating an complete task, with all data

            Task testTask = new Task() { description = "Descripcion" }; // Task

            // Moq
            this.taskRepo.Setup(t => t.Insert(It.IsAny<Task>())).Returns((Task task) => { return task; });

            TasksController controller = new TasksController(this.taskRepo.Object); // Controller

            var actionResult = controller.Post(testTask); // Action to test
            var result = actionResult as CreatedAtRouteResult;
            bool isValid = ValidateTask.Validate(testTask, ref controller);


            // Assert
            Assert.AreEqual(result.StatusCode, 201);
            Assert.AreEqual(result.Value, testTask);

            Assert.IsTrue(isValid);

            this.taskRepo.Verify(t => t.Insert(testTask));
        }

        [Test]
        public void Test_Create_Task_Null()
        {
            // Testing creating a complete task, with all data

            // Moq
            this.taskRepo.Setup(t => t.Insert(It.IsAny<Task>())).Returns((Task task) => { return task; });

            TasksController controller = new TasksController(this.taskRepo.Object); // Controller

            var actionResult = controller.Post(null); // Action to test
            var result = actionResult as BadRequestObjectResult;

            // Assert
            Assert.AreEqual(result.StatusCode, 400);
        }

        [Test]
        public void Test_Create_Task_No_Description()
        {
            // Testing creating a task, with uncomplete data

            Task testTask = new Task() { description="" }; // Task

            this.taskRepo.Setup(t => t.Insert(It.IsAny<Task>())).Returns((Task task) => { return task; });

            TasksController controller = new TasksController(this.taskRepo.Object);
            bool isValid = ValidateTask.Validate(testTask, ref controller);

            var actionResult = controller.Post(testTask);
            var result = actionResult as BadRequestObjectResult;

            // Assert
            Assert.AreEqual(result.StatusCode, 400);
            Assert.IsFalse(isValid);

        }

    }
}