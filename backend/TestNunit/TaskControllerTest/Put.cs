using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Repositories;
using api.Controllers;
using Moq;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using TestNunit.TaskControllerTest;

namespace TaskControllerTest
{
    public class Put
    {
        private Mock<ITasks<Task, int>> taskRepo;

        [SetUp]
        public void Setup()
        {
            this.taskRepo = new Mock<ITasks<Task, int>>();
        }

        [Test]
        public void Test_Update_Task()
        {
            // Testing updating a complete task, with all data
            Task testTask = new Task() { id = 1, description = "Descripcion", pending = false }; // Is going to be updated
            Task testUpdatedTask = new Task() { id = 1, description = "New Descripcion", pending = true }; // Is the pased task

            // Moq
            this.taskRepo.Setup(t => t.Any(It.IsAny<int>())).Returns((int id) => { return id==testTask.id; });
            this.taskRepo.Setup(t => t.Update(It.IsAny<Task>())).Returns((Task task) => {
                                                                        testTask = testUpdatedTask;
                                                                        return testUpdatedTask;
                                                                     });

            TasksController controller = new TasksController(this.taskRepo.Object); // Controller

            var actionResult = controller.Put(testUpdatedTask, testUpdatedTask.id); // Action to test
            var result = actionResult as CreatedAtRouteResult;
            bool isValid = ValidateTask.Validate(testTask, ref controller);


            // Assert
            Assert.AreEqual(result.StatusCode, 201);
            Assert.AreEqual(result.Value, testUpdatedTask);

            Assert.IsTrue(isValid);

            this.taskRepo.Verify(t => t.Any(testUpdatedTask.id));
            this.taskRepo.Verify(t => t.Update(testUpdatedTask));
        }

        [Test]
        public void Test_Update_Task_Null()
        {
            // Testing updating a complete task, with null task
            Task testTask = new Task() { id = 1, description = "Descripcion", pending = false }; // Is going to be updated

            // Moq
            this.taskRepo.Setup(t => t.Any(It.IsAny<int>())).Returns((int id) => { return id == testTask.id; });
            this.taskRepo.Setup(t => t.Update(It.IsAny<Task>())).Returns((Task task) => {
                testTask = null;
                return null;
            });

            TasksController controller = new TasksController(this.taskRepo.Object); // Controller

            var actionResult = controller.Put(null, 1); // Action to test
            var result = actionResult as BadRequestResult;

            // Assert
            Assert.AreEqual(result.StatusCode, 400);
        }

        [Test]
        public void Test_Update_Task_No_Description()
        {
            // Testing updating a complete task, without description
            Task testTask = new Task() { id = 1, description = "Descripcion", pending = false }; // Is going to be updated
            Task testUpdatedTask = new Task() { id = 1, description = "", pending = true }; // Is the pased task

            this.taskRepo.Setup(t => t.Any(It.IsAny<int>())).Returns((int id) => { return id == testTask.id; });
            this.taskRepo.Setup(t => t.Update(It.IsAny<Task>())).Returns((Task task) => {
                testTask = testUpdatedTask;
                return testUpdatedTask;
            });

            TasksController controller = new TasksController(this.taskRepo.Object); // Controller

            bool isValid = ValidateTask.Validate(testUpdatedTask, ref controller);

            var actionResult = controller.Put(testUpdatedTask, testUpdatedTask.id); // Action to test
            var result = actionResult as BadRequestResult;

            // Assert
            Assert.AreEqual(result.StatusCode, 400);
            Assert.IsFalse(isValid);
        }

        [Test]
        public void Test_Update_Task_Not_match_id()
        {
            // Testing updating a complete task, bad id
            Task testTask = new Task() { id = 1, description = "Descripcion", pending = false }; // Is going to be updated
            Task testUpdatedTask = new Task() { id = 1, description = "New Descripcion", pending = true }; // Is the pased task

            // Moq
            this.taskRepo.Setup(t => t.Any(It.IsAny<int>())).Returns((int id) => { return id == testTask.id; });
            this.taskRepo.Setup(t => t.Update(It.IsAny<Task>())).Returns((Task task) => {
                testTask = testUpdatedTask;
                return testUpdatedTask;
            });

            TasksController controller = new TasksController(this.taskRepo.Object); // Controller

            var actionResult = controller.Put(testUpdatedTask, 3); // Action to test
            var result = actionResult as BadRequestResult;

            // Assert
            Assert.AreEqual(result.StatusCode, 400);
        }

        [Test]
        public void Test_Update_Task_NotFound()
        {
            // Testing updating a complete task, with not found
            Task testTask = new Task() { id = 1, description = "Descripcion", pending = false }; // Is going to be updated
            Task testUpdatedTask = new Task() { id = 2, description = "New Descripcion", pending = true }; // Is the pased task

            // Moq
            this.taskRepo.Setup(t => t.Any(It.IsAny<int>())).Returns((int id) => { return id == testTask.id; });
            this.taskRepo.Setup(t => t.Update(It.IsAny<Task>())).Returns((Task task) => {
                testTask = testUpdatedTask;
                return testUpdatedTask;
            });

            TasksController controller = new TasksController(this.taskRepo.Object); // Controller

            var actionResult = controller.Put(testUpdatedTask, testUpdatedTask.id); // Action to test
            var result = actionResult as NotFoundResult;
            bool isValid = ValidateTask.Validate(testTask, ref controller);


            // Assert
            Assert.AreEqual(result.StatusCode, 404);

            Assert.IsTrue(isValid);

            this.taskRepo.Verify(t => t.Any(testUpdatedTask.id));
        }

    }
}