using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities
{
    public class ErrorLogs
    {
        public virtual long Id { get; set; }
        public virtual string ExceptionErrorLog { get; set; }
    }
}
