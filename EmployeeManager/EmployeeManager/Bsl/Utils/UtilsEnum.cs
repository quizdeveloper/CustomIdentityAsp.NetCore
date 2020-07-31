using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.Bsl.Utils
{
    public enum DbHelperEnum
    {
        [Description("Store procedure")]
        StoredProcedure = 1,

        [Description("Command line")]
        Text = 2
    }
}
