using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Pluralize.NET;

namespace ASP_FinanceCalculator_Server.Repos
{
    public class DataContext<TModel>
    {
        private SqlConnection _connection;
        private string _connectionString;

        private Pluralizer _pluralizer;
        private string _modelName => typeof(TModel).Name;
        private string _modelNamePlural => _pluralizer.Pluralize(_modelName);

        public void LoadConnectionString(string connectionString) => _connectionString = connectionString;

        private SqlCommand OpenConnection(string command, IEnumerable<NadoMapperParameter> parameters = null)
        {
            SqlCommand cmd = new SqlCommand(command,_connection) { CommandType = CommandType.StoredProcedure };

            if (parameters != null)
            {
                foreach (NadoMapperParameter parameter in parameters)
                    cmd.Parameters.Add(new SqlParameter(parameter.Name, parameter.Value));
            }

            cmd.Connection.Open();

            return cmd;
        }

        public bool VerifyInitialize()
        {
            _connection = new SqlConnection(_connectionString);
            _pluralizer = new Pluralizer();

            return true;
        }

        private TModel MapSingle(Dictionary<string, object> props) =>
            JsonConvert.DeserializeObject<TModel>(JsonConvert.SerializeObject(props));

        private TModel MapSingle(object model) =>
            JsonConvert.DeserializeObject<TModel>(JsonConvert.SerializeObject(model));

        public async Task<long> UpdateAsync(TModel model)
        {
            var props = model.GetType().GetProperties();

            // use prop names + values to generate list of NadoMapperParameter
            // pass params + "Update[modelName]" to ExecuteScalar + return result

            return 1;
        }

        public async Task<IEnumerable<TModel>> ExecuteReaderAsync(string command, IEnumerable<NadoMapperParameter> parameters = null)
        {
            var cmd = OpenConnection(command,parameters);

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

        public async Task<TModel> ExecuteScalarAsync(string command, IEnumerable<NadoMapperParameter> parameters = null)
        {
            var cmd = OpenConnection(command, parameters);

            var data = await cmd.ExecuteScalarAsync();

            cmd.Connection.Close();
            return MapSingle(data);
        }

        public async Task<TModel> ExecuteScalarAsync(string command, NadoMapperParameter parameters)
            => await ExecuteScalarAsync(command, new List<NadoMapperParameter>() {parameters});

        public async Task<long> ExecuteNonQueryAsync(string command, IEnumerable<NadoMapperParameter> parameters = null)
        {
            var cmd = OpenConnection(command, parameters);

            var rowsUpdated = await cmd.ExecuteNonQueryAsync();

            cmd.Connection.Close();
            return rowsUpdated;
        }
    }
}