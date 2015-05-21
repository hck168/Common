using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Job.Framework
{
    public abstract class DbContextOperate
    {
        internal DbContext DbContext { get; set; }
        internal string TableName { get; set; }
    }
}
