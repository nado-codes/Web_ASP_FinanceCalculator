using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP_FinanceCalculator_Server.Repos.Conventions
{
    public class IgnoreDateAddedDuringUpdatePropertyConvention : PropertyConventionBase
    {
        public IgnoreDateAddedDuringUpdatePropertyConvention()
        {
            PropertyName = "DateAdded";
            CRUDType = CRUDType.Update;
        }
    }
}