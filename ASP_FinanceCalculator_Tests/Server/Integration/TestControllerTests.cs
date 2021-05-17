using ASP_FinanceCalculator_Server.Controllers;
using System;
using System.Linq;
using Xunit;

namespace ASP_FinanceCalculator_Tests
{
    public class TestControllerTests
    {
        [Fact]
        public async void IntegrationTest()
        {
            var testController = new TestController();

            //Add

            //Update

            //GetAll
            var tests = await testController.GetAllAsync();

            Assert.True(tests.Count() > 0);

            //GetById

            //Delete            
        }
    }
}
