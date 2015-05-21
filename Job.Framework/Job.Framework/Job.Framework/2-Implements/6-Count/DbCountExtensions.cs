using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Job.Framework
{
    public static class DbCountExtensions
    {
        public static long Where(this DbCount dbCount, object conditionData)
        {
            return Where(dbCount, false, conditionData);
        }

        public static long Where(this DbCount dbCount, string conditionFormat, object conditionData)
        {
            var condition = "1 = 1";

            var dbParameters = conditionData != null ? dbCount.DbContext.GetParameters(conditionData) : null;

            if (!string.IsNullOrWhiteSpace(conditionFormat))
            {
                condition = conditionFormat;
            }

            return dbCount.Execute(condition, dbParameters);
        }

        public static long Where(this DbCount dbCount, bool isOr, object conditionData)
        {
            var condition = "1 = 1";

            var dbParameters = conditionData != null ? dbCount.DbContext.GetParameters(conditionData) : null;

            if (dbParameters != null && dbParameters.Any())
            {
                condition = string.Join(isOr ? " OR " : " AND ", dbParameters.Select(p => string.Format("{0} = {1}", p.ParameterName.TrimStart('@'), p.ParameterName)));
            }

            return dbCount.Execute(condition, dbParameters);
        }

        private static long Execute(this DbCount dbCount, string condition, DbParameter[] dbParameters)
        {
            var sql = string.Format(@"SELECT COUNT(1) FROM {0}", dbCount.TableName);

            if (!string.IsNullOrWhiteSpace(condition))
            {
                sql += string.Format(" WHERE {0}", condition);
            }

            return dbCount.DbContext.ExecuteScalar<long>(sql, dbParameters);
        }
    }
}
