using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegistrationTarget
{
    public interface IRegistrationTarget
    {
        Steps Register();
    }
}
