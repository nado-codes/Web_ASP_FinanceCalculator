using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ASP_FinanceCalculator_Server.Repos
{
    public class RepositoryBase<Model> : Repository<Model> where Model: new()
    {
        
    }
}