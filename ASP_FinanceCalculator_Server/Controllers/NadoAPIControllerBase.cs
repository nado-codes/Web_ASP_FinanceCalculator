using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ASP_FinanceCalculator_Server.Models;
using ASP_FinanceCalculator_Server.Repos;

namespace ASP_FinanceCalculator_Server.Controllers
{
    public class NadoAPIControllerBase<TModel,TRepo> : ApiController where TModel : ModelBase, new() where TRepo: Repository<TModel>, new()
    {
        protected TRepo _Repo = new TRepo();

        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<TModel>> GetAllAsync()
        {
            // TODO: Security here for "GEt" method
            return await _Repo.GetAllAsync();
        }
    }
}
