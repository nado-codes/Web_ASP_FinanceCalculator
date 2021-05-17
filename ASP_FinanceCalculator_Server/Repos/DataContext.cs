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

        private SqlCommand OpenConnection(string procName, List<NadoMapperParameter> parameters = null)
        {
            SqlCommand cmd = new SqlCommand(procName,_connection) { CommandType = CommandType.StoredProcedure };

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

        //var models = new List<Code>();

        // Dictionary<string,object> [dictionary]

        //while (reader.Read())
        //for(int i = 0; i < reader.VisibleFieldCount; ++i)
        // [dictionary].Add(reader.GetName(i),reader.GetValue(i));

        // PCMAPPER.Map([dictionary],new Code())

        // _connection.Close();

        // return models;

        // == PCMAPPER ==
        // [MODEL TYPE] mappedObject = JsonConvert.DeserializeObject<MODEL TYPE>(JsonConvert.SerializeObject(source))

        // return mappedObject

        private TModel MapSingle(Dictionary<string, object> props) =>
            JsonConvert.DeserializeObject<TModel>(JsonConvert.SerializeObject(props));

        public async Task<IEnumerable<TModel>> ExecuteReaderAsync()
        {
            var cmd = OpenConnection("GetAll" + _modelNamePlural);
            var data = await cmd.ExecuteReaderAsync();

            var models = new List<TModel>();

            while (data.Read())
            {
                var objectProps = new Dictionary<string,object>();

                for(int i = 0; i < data.VisibleFieldCount; ++i)
                    objectProps.Add(data.GetName(i),data.GetValue(i));

                models.Add(MapSingle(objectProps));
            }

            _connection.Close();

            return models;
        }

        /* public async Task<TModel> ExecuteScalarAsync(string cmd, IEnumerable<NadoMapperParameter> parameters = null)
        {

        } */
    }
}