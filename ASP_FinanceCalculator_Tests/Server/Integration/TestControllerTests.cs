using ASP_FinanceCalculator_Server.Controllers;
using System;
using System.Linq;
using ASP_FinanceCalculator_Server.Models;
using Xunit;

namespace ASP_FinanceCalculator_Tests.Server
{
    public class TestControllerTests
    {
        [Fact]
        public async void IntegrationTest()
        {
            var testController = new TestController();

            //Add
            var addTestName = "MyTest";


            var addTest = await testController.AddAsync(new Test()
            {
                Name = addTestName
            });

            Assert.True(addTest.Name == addTestName);

            //Update
            var updateTestName = "UpdatedTest";

            addTest.Name = updateTestName;

            var updateTestRowsUpdated = await testController.UpdateAsync(addTest);
            Assert.True(updateTestRowsUpdated == 1);

            //GetById
            var updatedTest = await testController.GetByIdAsync(addTest.Id);
            Assert.True(updatedTest.Name == updateTestName);

            //GetAll
            var tests = await testController.GetAllAsync();
            Assert.True(tests.Count() > 0);

            //Delete
            var deleteTestRowsUpdated = await testController.DeleteAsync(updatedTest);
            var deletedTest = await testController.GetByIdAsync(updatedTest.Id);

            Assert.True(deletedTest == null);
        }
    }
}
