using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP_FinanceCalculator_Server.Repos.Conventions
{
    public class PropertyConventionBase
    {
        public string PropertyName { get; set; }
        public CRUDType CRUDType { get; set; }
    }
}