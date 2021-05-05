using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

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
    public class Repository<Entity>
    {
        private string _connectionString;

        private SqlConnection _connection;
        //SqlCommand cmd = new SqlCommand("PROC",_connection)
        //Connection.open

        //cmd.CommandType = System.Data.CommandType.StoredProcedure

        //cmd.Parameters.Add(new SqlParameter("PARAM_NAME",VALUE))

        //SqlDataReader reader = cmd.ExecuteReader;

        //var models = new List<Code>();

        //while (reader.Read())
        //for(int i = 0; i < reader.VisibleFieldCount; ++i)
        // [dictionary].Add(reader.GetName(i),reader.GetValue(i));

        // PCMAPPER.Map([dictionary],new Code())

        // _connection.Close();

        // return models;

        // == PCMAPPER ==
        // [MODEL TYPE] mappedObject = JsonConvert.DeserializeObject<MODEL TYPE>(JsonConvert.SerializeObject(source))

        // return mappedObject

        public void LoadConnectionString(string connectionString) => _connectionString = connectionString;

        private SqlCommand OpenConnection(string procName, List<NadoMapperParameter> parameters = null)
        {
            SqlCommand cmd = new SqlCommand(procName);

            return null;
        }

        private void Foo()
        {
            SqlCommand cmd = new SqlCommand();

            cmd.ExecuteNonQueryAsync();
            cmd.ExecuteReaderAsync();
            cmd.ExecuteScalarAsync();
        }
    }
}