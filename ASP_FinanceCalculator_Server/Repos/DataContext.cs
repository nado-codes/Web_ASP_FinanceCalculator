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

    public class DataContext<TEntity>
    {
        private SqlConnection _connection;
        private string _connectionString;
        public List<PropertyConventionBase> PropertyConventions;

        private Pluralizer _pluralizer;
        private string _modelName => typeof(TEntity).Name;
        private string _modelNamePlural => _pluralizer.Pluralize(_modelName);

        public void LoadConnectionString(string connectionString) => _connectionString = connectionString;

        public bool VerifyInitialize()
        {
            _connection = new SqlConnection(_connectionString);
            _pluralizer = new Pluralizer();
            PropertyConventions = new List<PropertyConventionBase>();

            return true;
        }

        protected IEnumerable<NadoMapperParameter> GetParamsFromModel(TEntity model)
        {
            var parameters = new List<NadoMapperParameter>();
            foreach (PropertyInfo prop in model.GetType().GetProperties())
                parameters.Add(new NadoMapperParameter() { Name = prop.Name, Value = prop.GetValue(model) });

            return parameters;
        }

        public TEntity MapSingle(Dictionary<string, object> props) =>
            JsonConvert.DeserializeObject<TEntity>(JsonConvert.SerializeObject(props));

        public TEntity MapSingle(object model) =>
            JsonConvert.DeserializeObject<TEntity>(JsonConvert.SerializeObject(model));

        private SqlCommand OpenConnection(string command, CRUDType crudType, NadoMapperParameter parameter)
            => OpenConnection(command, crudType, CommandType.StoredProcedure, new List<NadoMapperParameter>() { parameter });

        private SqlCommand OpenConnection(string command, CRUDType crudType, CommandType commandType, IEnumerable<NadoMapperParameter> parameters = null)
        {
            SqlCommand cmd = new SqlCommand(command,_connection) { CommandType = commandType };

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

       

        

        

        

        /* public Task<TModel> GetSingleAsync(long id)
        {
            var cmd = OpenConnection("Add" + _modelName, CRUDType.Read);

            var data = Task.Run(cmd.ExecuteScalar());

            return MapSingle(data);
        } */

        //TODO: How to map with tasks?
        public Task<TEntity> MapWithTask(Task obj)
        {
            //return obj.R
            return null;
        }
        public Task<TEntity> AddAsync(TEntity model)
        {
            var cmd = OpenConnection("Add" + _modelName, CRUDType.Create, CommandType.StoredProcedure,GetParamsFromModel(model));
            var id = Task.WhenAll(cmd.ExecuteScalarAsync());
            cmd.Connection.Close();

            //yield 

            cmd = OpenConnection($"SELECT * from {_modelNamePlural} where Id={id}", CRUDType.Read, CommandType.Text);
            var data = (cmd.ExecuteReaderAsync()).Result;
            
            data.Read();

            var objectProps = new Dictionary<string, object>();

            for (int i = 0; i < data.VisibleFieldCount; ++i)
                objectProps.Add(data.GetName(i), data.GetValue(i));

            cmd.Connection.Close();

            return Task.FromResult(MapSingle(objectProps));
        }

        public async Task<long> UpdateAsync(TEntity model) => await ExecuteNonQueryAsync("Update" + _modelName, CRUDType.Update, GetParamsFromModel(model));

        public Task<IEnumerable<TEntity>> ExecuteReaderAsync(string command, IEnumerable<NadoMapperParameter> parameters = null)
        {
            var cmd = OpenConnection(command,CRUDType.Read,CommandType.StoredProcedure,parameters);

            var data = cmd.ExecuteReader();

            var models = new List<TEntity>();

            while (data.Read())
            {
                var objectProps = new Dictionary<string, object>();

                for (int i = 0; i < data.VisibleFieldCount; ++i)
                    objectProps.Add(data.GetName(i), data.GetValue(i));

                models.Add(MapSingle(objectProps));
            }

            cmd.Connection.Close();
            return Task.FromResult<IEnumerable<TEntity>>(models);
        }

        public async Task<IEnumerable<TEntity>> ExecuteReaderAsync(string command, NadoMapperParameter parameter)
            => await ExecuteReaderAsync(command, new List<NadoMapperParameter>() { parameter });

        public async Task<object> ExecuteScalarAsync(string command, CRUDType crudType, IEnumerable<NadoMapperParameter> parameters = null)
        {
            var cmd = OpenConnection(command, crudType, CommandType.StoredProcedure, parameters);

            var data = await cmd.ExecuteScalarAsync();

            cmd.Connection.Close();
            return MapSingle(data);
        }

        public async Task<object> ExecuteScalarAsync(string command, CRUDType crudType, NadoMapperParameter parameters)
            => await ExecuteScalarAsync(command, crudType, new List<NadoMapperParameter>() {parameters});

        public async Task<long> ExecuteNonQueryAsync(string command, CRUDType crudType, IEnumerable<NadoMapperParameter> parameters = null)
        {
            var cmd = OpenConnection(command, crudType, CommandType.StoredProcedure, parameters);

            var rowsUpdated = await cmd.ExecuteNonQueryAsync();

            cmd.Connection.Close();
            return rowsUpdated;
        }
    }
}