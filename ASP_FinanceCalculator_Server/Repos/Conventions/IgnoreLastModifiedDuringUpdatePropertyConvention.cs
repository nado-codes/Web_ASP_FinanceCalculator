using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP_FinanceCalculator_Server.Repos.Conventions
{
    public class IgnoreLastModifiedDuringUpdatePropertyConvention : PropertyConventionBase
    {
        public IgnoreLastModifiedDuringUpdatePropertyConvention()
        {
            PropertyName = "LastModified";
            CRUDType = CRUDType.Update;
        }
    }
}