using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Pluralize.NET;

namespace ASP_FinanceCalculator_Server.Repos
{
    public abstract class DataContext<TModel>
    {
        private SqlConnection _connection;
        private string _connectionString;

        private Pluralizer _pluralizer;
        private string _modelName => typeof(TModel).Name;
        private string _modelNamePlural => _pluralizer.Pluralize(_modelName);

        private SqlCommand OpenConnection(string procName, List<NadoMapperParameter> parameters = null)
        {
            SqlCommand cmd = new SqlCommand(procName) { CommandType = CommandType.StoredProcedure };

            foreach (NadoMapperParameter parameter in parameters)
                cmd.Parameters.Add(new SqlParameter(parameter.Name, parameter.Value));

            _connection.Open();

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

        public IEnumerable<TModel> ExecuteReaderAsync()
        {
            var cmd = OpenConnection("GetAll" + _modelNamePlural);
            var data = cmd.ExecuteReader();

            _connection.Close();

            //TODO: Create mapping function for SINGLE and ALL to map reader data to TModel

            return null;
        }
    }
}