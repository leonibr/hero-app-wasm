using System;
using System.Collections.Generic;
using System.Text;

namespace HeroApp.App.Common.Exceptions
{
    public class DbRollBackException: Exception
    {
        public DbRollBackException(Exception innerException): base("The Database Transaction was RolledBack!",  innerException)
        {

        }
    }
}

