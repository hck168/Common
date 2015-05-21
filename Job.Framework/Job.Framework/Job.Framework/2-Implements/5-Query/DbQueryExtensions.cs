using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Job.Framework
{
    public static class DbQueryExtensions
    {
        public static IEnumerable<dynamic> Where(this DbQueryColumns<dynamic> dbQueryColumns, string conditionFormat, object conditionData)
        {
            return Where<dynamic>(dbQueryColumns, conditionFormat, conditionData);
        }

        public static IEnumerable<dynamic> Where(this DbQueryColumns<dynamic> dbQueryColumns, object conditionData)
        {
            return Where<dynamic>(dbQueryColumns, false, conditionData);
        }

        public static IEnumerable<dynamic> Where(this DbQueryColumns<dynamic> dbQueryColumns, bool isOr, object conditionData)
        {
            return Where<dynamic>(dbQueryColumns, isOr, conditionData);
        }

        public static IEnumerable<T> Where<T>(this DbQueryColumns<T> dbQueryColumns, object conditionData) where T : class,new()
        {
            return Where<T>(dbQueryColumns, false, conditionData);
        }

        public static IEnumerable<T> Where<T>(this DbQueryColumns<T> dbQueryColumns, string conditionFormat, object conditionData) where T : class,new()
        {
            var condition = "1 = 1";

            var dbParameters = conditionData != null ? dbQueryColumns.DbContext.GetParameters(conditionData) : null;

            if (!string.IsNullOrWhiteSpace(conditionFormat))
            {
                condition = conditionFormat;
            }

            return dbQueryColumns.Execute(condition, dbParameters);
        }

        public static IEnumerable<T> Where<T>(this DbQueryColumns<T> dbQueryColumns, bool isOr, object conditionData) where T : class,new()
        {
            var condition = "1 = 1";

            var dbParameters = conditionData != null ? dbQueryColumns.DbContext.GetParameters(conditionData) : null;

            if (dbParameters != null && dbParameters.Any())
            {
                condition = string.Join(isOr ? " OR " : " AND ", dbParameters.Select(p => string.Format("{0} = {1}", p.ParameterName.TrimStart('@'), p.ParameterName)));
            }

            return dbQueryColumns.Execute<T>(condition, dbParameters);
        }

        private static IEnumerable<T> Execute<T>(this DbQueryColumns<T> dbQueryColumns, string condition, DbParameter[] dbParameters) where T : class,new()
        {
            var columns = "*";

            if (dbQueryColumns.Columns.Length > 0)
            {
                columns = string.Join(",", dbQueryColumns.Columns);
            }

            var sql = string.Format(@"SELECT {0}{1} FROM {2}", dbQueryColumns.Top > 0 ? "TOP " + dbQueryColumns.Top + " " : string.Empty, columns, dbQueryColumns.TableName);

            if (!string.IsNullOrWhiteSpace(condition))
            {
                sql += string.Format(" WHERE {0}", condition);
            }

            if (!string.IsNullOrWhiteSpace(dbQueryColumns.Sort))
            {
                sql += string.Format(" ORDER BY {0}", dbQueryColumns.Sort);
            }

            var isDynamic = string.Equals(typeof(T).FullName, "System.Object");

            if (isDynamic == false)
            {
                var columnProperties = dbQueryColumns.DbContext.GetProperties(typeof(T));

                if (columnProperties.Length == 0)
                {
                    throw new ArgumentNullException("实体模型对象未实现任何属性");
                }
            }

            using (var daReader = dbQueryColumns.DbContext.ExecuteReader(sql, dbParameters))
            {
                while (daReader.Read())
                {
                    if (isDynamic)
                    {
                        yield return dbQueryColumns.DbContext.TransferType(daReader);
                    }
                    else
                    {
                        yield return dbQueryColumns.DbContext.TransferType<T>(daReader);
                    }
                }
            }
        }
    }
}
