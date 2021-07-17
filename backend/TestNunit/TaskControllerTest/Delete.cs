using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Repositories;
using api.Controllers;
using Moq;

namespace TaskControllerTest
{
    public class Delete
    {
        private Mock<ITasks<Task, int>> taskRepo;

        [SetUp]
        public void Setup()
        {
            this.taskRepo = new Mock<ITasks<Task, int>>();
        }

        [Test]
        public void Test_Delete_A_Task()
        {
            // Testing deleting a task

            Task testTask = new Task() { id = 1, description = "Descripcion", pending = true }; // Task

            //Moq
            this.taskRepo.Setup(t => t.GetById(It.IsAny<int>())).Returns((int id) => {
                                                                                if (id == testTask.id) { return testTask; }
                                                                                else { return null; }
                                                                            });

            this.taskRepo.Setup(t => t.Any(It.IsAny<int>())).Returns((int id) => {
                                                                                if (id == testTask.id) { return true; }
                                                                                else { return false; }
                                                                            });

            this.taskRepo.Setup(t => t.Delete(It.IsAny<Task>())).Returns((Task task) =>{
                                                                                if (task.id == testTask.id) { return true; }
                                                                                else { return false; }
                                                                              });

            TasksController controller = new TasksController(this.taskRepo.Object); // Controller

            var actionResult = controller.Delete(1); // Action to test
            var result = actionResult as OkResult;

            // Assert
            Assert.AreEqual(result.StatusCode, 200);

            this.taskRepo.Verify(t => t.GetById(1));
            this.taskRepo.Verify(t => t.Any(1));
            this.taskRepo.Verify(t => t.Delete(testTask));
        }

        [Test]
        public void Test_Delete_A_Task_NotFound()
        {
            // Testing deleting a task that not exists

            Task testTask = new Task() { id = 1, description = "Descripcion", pending = true }; // Task

            //Moq

            this.taskRepo.Setup(t => t.Any(It.IsAny<int>())).Returns((int id) => {
                                                                                if (id == testTask.id) { return true; }
                                                                                else { return false; }
                                                                            });


            TasksController controller = new TasksController(this.taskRepo.Object); // Controller

            var actionResult = controller.Delete(2); // Action to test
            var result = actionResult as NotFoundResult;

            // Assert
            Assert.AreEqual(result.StatusCode, 404);

            this.taskRepo.Verify(t => t.Any(2));
        }
    }
}