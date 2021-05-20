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
using ASP_FinanceCalculator_Server.Models;
using ASP_FinanceCalculator_Server.Repos;

namespace ASP_FinanceCalculator_Server.Controllers
{
    [RoutePrefix("api/tests")]
    public class TestController : NadoAPIControllerBase<Test,TestsRepositoryBase>
    {
        
    }
}
