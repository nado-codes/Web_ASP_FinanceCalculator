using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP_FinanceCalculator_Server.Repos.Conventions
{
    public class IgnoreIdDuringAddPropertyConvention : PropertyConventionBase
    {
        public IgnoreIdDuringAddPropertyConvention()
        {
            PropertyName = "Id";
            CRUDType = CRUDType.Create;
        }
    }
}