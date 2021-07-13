using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Repositories;
using api.Controllers;
using Moq;
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TaskControllerTest
{
    public class TaskControllerTest
    {
        [SetUp]
        public void Setup()
        {
        }

        // Get FUNCTION
        [Test]
        public void Test_Get_All_Task()
        {
            // Testing getting an array of tasks

            Task[] testTasks = new Task[] { // Array of task
                 new Task() { id = 1, description = "Descripcion", pending = true },
                 new Task() { id = 2, description = "Descripcion 2", pending = false }
            };

            //Moq
            Mock<ITasks<Task, int>> taskRepo = new Mock<ITasks<Task, int>>();
            taskRepo.Setup(t => t.GetAll()).Returns(testTasks);

            TasksController controller = new TasksController(taskRepo.Object); // Controller

            var actionResult = controller.Get(); // Action to test
            var result = actionResult as OkObjectResult;

            // Assert
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.Value, testTasks);

            taskRepo.Verify(t => t.GetAll());
        }

        // GetById FUNCTIONS
        [Test]
        public void Test_Get_A_Task()
        {
            // Testing getting an array of tasks

            Task testTask = new Task() { id = 1, description = "Descripcion", pending = true }; // Task

            //Moq
            Mock<ITasks<Task, int>> taskRepo = new Mock<ITasks<Task, int>>();
            taskRepo.Setup(t => t.GetById( It.IsAny<int>() )).Returns((int id)=> { 
                                                                            if(id == testTask.id) { return testTask; }
                                                                            else { return null; }
                                                                       });

            TasksController controller = new TasksController(taskRepo.Object); // Controller

            var actionResult = controller.GetById(1); // Action to test
            var result = actionResult as OkObjectResult;

            // Assert
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.Value, testTask);

            taskRepo.Verify(t => t.GetById(1));
        }

        [Test]
        public void Test_Get_A_Task_NotFound()
        {
            // Testing getting an array of tasks

            Task testTask = new Task() { id = 1, description = "Descripcion", pending = true }; // Task

            //Moq
            Mock<ITasks<Task, int>> taskRepo = new Mock<ITasks<Task, int>>();
            taskRepo.Setup(t => t.GetById(It.IsAny<int>())).Returns((int id) => {
                if (id == testTask.id) { return testTask; }
                else { return null; }
            });

            TasksController controller = new TasksController(taskRepo.Object); // Controller

            var actionResult = controller.GetById(2); // Action to test
            var result = actionResult as NotFoundResult;

            // Assert
            Assert.AreEqual(result.StatusCode, 404);

            taskRepo.Verify(t => t.GetById(2));
        }

        // Post FUNCTIONS
        [Test]
        public void Test_Create_Task()
        {
            // Testing creating an complete task, with all data

            Task testTask = new Task() { description = "Descripcion" }; // Task

            // Moq
            Mock<ITasks<Task, int>> taskRepo = new Mock<ITasks<Task, int>>();
            taskRepo.Setup(t => t.Insert(It.IsAny<Task>())).Returns((Task task) => { return task; });

            TasksController controller = new TasksController(taskRepo.Object); // Controller

            var actionResult = controller.Post(testTask); // Action to test
            var result = actionResult as CreatedAtRouteResult;
            bool isValid = validTask(testTask, ref controller);


            // Assert
            Assert.AreEqual(result.StatusCode, 201);
            Assert.AreEqual(result.Value, testTask);

            Assert.IsTrue(isValid);

            taskRepo.Verify(t => t.Insert(testTask));
        }

        [Test]
        public void Test_Create_Task_Null()
        {
            // Testing creating a complete task, with all data

            // Moq
            Mock<ITasks<Task, int>> taskRepo = new Mock<ITasks<Task, int>>();
            taskRepo.Setup(t => t.Insert(It.IsAny<Task>())).Returns((Task task) => { return task; });

            TasksController controller = new TasksController(taskRepo.Object); // Controller

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

            // This code It's here to show the function but in real testing it's not valid
            // It's an example for the technical interview
            Mock<ITasks<Task, int>> taskRepo = new Mock<ITasks<Task, int>>();
            taskRepo.Setup(t => t.Insert(It.IsAny<Task>())).Returns((Task task) => { return task; });

            TasksController controller = new TasksController(taskRepo.Object);
            bool isValid = validTask(testTask, ref controller);

            var actionResult = controller.Post(testTask);
            var result = actionResult as BadRequestObjectResult;

            // Assert
            Assert.AreEqual(result.StatusCode, 400);
            Assert.IsFalse(isValid);

        }

        // Put FUNCTIONS
        [Test]
        public void Test_Update_Task()
        {
            // Testing updating a complete task, with all data
            Task testTask = new Task() { id = 1, description = "Descripcion", pending = false }; // Is going to be updated
            Task testUpdatedTask = new Task() { id = 1, description = "New Descripcion", pending = true }; // Is the pased task

            // Moq
            Mock<ITasks<Task, int>> taskRepo = new Mock<ITasks<Task, int>>();
            taskRepo.Setup(t => t.Any(It.IsAny<int>())).Returns((int id) => { return id==testTask.id; });
            taskRepo.Setup(t => t.Update(It.IsAny<Task>())).Returns((Task task) => {
                                                                        testTask = testUpdatedTask;
                                                                        return testUpdatedTask;
                                                                     });

            TasksController controller = new TasksController(taskRepo.Object); // Controller

            var actionResult = controller.Put(testUpdatedTask, testUpdatedTask.id); // Action to test
            var result = actionResult as CreatedAtRouteResult;
            bool isValid = validTask(testTask, ref controller);


            // Assert
            Assert.AreEqual(result.StatusCode, 201);
            Assert.AreEqual(result.Value, testUpdatedTask);

            Assert.IsTrue(isValid);

            taskRepo.Verify(t => t.Any(testUpdatedTask.id));
            taskRepo.Verify(t => t.Update(testUpdatedTask));
        }

        [Test]
        public void Test_Update_Task_Null()
        {
            // Testing updating a complete task, with null task
            Task testTask = new Task() { id = 1, description = "Descripcion", pending = false }; // Is going to be updated

            // Moq
            Mock<ITasks<Task, int>> taskRepo = new Mock<ITasks<Task, int>>();
            taskRepo.Setup(t => t.Any(It.IsAny<int>())).Returns((int id) => { return id == testTask.id; });
            taskRepo.Setup(t => t.Update(It.IsAny<Task>())).Returns((Task task) => {
                testTask = null;
                return null;
            });

            TasksController controller = new TasksController(taskRepo.Object); // Controller

            var actionResult = controller.Put(null, 1); // Action to test
            var result = actionResult as BadRequestResult;

            // Assert
            Assert.AreEqual(result.StatusCode, 400);
        }

        [Test]
        public void Test_Update_Task_BadRequest()
        {
            // Testing updating a complete task, bad id
            Task testTask = new Task() { id = 1, description = "Descripcion", pending = false }; // Is going to be updated
            Task testUpdatedTask = new Task() { id = 1, description = "New Descripcion", pending = true }; // Is the pased task

            // Moq
            Mock<ITasks<Task, int>> taskRepo = new Mock<ITasks<Task, int>>();
            taskRepo.Setup(t => t.Any(It.IsAny<int>())).Returns((int id) => { return id == testTask.id; });
            taskRepo.Setup(t => t.Update(It.IsAny<Task>())).Returns((Task task) => {
                testTask = testUpdatedTask;
                return testUpdatedTask;
            });

            TasksController controller = new TasksController(taskRepo.Object); // Controller

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
            Mock<ITasks<Task, int>> taskRepo = new Mock<ITasks<Task, int>>();
            taskRepo.Setup(t => t.Any(It.IsAny<int>())).Returns((int id) => { return id == testTask.id; });
            taskRepo.Setup(t => t.Update(It.IsAny<Task>())).Returns((Task task) => {
                testTask = testUpdatedTask;
                return testUpdatedTask;
            });

            TasksController controller = new TasksController(taskRepo.Object); // Controller

            var actionResult = controller.Put(testUpdatedTask, testUpdatedTask.id); // Action to test
            var result = actionResult as NotFoundResult;
            bool isValid = validTask(testTask, ref controller);


            // Assert
            Assert.AreEqual(result.StatusCode, 404);

            Assert.IsTrue(isValid);

            taskRepo.Verify(t => t.Any(testUpdatedTask.id));
        }

        [Test]
        public void Test_Update_Task_No_Description()
        {
            // Testing updating a complete task, without description
            Task testTask = new Task() { id = 1, description = "Descripcion", pending = false }; // Is going to be updated
            Task testUpdatedTask = new Task() { id = 1, description = "", pending = true }; // Is the pased task

            Mock<ITasks<Task, int>> taskRepo = new Mock<ITasks<Task, int>>();
            taskRepo.Setup(t => t.Any(It.IsAny<int>())).Returns((int id) => { return id == testTask.id; });
            taskRepo.Setup(t => t.Update(It.IsAny<Task>())).Returns((Task task) => {
                testTask = testUpdatedTask;
                return testUpdatedTask;
            });

            TasksController controller = new TasksController(taskRepo.Object); // Controller

            bool isValid = validTask(testUpdatedTask, ref controller);

            var actionResult = controller.Put(testUpdatedTask, testUpdatedTask.id); // Action to test
            var result = actionResult as BadRequestResult;

            // Assert
            Assert.AreEqual(result.StatusCode, 400);
            Assert.IsFalse(isValid);
        }

        private bool validTask(Task task, ref TasksController controller)
        {
            // This function is because the unit test always pass ModelState.IsValid

            if(task == null)
            {
                controller.ModelState.AddModelError("Error", "Error task not valid");
                return false;
            }

            // Function to test Model.IsValid
            var context = new ValidationContext(task, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(
                                task, context, results,
                                validateAllProperties: true
                            );
            if (!isValid)
            {
                controller.ModelState.AddModelError("Error", "Error task not valid");
            }

            return isValid;
        }

    }
}