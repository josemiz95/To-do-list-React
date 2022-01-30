using Repository.Contracts;
using Moq;
using NUnit.Framework;
using Repository.Models;
using API.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using AutoMapper;
using API.Mappings;
using API.ViewModels;

namespace UnitTests
{
    public class TaskControllerTest
    {
        private Mock<ITaskRepository> taskRepo;
        private IMapper mapper;

        public TaskControllerTest()
        {
            taskRepo = new Mock<ITaskRepository>();

            var mapperProfile = new Maps();
            var configurationMapper = new MapperConfiguration(cfg => cfg.AddProfile(mapperProfile));
            mapper = new Mapper(configurationMapper);

        }

        public class TheGetMethod: TaskControllerTest
        {
            [Test]
            public void Test_Get_All_Task()
            {
                // Testing getting an array of tasks

                Task[] testTasks = new Task[] { // Array of task
                 new Task() { id = 1, description = "Descripcion", pending = true },
                 new Task() { id = 2, description = "Descripcion 2", pending = false }
            };

                //Moq
                this.taskRepo.Setup(t => t.All()).Returns(testTasks);

                TasksController controller = new TasksController(this.taskRepo.Object, mapper); // Controller

                var actionResult = controller.Get(); // Action to test
                var result = actionResult as OkObjectResult;

                // Assert
                Assert.AreEqual(result.StatusCode, 200);

                this.taskRepo.Verify(t => t.All());
            }
        }


        public class TheGetByIdMethod: TaskControllerTest
        {
            [Test]
            public void Test_Get_A_Task()
            {
                // Testing getting an array of tasks

                Task testTask = new Task() { id = 1, description = "Descripcion", pending = true }; // Task

                //Moq
                this.taskRepo.Setup(t => t.Find(It.IsAny<int>())).Returns((int id) => {
                    if (id == testTask.id) { return testTask; }
                    else { return null; }
                });

                TasksController controller = new TasksController(this.taskRepo.Object, mapper); // Controller

                var actionResult = controller.GetById(1); // Action to test
                var result = actionResult as OkObjectResult;

                // Assert
                Assert.AreEqual(result.StatusCode, 200);

                this.taskRepo.Verify(t => t.Find(1));
            }

            [Test]
            public void Test_Get_A_Task_NotFound()
            {
                // Testing getting an array of tasks

                Task testTask = new Task() { id = 1, description = "Descripcion", pending = true }; // Task

                //Moq
                this.taskRepo.Setup(t => t.Find(It.IsAny<int>())).Returns((int id) => {
                    if (id == testTask.id) { return testTask; }
                    else { return null; }
                });

                TasksController controller = new TasksController(this.taskRepo.Object, mapper); // Controller

                var actionResult = controller.GetById(2); // Action to test
                var result = actionResult as NotFoundResult;

                // Assert
                Assert.AreEqual(result.StatusCode, 404);

                this.taskRepo.Verify(t => t.Find(2));
            }
        }
    
        public class ThePostMethod : TaskControllerTest
        {
            [Test]
            public void Test_Create_Task()
            {
                // Testing creating an complete task, with all data

                TaskVM testTask = new TaskVM() { description = "Descripcion" }; // Task

                // Moq
                this.taskRepo.Setup(t => t.Create(It.IsAny<Task>())).Returns((Task task) => { return task; });

                TasksController controller = new TasksController(this.taskRepo.Object, mapper); // Controller

                var actionResult = controller.Post(testTask); // Action to test
                var result = actionResult as CreatedAtRouteResult;
                bool isValid = Validate(testTask, ref controller);

                // Assert
                Assert.AreEqual(result.StatusCode, 201);

                Assert.IsTrue(isValid);
            }

            [Test]
            public void Test_Create_Task_Null()
            {
                // Testing creating a complete task, with all data

                // Moq
                this.taskRepo.Setup(t => t.Create(It.IsAny<Task>())).Returns((Task task) => { return task; });

                TasksController controller = new TasksController(this.taskRepo.Object, mapper); // Controller

                var actionResult = controller.Post(null); // Action to test
                var result = actionResult as BadRequestObjectResult;

                // Assert
                Assert.AreEqual(result.StatusCode, 400);
            }

            [Test]
            public void Test_Create_Task_No_Description()
            {
                // Testing creating a task, with uncomplete data

                TaskVM testTask = new TaskVM() { description = "" }; // Task

                this.taskRepo.Setup(t => t.Create(It.IsAny<Task>())).Returns((Task task) => { return task; });

                TasksController controller = new TasksController(this.taskRepo.Object, mapper);
                bool isValid = Validate(testTask, ref controller);

                var actionResult = controller.Post(testTask);
                var result = actionResult as BadRequestObjectResult;

                // Assert
                Assert.AreEqual(result.StatusCode, 400);
                Assert.IsFalse(isValid);

            }
        }
    
