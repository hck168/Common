using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Job.Framework
{
    public static class DbSelectExtensions
    {
        public static dynamic Where(this DbSelectColumns<dynamic> dbSelectColumns, string conditionFormat, object conditionData)
        {
            return Where<dynamic>(dbSelectColumns, conditionFormat, conditionData);
        }

        public static dynamic Where(this DbSelectColumns<dynamic> dbSelectColumns, bool isOr, object conditionData)
        {
            return Where<dynamic>(dbSelectColumns, isOr, conditionData);
        }

        public static dynamic Where(this DbSelectColumns<dynamic> dbSelectColumns, object conditionData)
        {
            return Where<dynamic>(dbSelectColumns, conditionData);
        }

        public static T Where<T>(this DbSelectColumns<T> dbSelectColumns, string conditionFormat, object conditionData) where T : class,new()
        {
            return dbSelectColumns.DbContext.Query<T>(dbSelectColumns.TableName, 1, dbSelectColumns.Sort).Columns(dbSelectColumns.Columns).Where(conditionFormat, conditionData).SingleOrDefault();
        }

        public static T Where<T>(this DbSelectColumns<T> dbSelectColumns, bool isOr, object conditionData) where T : class,new()
        {
            return dbSelectColumns.DbContext.Query<T>(dbSelectColumns.TableName, 1, dbSelectColumns.Sort).Columns(dbSelectColumns.Columns).Where(isOr, conditionData).SingleOrDefault();
        }

        public static T Where<T>(this DbSelectColumns<T> dbSelectColumns, object conditionData) where T : class,new()
        {
            return dbSelectColumns.DbContext.Query<T>(dbSelectColumns.TableName, 1, dbSelectColumns.Sort).Columns(dbSelectColumns.Columns).Where(false, conditionData).SingleOrDefault();
        }
    }
}
