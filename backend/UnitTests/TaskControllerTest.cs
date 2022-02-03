namespace UnitTests
{
    
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;
    using API.Mappings;
    using Xunit;
    using AutoFixture;
    using Moq;
    using Repository.Contracts;
    using Repository.Models;
    using API.ViewModels;
    using API.Controllers;

    public class TaskControllerTest
    {
        private readonly IFixture _fixture;
        private Mock<ITaskRepository> _taskRepo;
        private IMapper _mapper;

        public TaskControllerTest()
        {
            _fixture = new Fixture();
            _taskRepo = new Mock<ITaskRepository>();

            var mapperProfile = new Maps();
            var configurationMapper = new MapperConfiguration(cfg => cfg.AddProfile(mapperProfile));
            _mapper = new Mapper(configurationMapper);

        }

        public class TheGetMethod: TaskControllerTest
        {
            [Fact]
            public void Test_Get_All_Task()
            {
                //Arrange
                var tasks = _fixture.Create<List<Task>>();
                _taskRepo.Setup(x => x.All()).Returns(tasks);
                TasksController controller = new TasksController(_taskRepo.Object, _mapper);

                //Act
                var mappedTasks = _mapper.Map< List<Task>, List<TaskVM>>(tasks);
                var actionResult = controller.Get();

                var result = actionResult as OkObjectResult;
                var resultTasks = result.Value as List<TaskVM>;

                // Assert
                Assert.NotNull(result);
                Assert.NotNull(result.Value);
                Assert.Equal(200, result.StatusCode);

                Assert.All(resultTasks, resultTask => {
                    Assert.Contains(mappedTasks, task => task.id == resultTask.id);
                    Assert.Contains(mappedTasks, task => task.description == resultTask.description);
                });

                //Assert.Equal(mappedTasks, resultTasks); ????
            }
        }


        public class TheGetByIdMethod : TaskControllerTest
        {
            [Fact]
            public void Test_Get_A_Task()
            {
                //Arrange
                var task = _fixture.Create<Task>();
                _taskRepo.Setup(x => x.Find(task.id)).Returns(task);
                TasksController controller = new TasksController(_taskRepo.Object, _mapper);

                //Act
                var mappedTask = _mapper.Map<Task, TaskVM>(task);
                var actionResult = controller.GetById(task.id);

                var result = actionResult as OkObjectResult;
                var resultTask = result.Value as TaskVM;

                // Assert
                Assert.NotNull(result);
                Assert.NotNull(resultTask);

                Assert.Equal(200, result.StatusCode);
                Assert.Equal(mappedTask.id, resultTask.id);
                //Assert.Equal(mappedTask, resultTask); ???
            }

            [Fact]
            public void Test_Get_A_Task_NotFound()
            {
                //Arrange
                _taskRepo.Setup(x => x.Find(It.IsAny<int>())).Returns(null as Task);
                TasksController controller = new TasksController(_taskRepo.Object, _mapper);

                //Act
                var actionResult = controller.GetById(1);
                var result = actionResult as NotFoundResult;

                // Assert
                Assert.NotNull(result);
                Assert.Equal(404, result.StatusCode);
            }
        }

        public class ThePostMethod : TaskControllerTest
        {
            [Fact]
            public void Test_Create_Task()
            {
                //Arrange
                var task = _fixture.Create<TaskVM>();
                _taskRepo.Setup(t => t.Create(It.IsAny<Task>())).Returns((Task taskResult) => { return taskResult; });
                TasksController controller = new TasksController(_taskRepo.Object, _mapper);

                //Act
                var actionResult = controller.Post(task);
                var result = actionResult as CreatedAtRouteResult;
                var resultTask = result.Value as TaskVM;

                // Assert
                Assert.Equal(201, result.StatusCode);
                Assert.Equal(task.id, resultTask.id);
            }

            [Fact]
            public void Test_Create_Task_Null()
            {
                //Arrange
                TasksController controller = new TasksController(_taskRepo.Object, _mapper);

                //Act
                var actionResult = controller.Post(null);
                var result = actionResult as BadRequestResult;

                // Assert
                Assert.Equal(400, result.StatusCode);
            }

            [Fact]
            public void Test_Create_Task_No_Description()
            {
                //Arrange
                var task = _fixture.Create<TaskVM>();
                task.description = "";
                TasksController controller = new TasksController(_taskRepo.Object, _mapper);

                //Act
                var actionResult = controller.Post(task);
                var result = actionResult as BadRequestResult;

                // Assert
                Assert.Equal(400, result.StatusCode);
            }
        }

        public class ThePutMethod : TaskControllerTest
        {
            [Fact]
            public void Test_Update_Task()
            {
                //Arrange
                var task = _fixture.Create<TaskVM>();
                var taskUpdated = new TaskVM { id = task.id, description = _fixture.Create<string>() }; // clonar objetos?
                _taskRepo.Setup(t => t.Exists(task.id)).Returns(true);
                _taskRepo.Setup(t => t.Update(It.IsAny<Task>())).Returns(true);
                TasksController controller = new TasksController(_taskRepo.Object, _mapper);

                //Act
                var actionResult = controller.Put(taskUpdated, task.id);
                var result = actionResult as CreatedAtRouteResult;
                var resultTask = result.Value as TaskVM;

                // Assert
                Assert.Equal(201, result.StatusCode);
                Assert.Equal(task.id, resultTask.id);
                Assert.NotEqual(task.description, resultTask.description);
            }

            [Fact]
            public void Test_Update_Task_Null()
            {
                //Arrange
                TasksController controller = new TasksController(_taskRepo.Object, _mapper);

                //Act

                var actionResult = controller.Put(null, 1);
                var result = actionResult as BadRequestResult;

                // Assert
                Assert.Equal(400, result.StatusCode);
            }

            [Fact]
            public void Test_Update_Task_No_Description()
            {
                //Arrange
                var task = _fixture.Create<TaskVM>();
                task.description = "";
                TasksController controller = new TasksController(_taskRepo.Object, _mapper);

                //Act
                var actionResult = controller.Put(task, task.id);
                var result = actionResult as BadRequestResult;

                // Assert
                Assert.Equal(400, result.StatusCode);
            }

            [Fact]
            public void Test_Update_Task_Not_match_id()
            {
                //Arrange
                var task = _fixture.Create<TaskVM>();
                var randomId = _fixture.Create<int>();
                TasksController controller = new TasksController(_taskRepo.Object, _mapper);

                //Act
                var actionResult = controller.Put(task, randomId);
                var result = actionResult as BadRequestResult;

                // Assert
                Assert.Equal(400, result.StatusCode);
            }

            [Fact]
            public void Test_Update_Task_NotFound()
            {
                //Arrange
                var task = _fixture.Create<TaskVM>();
                var taskUpdated = _fixture.Create<TaskVM>();
                _taskRepo.Setup(t => t.Exists(task.id)).Returns(true);
                TasksController controller = new TasksController(_taskRepo.Object, _mapper);

                //Act
                var actionResult = controller.Put(taskUpdated, taskUpdated.id);
                var result = actionResult as NotFoundResult;

                // Assert
                Assert.Equal(404, result.StatusCode);
            }
        }

        public class TheDeleteMethod : TaskControllerTest
        {
            [Fact]
            public void Test_Delete_A_Task()
            {
                //Arrange
                var task = _fixture.Create<Task>();
                _taskRepo.Setup(t => t.Exists(task.id)).Returns(true);
                _taskRepo.Setup(t => t.Find(task.id)).Returns(task);
                _taskRepo.Setup(t => t.Delete(task)).Returns(true);
                TasksController controller = new TasksController(_taskRepo.Object, _mapper);

                //Act
                var actionResult = controller.Delete(task.id);
                var result = actionResult as OkResult;

                // Assert 
                Assert.Equal(200, result.StatusCode);
            }

            [Fact]
            public void Test_Delete_A_Task_NotFound()
            {
                //Arrange
                var task = _fixture.Create<Task>();
                var randomId = _fixture.Create<int>();
                _taskRepo.Setup(t => t.Exists(task.id)).Returns(true);
                _taskRepo.Setup(t => t.Find(task.id)).Returns(task);
                _taskRepo.Setup(t => t.Delete(task)).Returns(true);
                TasksController controller = new TasksController(_taskRepo.Object, _mapper);

                //Act
                var actionResult = controller.Delete(randomId);
                var result = actionResult as NotFoundResult;

                // Assert 
                Assert.Equal(404, result.StatusCode);
            }
        }
    }
}
