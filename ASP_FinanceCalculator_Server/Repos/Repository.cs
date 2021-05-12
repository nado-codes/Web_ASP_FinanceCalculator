using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Web;
using ASP_FinanceCalculator_Server.Models;
using Pluralize;
using Pluralize.NET;

namespace ASP_FinanceCalculator_Server.Repos
{
    public struct NadoMapperParameter
    {
        public string Name { get; }
        public object Value { get; }

        public NadoMapperParameter(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
    public class Repository<TModel> where TModel : ModelBase
    {

        private DataContext<TModel> _dataContext = new DataContext<TModel>();

        public bool VerifyInitialize()
        {
            _dataContext.VerifyInitialize();
            _dataContext.LoadConnectionString("localhost");

            return true;
        }

        public Task<IEnumerable<TModel>> GetAllAsync()
        {
            VerifyInitialize();

            return _dataContext.ExecuteReaderAsync();
        }

        public Task<IEnumerable<TModel>> GetAsync(NadoMapperParameter parameter)
        {
            VerifyInitialize();
            return null;
        }

        public Task<IEnumerable<TModel>> GetAsync(string procName, NadoMapperParameter parameter)
        {
            VerifyInitialize();
            return null;
        }

        public Task<IEnumerable<TModel>> GetAsync(IEnumerable<NadoMapperParameter> parameters = null)
        {
            VerifyInitialize();
            return null;
        }

        public Task<IEnumerable<TModel>> GetAsync(string procName, IEnumerable<NadoMapperParameter> parameters = null)
        {
            VerifyInitialize();
            return null;
        }

        public Task<TModel> GetSingleAsync(IEnumerable<NadoMapperParameter> parameters)
        {
            VerifyInitialize();
            return null;
        }

        public Task<TModel> GetSingleAsync(long id)
        {
            VerifyInitialize();
            return null;
        }

        public Task<TModel> GetSingleAsync(string procName, IEnumerable<NadoMapperParameter> parameters = null)
        {
            VerifyInitialize();
            return null;
        }

        public Task<TModel> GetSingleAsync(NadoMapperParameter parameter)
        {
            VerifyInitialize();
            return null;
        }

        public Task<TModel> GetSingleAsync(string procName, NadoMapperParameter parameter)
        {
            VerifyInitialize();
            return null;
        }

        public Task<TModel> AddAsync(TModel item, IEnumerable<NadoMapperParameter> parameters = null)
        {
            VerifyInitialize();
            return null;
        }

        public Task<TModel> AddUpdateAsync(TModel item)
        {
            VerifyInitialize();
            return null;
        }

        public Task<long> UpdateAsync(TModel item)
        {
            VerifyInitialize();
            return null;
        }

        public Task<long> DeleteAsync(TModel item)
        {
            VerifyInitialize();
            return null;
        }
    }
}