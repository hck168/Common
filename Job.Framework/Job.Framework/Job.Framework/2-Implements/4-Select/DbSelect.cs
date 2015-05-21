using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Job.Framework
{
    public class DbSelect<T> : DbContextOperate where T : class,new()
    {
        internal string Sort { get; private set; }

        internal DbSelect(DbContext dbContext, string tableName, string sort)
        {
            base.DbContext = dbContext;
            base.TableName = tableName;

            this.Sort = sort;
        }
    }

    public sealed class DbSelectColumns<T> : DbSelect<T> where T : class,new()
    {
        internal string[] Columns { get; private set; }

        internal DbSelectColumns(DbContext dbContext, string tableName, string sort, string[] columns) : base(dbContext, tableName,sort)
        {
            this.Columns = columns;
        }
    }
}
