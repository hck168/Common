using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Job.Framework
{
    public static class DbDeleteExtensions
    {
        public static DbContextResult Where(this DbDelete dbDelete, object conditionData)
        {
            return Where(dbDelete, false, conditionData);
        }

        public static DbContextResult Where(this DbDelete dbDelete, string conditionFormat, object conditionData)
        {
            var condition = "1 = 1";

            var dbParameters = conditionData != null ? dbDelete.DbContext.GetParameters(conditionData) : null;

            if (!string.IsNullOrWhiteSpace(conditionFormat))
            {
                condition = conditionFormat;
            }

            return dbDelete.Execute(condition, dbParameters);
        }

        public static DbContextResult Where(this DbDelete dbDelete, bool isOr, object conditionData)
        {
            var condition = "1 = 1";

            var dbParameters = conditionData != null ? dbDelete.DbContext.GetParameters(conditionData) : null;

            if (dbParameters != null && dbParameters.Any())
            {
                condition = string.Join(isOr ? " OR " : " AND ", dbParameters.Select(p => string.Format("{0} = {1}", p.ParameterName.TrimStart('@'), p.ParameterName)));
            }

            return dbDelete.Execute(condition, dbParameters);
        }

        private static DbContextResult Execute(this DbDelete dbDelete, string condition, DbParameter[] dbParameters)
        {
            var sql = string.Format(@"DELETE FROM {0}", dbDelete.TableName);

            if (!string.IsNullOrWhiteSpace(condition))
            {
                sql += string.Format(" WHERE {0}", condition);
            }

            return new DbContextResult
            (
                state: DbOperateState.Success,
                value: dbDelete.DbContext.ExecuteNonQuery(sql, dbParameters)
            );
        }
    }
}
