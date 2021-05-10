using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
        private string _connectionString;

        private SqlConnection _connection;

        private Pluralizer _pluralizer;

        private string ModelName => typeof(TModel).Name;

        private string ModelNamePlural => _pluralizer.Pluralize(ModelName);

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
            SqlCommand cmd = new SqlCommand(procName) {CommandType = CommandType.StoredProcedure};

            foreach (NadoMapperParameter parameter in parameters)
                cmd.Parameters.Add(new SqlParameter(parameter.Name, parameter.Value));

            _connection.Open();

            return cmd;
        }

        public bool VerifyInitialize()
        {
            _connection = new SqlConnection();
            _pluralizer = new Pluralizer();

            return true;
        }

        public Task<IEnumerable<TModel>> GetAllAsync()
        {
            var cmd = OpenConnection("GetAll" + ModelNamePlural);
            var data = cmd.ExecuteReader();

            _connection.Close();

            //TODO: Create mapping function for SINGLE and ALL to map reader data to TModel

            return null;
        }

        public Task<IEnumerable<TModel>> GetAsync(NadoMapperParameter parameter)
        {
            return null;
        }

        public Task<IEnumerable<TModel>> GetAsync(string procName, NadoMapperParameter parameter)
        {
            return null;
        }

        public Task<IEnumerable<TModel>> GetAsync(IEnumerable<NadoMapperParameter> parameters = null)
        {
            return null;
        }

        public Task<IEnumerable<TModel>> GetAsync(string procName, IEnumerable<NadoMapperParameter> parameters = null)
        {
            return null;
        }

        public Task<TModel> GetSingleAsync(IEnumerable<NadoMapperParameter> parameters)
        {
            return null;
        }

        public Task<TModel> GetSingleAsync(long id)
        {
            return null;
        }

        public Task<TModel> GetSingleAsync(string procName, IEnumerable<NadoMapperParameter> parameters = null)
        {
            return null;
        }

        public Task<TModel> GetSingleAsync(NadoMapperParameter parameter)
        {
            return null;
        }

        public Task<TModel> GetSingleAsync(string procName, NadoMapperParameter parameter)
        {
            return null;
        }

        public Task<TModel> AddAsync(TModel item, IEnumerable<NadoMapperParameter> parameters = null)
        {
            return null;
        }

        public Task<TModel> AddUpdateAsync(TModel item)
        {
            return null;
        }

        public Task<long> UpdateAsync(TModel item)
        {
            return null;
        }

        public Task<long> DeleteAsync(TModel item)
        {
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