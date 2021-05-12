using ASP_FinanceCalculator_Server.Controllers;
using System;
using System.Linq;
using Xunit;

namespace ASP_FinanceCalculator_Tests
{
    public class TestControllerTests
    {
        [Fact]
        public async void Integration()
        {
            var testController = new TestController();

            var tests = await testController.GetAllAsync();

            Assert.True(tests.Count() > 0);
        }
    }
}
