using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using ASP_FinanceCalculator_Server.Models;
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

    public class DataContext<TEntity> where TEntity: ModelBase
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

        public async Task<IEnumerable<TEntity>> GetAllAsync() => await ExecuteReaderAsync($"Get{_modelNamePlural}");

        public async Task<TEntity> GetSingleAsync(NadoMapperParameter parameter)
        {
            var parameterName = parameter.Name[0] + parameter.Name.Substring(1);
            var cmd = OpenConnection($"Get{_modelName}By{parameterName}", CRUDType.Read, parameter);

            var data = await cmd.ExecuteScalarAsync();

            return MapSingle(data);
        }

        public async Task<TEntity> GetSingleAsync(long id) =>
            (await ExecuteReaderAsync($"Get{_modelName}ById", new NadoMapperParameter() {Name = "id", Value = id}))
            .FirstOrDefault();

        public async Task<TEntity> GetSingleByNameAsync(string name)
        {
            var cmd = OpenConnection($"Get{_modelName}ByName", CRUDType.Read, new NadoMapperParameter() { Name = "name", Value = name });

            var data = await cmd.ExecuteScalarAsync();

            return MapSingle(data);
        }

        public async Task<TEntity> GetSingleAsync(string procName, IEnumerable<NadoMapperParameter> parameters = null)
        {
            var cmd = OpenConnection(procName, CRUDType.Read, CommandType.StoredProcedure, parameters);

            var data = await cmd.ExecuteScalarAsync();

            return MapSingle(data);
        }

        public async Task<TEntity> AddAsync(TEntity model)
        {
            var cmd = OpenConnection($"Add{_modelName}", CRUDType.Create, CommandType.StoredProcedure,GetParamsFromModel(model));
            var id = await cmd.ExecuteScalarAsync();
            cmd.Connection.Close();

            cmd = OpenConnection($"SELECT * from {_modelNamePlural} where Id={id}", CRUDType.Read, CommandType.Text);
            var data = await cmd.ExecuteReaderAsync();
            
            data.Read();

            var objectProps = new Dictionary<string, object>();

            for (int i = 0; i < data.VisibleFieldCount; ++i)
                objectProps.Add(data.GetName(i), data.GetValue(i));

            cmd.Connection.Close();

            return MapSingle(objectProps);
        }

        public async Task<long> UpdateAsync(TEntity model) => await ExecuteNonQueryAsync($"Update{_modelName}", CRUDType.Update, GetParamsFromModel(model));

        public async Task<long> DeleteAsync(TEntity model) 
            => await ExecuteNonQueryAsync($"Delete{_modelName}", CRUDType.Update, new List<NadoMapperParameter>()
            {
                new NadoMapperParameter(){Name="id",Value=model.Id},
                new NadoMapperParameter(){Name="lastModified",Value=model.LastModified}
            });

        public async Task<IEnumerable<TEntity>> ExecuteReaderAsync(string command, IEnumerable<NadoMapperParameter> parameters = null)
        {
            var cmd = OpenConnection(command,CRUDType.Read,CommandType.StoredProcedure,parameters);

            var data = await cmd.ExecuteReaderAsync();

            var models = new List<TEntity>();

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

        public async Task<IEnumerable<TEntity>> ExecuteReaderAsync(string command, NadoMapperParameter parameter)
            => await ExecuteReaderAsync(command, new List<NadoMapperParameter>() { parameter });

        public async Task<object> ExecuteScalarAsync(string command, CRUDType crudType, IEnumerable<NadoMapperParameter> parameters = null)
        {
            var cmd = OpenConnection(command, crudType, CommandType.StoredProcedure, parameters);

            var data = await cmd.ExecuteScalarAsync();

            cmd.Connection.Close();
            return data;
        }

        public async Task<object> ExecuteScalarAsync(string command, CRUDType crudType, NadoMapperParameter parameter)
            => await ExecuteScalarAsync(command, crudType, new List<NadoMapperParameter>() {parameter});

        public async Task<long> ExecuteNonQueryAsync(string command, CRUDType crudType, IEnumerable<NadoMapperParameter> parameters = null)
        {
            var cmd = OpenConnection(command, crudType, CommandType.StoredProcedure, parameters);

            var rowsUpdated = await cmd.ExecuteNonQueryAsync();

            cmd.Connection.Close();
            return rowsUpdated;
        }
    }
}