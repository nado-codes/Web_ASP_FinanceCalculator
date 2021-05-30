using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Web;
using ASP_FinanceCalculator_Server.Models;
using ASP_FinanceCalculator_Server.Repos.Conventions;
using Pluralize;
using Pluralize.NET;

namespace ASP_FinanceCalculator_Server.Repos
{
    
    public class RepositoryBase<TModel> where TModel : ModelBase
    {
        private Pluralizer _pluralizer;
        private string _modelName => typeof(TModel).Name;
        private string _modelNamePlural => _pluralizer.Pluralize(_modelName);

        private DataContext<TModel> _dataContext = new DataContext<TModel>();

        public bool VerifyInitialize()
        {
            _dataContext.LoadConnectionString("Data Source=localhost;Initial Catalog=TestDB;Integrated Security=True;");
            _dataContext.VerifyInitialize();
            _pluralizer = new Pluralizer();

            _dataContext.PropertyConventions.Add(new IgnoreDateAddedDuringAddPropertyConvention());
            _dataContext.PropertyConventions.Add(new IgnoreLastModifiedDuringAddPropertyConvention());
            _dataContext.PropertyConventions.Add(new IgnoreDateAddedDuringUpdatePropertyConvention());
            _dataContext.PropertyConventions.Add(new IgnoreLastModifiedDuringUpdatePropertyConvention());
            _dataContext.PropertyConventions.Add(new IgnoreIdDuringAddPropertyConvention());

            return true;
        }

        public Task<IEnumerable<TModel>> GetAllAsync()
        {
            VerifyInitialize();

            return _dataContext.ExecuteReaderAsync("Get" + _modelNamePlural);
        }

        public Task<IEnumerable<TModel>> GetAsync(NadoMapperParameter parameter)
        {
            VerifyInitialize();

            return null;
        }

        public Task<IEnumerable<TModel>> GetAsync(string procName, NadoMapperParameter parameter)
        {
            VerifyInitialize();
            return _dataContext.ExecuteReaderAsync(procName,parameter);
        }

        public Task<IEnumerable<TModel>> GetAsync(IEnumerable<NadoMapperParameter> parameters = null)
        {
            VerifyInitialize();
            return null;
        }

        public Task<IEnumerable<TModel>> GetAsync(string procName, IEnumerable<NadoMapperParameter> parameters = null)
        {
            VerifyInitialize();
            return _dataContext.ExecuteReaderAsync(procName,parameters);
        }

        public Task<TModel> GetSingleAsync(IEnumerable<NadoMapperParameter> parameters)
        {
            VerifyInitialize();

            return null;
        }

        // TODO: Update when DataContext has built-in methods for GetSingleAsync, returning "Task"
        public Task<TModel> GetSingleAsync(long id)
        {
            VerifyInitialize();
            return null; //_dataContext.ExecuteScalarAsync("Get"+_modelName+"ById", CRUDType.Read, new NadoMapperParameter{Name="id", Value=id});
        }

        // TODO: Update when DataContext has built-in methods for GetSingleAsync, returning "Task"
        public Task<TModel> GetSingleAsync(string procName, IEnumerable<NadoMapperParameter> parameters = null)
        {
            VerifyInitialize();
            return null; //_dataContext.ExecuteScalarAsync(procName,CRUDType.Read, parameters);
        }

        public Task<TModel> GetSingleAsync(NadoMapperParameter parameter)
        {
            VerifyInitialize();
            return null;
        }

        // TODO: Update when DataContext has built-in methods for GetSingleAsync, returning "Task"
        public Task<TModel> GetSingleAsync(string procName, NadoMapperParameter parameter)
        {
            VerifyInitialize();

            //var model =  TODO: How to handle mapping within tasks C#?

            return null; //_dataContext.MapSingle(_dataContext.ExecuteScalarAsync(procName,CRUDType.Read, parameter));
        }

        public Task<TModel> AddAsync(TModel item)
        {
            VerifyInitialize();

            return _dataContext.AddAsync(item);
        }

        /*{
            VerifyInitialize();

            var id = await _dataContext.ExecuteScalarAsync("Add" + _modelName, CRUDType.Create,
                _dataContext.GetParamsFromModel(item));

            return null;
        }*/

        public Task<TModel> AddUpdateAsync(TModel item)
        {
            VerifyInitialize();
            return null;
        }

        public Task<long> UpdateAsync(TModel item)
        {
            VerifyInitialize();
            return _dataContext.UpdateAsync(item);
        }

        public Task<long> DeleteAsync(TModel item)
        {
            VerifyInitialize();
            return null;
        }
    }
}