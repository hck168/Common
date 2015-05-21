using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Job.Framework
{
    public class DbQuery<T> : DbContextOperate where T : class,new()
    {
        internal int Top { get; private set; }
        internal string Sort { get; private set; }

        internal DbQuery(DbContext dbContext, string tableName, int top, string sort)
        {
            base.DbContext = dbContext;
            base.TableName = tableName;

            this.Top = top;
            this.Sort = sort;
        }
    }

    public sealed class DbQueryColumns<T> : DbQuery<T> where T : class,new()
    {
        internal string[] Columns { get; private set; }

        internal DbQueryColumns(DbContext dbContext, string tableName, int top, string sort, string[] columns) : base(dbContext, tableName, top, sort)
        {
            this.Columns = columns;
        }
    }
}
