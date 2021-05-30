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

            var updateTestRowsUpdated = await testController.UpdateAsync(new Test());
            Assert.True(updateTestRowsUpdated == 1);

            //GetById
            var updateTest = await testController.GetByIdAsync(addTest.Id);
            Assert.True(updateTest.Name == updateTestName);

            //GetAll
            var tests = await testController.GetAllAsync();
            Assert.True(tests.Count() > 0);

            //Delete
            var deleteTestRowsUpdated = await testController.DeleteAsync(updateTest);
            var deleteTest = await testController.GetByIdAsync(updateTest.Id);

            Assert.True(deleteTest == null);
        }
    }
}
