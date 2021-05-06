using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ASP_FinanceCalculator_Server.Models;

namespace ASP_FinanceCalculator_Server.Repos
{
    public class RepositoryBase<TModel> : Repository<TModel> where TModel: ModelBase, new() 
    {
        
    }
}