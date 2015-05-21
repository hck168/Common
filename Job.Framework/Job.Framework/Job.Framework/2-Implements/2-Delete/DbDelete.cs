using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Job.Framework
{
    public sealed class DbDelete : DbContextOperate
    {
        internal DbDelete(DbContext dbContext, string tableName)
        {
            base.DbContext = dbContext;
            base.TableName = tableName;
        }
    }
}
