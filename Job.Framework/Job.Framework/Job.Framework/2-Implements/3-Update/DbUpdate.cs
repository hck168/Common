using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Job.Framework
{
    public class DbUpdate : DbContextOperate
    {
        internal DbUpdate(DbContext dbContext, string tableName)
        {
            base.DbContext = dbContext;
            base.TableName = tableName;
        }
    }

    public sealed class DbUpdateSet : DbUpdate
    {
        internal object ColumnData { get; set; }

        internal DbUpdateSet(DbContext dbContext, string tableName, object columnData) : base(dbContext,tableName)
        {
            this.ColumnData = columnData;
        }
    }
}
