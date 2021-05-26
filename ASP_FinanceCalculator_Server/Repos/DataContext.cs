using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using ASP_FinanceCalculator_Server.Repos.Conventions;
using Newtonsoft.Json;
using Pluralize.NET;

namespace ASP_FinanceCalculator_Server.Repos
{
    public enum CRUDType
    {
        Create,
        Read,
        Update,
        Delete
    }

    public struct NadoMapperParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }

    public class DataContext<TModel>
    {
        private SqlConnection _connection;
        private string _connectionString;
        public List<PropertyConventionBase> PropertyConventions;

        private Pluralizer _pluralizer;
        private string _modelName => typeof(TModel).Name;
        private string _modelNamePlural => _pluralizer.Pluralize(_modelName);

        public void LoadConnectionString(string connectionString) => _connectionString = connectionString;

        private SqlCommand OpenConnection(string command, CRUDType crudType, IEnumerable<NadoMapperParameter> parameters = null)
        {
            SqlCommand cmd = new SqlCommand(command,_connection) { CommandType = CommandType.StoredProcedure };

            if (parameters != null)
            {
                foreach (NadoMapperParameter parameter in parameters)
                {
                    if(!PropertyConventions.Any(x => x.PropertyName == parameter.Name && x.CRUDType == crudType))
                        cmd.Parameters.Add(new SqlParameter(parameter.Name, parameter.Value));
                }
                    
            }

            cmd.Connection.Open();

            return cmd;
        }

        public bool VerifyInitialize()
        {
            _connection = new SqlConnection(_connectionString);
            _pluralizer = new Pluralizer();
            PropertyConventions = new List<PropertyConventionBase>();

            return true;
        }

        public TModel MapSingle(Dictionary<string, object> props) =>
            JsonConvert.DeserializeObject<TModel>(JsonConvert.SerializeObject(props));

        public TModel MapSingle(object model) =>
            JsonConvert.DeserializeObject<TModel>(JsonConvert.SerializeObject(model));

        public IEnumerable<NadoMapperParameter> GetParamsFromModel(TModel model)
        {
            var parameters = new List<NadoMapperParameter>();
            foreach (PropertyInfo prop in model.GetType().GetProperties())
                parameters.Add(new NadoMapperParameter(){Name= prop.Name, Value=prop.GetValue(model)});

            return parameters;
        }

        // TODO: How to map a returned object while returning a task? Make MapSingle return a task sometimes?
        /* public Task<TModel> GetSingleAsync(IEnumerable<NadoMapperParameter> parameters)
            => MapSingle(ExecuteScalarAsync("Get" + _modelName + "ById", CRUDType.Read,
                parameters)); */

        public async Task<TModel> AddAsync(TModel model)
        {
            var id = await ExecuteScalarAsync("Add" + _modelName, CRUDType.Create, GetParamsFromModel(model));

            return MapSingle(await ExecuteScalarAsync("Get" + _modelName + "ById", CRUDType.Read,
                new NadoMapperParameter() {Name = "id", Value = id}));
        }

        public async Task<long> UpdateAsync(TModel model) => await ExecuteNonQueryAsync("Update" + _modelName, CRUDType.Update, GetParamsFromModel(model));

        public async Task<IEnumerable<TModel>> ExecuteReaderAsync(string command, IEnumerable<NadoMapperParameter> parameters = null)
        {
            var cmd = OpenConnection(command,CRUDType.Read,parameters);

            var data = await cmd.ExecuteReaderAsync();

            var models = new List<TModel>();

            while (data.Read())
            {
                var objectProps = new Dictionary<string, object>();

                for (int i = 0; i < data.VisibleFieldCount; ++i)
                    objectProps.Add(data.GetName(i), data.GetValue(i));

                models.Add(MapSingle(objectProps));
            }

            cmd.Connection.Close();
            return models;
        }

        public async Task<IEnumerable<TModel>> ExecuteReaderAsync(string command, NadoMapperParameter parameter)
            => await ExecuteReaderAsync(command, new List<NadoMapperParameter>() { parameter });

        public async Task<object> ExecuteScalarAsync(string command, CRUDType crudType, IEnumerable<NadoMapperParameter> parameters = null)
        {
            var cmd = OpenConnection(command, crudType, parameters);

            var data = await cmd.ExecuteScalarAsync();

            cmd.Connection.Close();
            return MapSingle(data);
        }

        public async Task<object> ExecuteScalarAsync(string command, CRUDType crudType, NadoMapperParameter parameters)
            => await ExecuteScalarAsync(command, crudType, new List<NadoMapperParameter>() {parameters});

        public async Task<long> ExecuteNonQueryAsync(string command, CRUDType crudType, IEnumerable<NadoMapperParameter> parameters = null)
        {
            var cmd = OpenConnection(command, crudType, parameters);

            var rowsUpdated = await cmd.ExecuteNonQueryAsync();

            cmd.Connection.Close();
            return rowsUpdated;
        }
    }
}