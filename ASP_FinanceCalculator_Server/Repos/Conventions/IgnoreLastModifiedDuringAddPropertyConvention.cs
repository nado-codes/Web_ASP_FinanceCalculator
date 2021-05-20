using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP_FinanceCalculator_Server.Repos.Conventions
{
    public class IgnoreLastModifiedDuringAddPropertyConvention : PropertyConventionBase
    {
        public IgnoreLastModifiedDuringAddPropertyConvention()
        {
            PropertyName = "LastModified";
            CRUDType = CRUDType.Create;
        }
    }
}