using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ASP_FinanceCalculator_Server.Controllers
{
    [RoutePrefix("api/test")]
    public class TestController : ApiController
    {
        [HttpGet]
        [Route("")]
        public async Task<bool> Test()
        {
            // DbContext context = new DbContext("localhost");

            // return await context.
            return false;
        }
    }
}
