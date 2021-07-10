using NUnit.Framework;
using api.Models;
using api.Repositories;
using Moq;

namespace TestNunit
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Task task = new Task()
            {
                id = 1,
                description = "Descripcion de la tarea",
                pending = true
            };

            

            Assert.AreEqual(true, task.pending);
        }
    }
}