using Repository.Contracts;
using Moq;

namespace UnitTests
{
    class TaskControllerTest
    {
        private Mock<ITaskRepository> taskRepo;

        public TaskControllerTest()
        {
            taskRepo = new Mock<ITaskRepository>();
        }

    }
}
