namespace UnitTests
{
    using Repository.Contracts;
    using API.Controllers;
    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;
    using API.Mappings;
    using Xunit;
    using AutoFixture;
    using AutoFixture.AutoNSubstitute;
    using AutoFixture.AutoMoq;
    using Repository.Models;

    public class TaskControllerTest
    {
        private readonly IFixture _fixture;
        private ITaskRepository _taskRepo;
        private IMapper mapper;

        public TaskControllerTest()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _taskRepo = _fixture.Freeze<ITaskRepository>();

            var mapperProfile = new Maps();
            var configurationMapper = new MapperConfiguration(cfg => cfg.AddProfile(mapperProfile));
            mapper = new Mapper(configurationMapper);

        }

        public class TheGetMethod: TaskControllerTest
        {
            [Fact]
            public void Test_Get_All_Task()
            {
                //Arrange
                TasksController controller = new TasksController(_taskRepo, mapper); // Controller

                //Act
                var actionResult = controller.Get(); // Action to test
                var result = actionResult as OkObjectResult;

                // Assert
                Assert.NotNull(result);
                Assert.NotNull(result.Value);
                Assert.Equal(200, result.StatusCode);
            }
        }


        public class TheGetByIdMethod : TaskControllerTest
        {
            [Fact]
            public void Test_Get_A_Task()
            {
                //Arrange
                var task = _fixture.Create<Task>();
                TasksController controller = new TasksController(_taskRepo, mapper); // Controller
                //_taskRepo.Setup();

                //Act

                // Assert
            }

            [Fact]
            public void Test_Get_A_Task_NotFound()
            {
                //Arrange

                //Act

                // Assert   
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
