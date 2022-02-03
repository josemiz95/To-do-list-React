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
                TasksController controller = new TasksController(_taskRepo.Object, _mapper); // Controller

                //Act
                var mappedTasks = _mapper.Map< List<Task>, List<TaskVM>>(tasks);
                var actionResult = controller.Get(); // Action to test

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

                //Assert.Equal(mappedTasks, resultTasks);
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
                TasksController controller = new TasksController(_taskRepo.Object, _mapper); // Controller

                //Act
                var mappedTask = _mapper.Map<Task, TaskVM>(task);
                var actionResult = controller.GetById(task.id); // Action to test

                var result = actionResult as OkObjectResult;
                var resultTask = result.Value as TaskVM;

                // Assert
                Assert.NotNull(result);
                Assert.NotNull(resultTask);

                Assert.Equal(200, result.StatusCode);
                Assert.Equal(mappedTask.id, resultTask.id);
                //Assert.Equal(mappedTask, resultTask);
            }

            [Fact]
            public void Test_Get_A_Task_NotFound()
            {
                //Arrange
                _taskRepo.Setup(x => x.Find(It.IsAny<int>())).Returns(null as Task);
                TasksController controller = new TasksController(_taskRepo.Object, _mapper); // Controller

                //Act
                var actionResult = controller.GetById(1); // Action to test
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

                //Act

                // Assert    
            }

            [Fact]
            public void Test_Create_Task_Null()
            {
                //Arrange

                //Act

                // Assert    
            }

            [Fact]
            public void Test_Create_Task_No_Description()
            {
                

            }
        }

        public class ThePutMethod : TaskControllerTest
        {
            [Fact]
            public void Test_Update_Task()
            {
                //Arrange

                //Act

                // Assert    
            }

            [Fact]
            public void Test_Update_Task_Null()
            {
                //Arrange

                //Act

                // Assert    
            }

            [Fact]
            public void Test_Update_Task_No_Description()
            {
                //Arrange

                //Act

                // Assert    
            }

            [Fact]
            public void Test_Update_Task_Not_match_id()
            {
                //Arrange

                //Act

                // Assert    
            }

            [Fact]
            public void Test_Update_Task_NotFound()
            {
                //Arrange

                //Act

                // Assert    
            }
        }

        public class TheDeleteMethod : TaskControllerTest
        {
            [Fact]
            public void Test_Delete_A_Task()
            {
                //Arrange

                //Act

                // Assert 
            }

            [Fact]
            public void Test_Delete_A_Task_NotFound()
            {
                //Arrange

                //Act

                // Assert    
            }
        }
    }
}
