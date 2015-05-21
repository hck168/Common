using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Job.Framework
{
    public sealed class DbCount : DbContextOperate
    {
        internal DbCount(DbContext dbContext, string tableName)
        {
            base.DbContext = dbContext;
            base.TableName = tableName;
        }
    }
}
