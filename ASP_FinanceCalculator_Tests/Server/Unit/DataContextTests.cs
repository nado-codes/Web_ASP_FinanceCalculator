using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ASP_FinanceCalculator_Server.Models;
using ASP_FinanceCalculator_Server.Repos;
using Xunit;

namespace ASP_FinanceCalculator_Tests.Database
{
    public class DataContextTests
    {
        [Fact]
        public async void ExecuteReaderAsyncUnitTest()
        {
            var dataContext = new DataContext<Test>();
            dataContext.LoadConnectionString("Data Source=localhost;Initial Catalog=TestDB;Integrated Security=True;");
            dataContext.VerifyInitialize();

            var models = await dataContext.ExecuteReaderAsync("GetTestById", new NadoMapperParameter() {Name = "id", Value = 7});
        }
    }
}