        public class ThePutMethod : TaskControllerTest
        {
            [Test]
            public void Test_Update_Task()
            {
                // Testing updating a complete task, with all data
                Task testTask = new Task() { id = 1, description = "Descripcion", pending = false }; // Is going to be updated
                Task testUpdatedTask = new Task() { id = 1, description = "New Descripcion", pending = true }; // Is the pased task

                // Moq
                this.taskRepo.Setup(t => t.Exists(It.IsAny<int>())).Returns((int id) => { return id == testTask.id; });
                this.taskRepo.Setup(t => t.Update(It.IsAny<Task>())).Returns((Task task) => {
                    testTask = testUpdatedTask;
                    return true;
                });

                TasksController controller = new TasksController(this.taskRepo.Object, mapper); // Controller

                var actionResult = controller.Put(testUpdatedTask, testUpdatedTask.id); // Action to test
                var result = actionResult as CreatedAtRouteResult;

                var mappedTask = mapper.Map<Task, TaskVM>(testTask);

                bool isValid = Validate(mappedTask, ref controller);


                // Assert
                Assert.AreEqual(result.StatusCode, 201);
                Assert.AreEqual(result.Value, testUpdatedTask);

                Assert.IsTrue(isValid);

                this.taskRepo.Verify(t => t.Exists(testUpdatedTask.id));
                this.taskRepo.Verify(t => t.Update(testUpdatedTask));
            }

            [Test]
            public void Test_Update_Task_Null()
            {
                // Testing updating a complete task, with null task
                Task testTask = new Task() { id = 1, description = "Descripcion", pending = false }; // Is going to be updated

                // Moq
                this.taskRepo.Setup(t => t.Exists(It.IsAny<int>())).Returns((int id) => { return id == testTask.id; });
                this.taskRepo.Setup(t => t.Update(It.IsAny<Task>())).Returns((Task task) => {
                    testTask = null;
                    return false;
                });

                TasksController controller = new TasksController(this.taskRepo.Object, mapper); // Controller

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

                this.taskRepo.Setup(t => t.Exists(It.IsAny<int>())).Returns((int id) => { return id == testTask.id; });
                this.taskRepo.Setup(t => t.Update(It.IsAny<Task>())).Returns((Task task) => {
                    testTask = testUpdatedTask;
                    return true;
                });

                TasksController controller = new TasksController(this.taskRepo.Object, mapper); // Controller

                var mappedTask = mapper.Map<Task, TaskVM>(testUpdatedTask);
                bool isValid = Validate(mappedTask, ref controller);

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
                this.taskRepo.Setup(t => t.Exists(It.IsAny<int>())).Returns((int id) => { return id == testTask.id; });
                this.taskRepo.Setup(t => t.Update(It.IsAny<Task>())).Returns((Task task) => {
                    testTask = testUpdatedTask;
                    return true;
                });

                TasksController controller = new TasksController(this.taskRepo.Object, mapper); // Controller

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
                this.taskRepo.Setup(t => t.Exists(It.IsAny<int>())).Returns((int id) => { return id == testTask.id; });
                this.taskRepo.Setup(t => t.Update(It.IsAny<Task>())).Returns((Task task) => {
                    testTask = testUpdatedTask;
                    return true;
                });

                TasksController controller = new TasksController(this.taskRepo.Object, mapper); // Controller

                var actionResult = controller.Put(testUpdatedTask, testUpdatedTask.id); // Action to test
                var result = actionResult as NotFoundResult;

                var mappedTask = mapper.Map<Task, TaskVM>(testTask);
                bool isValid = Validate(mappedTask, ref controller);


                // Assert
                Assert.AreEqual(result.StatusCode, 404);

                Assert.IsTrue(isValid);

                this.taskRepo.Verify(t => t.Exists(testUpdatedTask.id));
            }
        }

        public class TheDeleteMethod : TaskControllerTest
        {
            [Test]
            public void Test_Delete_A_Task()
            {
                // Testing deleting a task

                Task testTask = new Task() { id = 1, description = "Descripcion", pending = true }; // Task

                //Moq
                this.taskRepo.Setup(t => t.Find(It.IsAny<int>())).Returns((int id) => {
                    if (id == testTask.id) { return testTask; }
                    else { return null; }
                });

                this.taskRepo.Setup(t => t.Exists(It.IsAny<int>())).Returns((int id) => {
                    if (id == testTask.id) { return true; }
                    else { return false; }
                });

                this.taskRepo.Setup(t => t.Delete(It.IsAny<Task>())).Returns((Task task) => {
                    if (task.id == testTask.id) { return true; }
                    else { return false; }
                });

                TasksController controller = new TasksController(this.taskRepo.Object, mapper); // Controller

                var actionResult = controller.Delete(1); // Action to test
                var result = actionResult as OkResult;

                // Assert
                Assert.AreEqual(result.StatusCode, 200);

                this.taskRepo.Verify(t => t.Find(1));
                this.taskRepo.Verify(t => t.Exists(1));
                this.taskRepo.Verify(t => t.Delete(testTask));
            }

            [Test]
            public void Test_Delete_A_Task_NotFound()
            {
                // Testing deleting a task that not exists

                Task testTask = new Task() { id = 1, description = "Descripcion", pending = true }; // Task

                //Moq

                this.taskRepo.Setup(t => t.Exists(It.IsAny<int>())).Returns((int id) => {
                    if (id == testTask.id) { return true; }
                    else { return false; }
                });


                TasksController controller = new TasksController(this.taskRepo.Object, mapper); // Controller

                var actionResult = controller.Delete(2); // Action to test
                var result = actionResult as NotFoundResult;

                // Assert
                Assert.AreEqual(result.StatusCode, 404);

                this.taskRepo.Verify(t => t.Exists(2));
            }
        }

        private bool Validate(TaskVM task, ref TasksController controller)
        {
            // This function is because the unit test always pass ModelState.IsValid

            if (task == null)
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
