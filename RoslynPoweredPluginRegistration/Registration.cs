using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynPoweredPluginRegistration
{
    class Registration
    {
        public Registration()
        {
            var a = new Account();
            var attrs = new object[] { a.AccountCategoryCode, a.ParentAccountId, a.Address1_Country };
        }
    }
}
