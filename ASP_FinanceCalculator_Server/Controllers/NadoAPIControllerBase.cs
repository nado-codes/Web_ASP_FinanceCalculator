using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Routing;
using ASP_FinanceCalculator_Server.Models;
using ASP_FinanceCalculator_Server.Repos;

namespace ASP_FinanceCalculator_Server.Controllers
{
    public class NadoAPIControllerBase<TModel,TRepo> : ApiController where TModel : ModelBase, new() where TRepo: Repository<TModel>, new()
    {
        protected TRepo _Repo = new TRepo();

        [HttpGet]
        public async Task<IEnumerable<TModel>> GetAllAsync()
        {
            // TODO: Security here for "GET" method
            return await _Repo.GetAllAsync();
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<TModel> GetByIdAsync(int id)
        {
            // TODO: Security here for "GET" method
            return await _Repo.GetSingleAsync(id);
        }

        [HttpPost]
        public async Task<TModel> AddAsync(TModel model)
        {
            // TODO: Security here for "GET" method
            return await _Repo.AddAsync(model);
        }

        [HttpPut]
        public async Task<long> UpdateAsync(TModel model)
        {
            // TODO: Security here for "GET" method
            return await _Repo.UpdateAsync(model);
        }

        [HttpDelete]
        public async Task<long> DeleteAsync(TModel model)
        {
            // TODO: Security here for "GET" method
            return await _Repo.DeleteAsync(model);
        }
    }
}
