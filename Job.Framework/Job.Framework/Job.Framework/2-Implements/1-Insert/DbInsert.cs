using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Job.Framework
{
    public sealed class DbInsert : DbContextOperate
    {
        internal bool IsAutoIncrement { get; private set; }

        internal DbInsert(DbContext dbContext, string tableName, bool isAutoIncrement)
        {
            base.DbContext = dbContext;
            base.TableName = tableName;

            this.IsAutoIncrement = isAutoIncrement;
        }
    }
}
