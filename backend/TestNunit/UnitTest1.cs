using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Repositories;
using api.Controllers;
using Moq;
using System;
using System.Net;

namespace TestNunit
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
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
            Mock<ITasks<Task, int>> taskRepo = new Mock<ITasks<Task, int>>();
            taskRepo.Setup(t => t.GetAll()).Returns(testTasks);

            TasksController controller = new TasksController(taskRepo.Object); // Controller

            var actionResult = controller.Get(); // Action to test
            var result = actionResult as OkObjectResult;

            // Testing
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.Value, testTasks);

            taskRepo.Verify(t => t.GetAll());
        }

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

            // Testing
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

            // Testing
            Assert.AreEqual(result.StatusCode, 404);

            taskRepo.Verify(t => t.GetById(2));
        }

        [Test]
        public void Test_Create_Task()
        {
            // Testing creating an complete task, with all data

            Task testTask = new Task() { id = 1, description = "Descripcion", pending = true }; // Task

            // Moq
            Mock<ITasks<Task, int>> taskRepo = new Mock<ITasks<Task, int>>();
            taskRepo.Setup(t => t.Insert(It.IsAny<Task>())).Returns((Task task) => { return task; });

            TasksController controller = new TasksController(taskRepo.Object); // Controller

            var actionResult = controller.Post(testTask); // Action to test
            var result = actionResult as CreatedAtRouteResult;

            // Testing
            Assert.AreEqual(result.StatusCode, 201);
            Assert.AreEqual(result.Value, testTask);

            taskRepo.Verify(t => t.Insert(testTask));
        }
    }
}