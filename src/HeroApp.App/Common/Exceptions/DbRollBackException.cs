using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HeroApp.App.Common.Exceptions
{
    public class DbRollBackException: Exception
    {
        public DbRollBackException(Exception innerException): base("The Database Transaction was RolledBack!",  innerException.InnerException)
        {
            
            var stack = innerException.StackTrace;
            if (stack != null)
            {
                var arr = stack.Split(Environment.NewLine);
                ListStatckTrace = arr.ToList();
            }
        }

        public IEnumerable<string> ListStatckTrace { get; private set; }
    }
}

