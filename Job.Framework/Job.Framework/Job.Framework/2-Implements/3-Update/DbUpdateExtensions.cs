using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Job.Framework
{
    public static class DbUpdateExtensions
    {
        public static DbContextResult Where(this DbUpdateSet dbUpdateSet, object conditionData)
        {
            return Where(dbUpdateSet, false, conditionData);
        }

        public static DbContextResult Where(this DbUpdateSet dbUpdateSet, string conditionFormat, object conditionData)
        {
            var condition = "1 = 1";

            var dbParameters = dbUpdateSet.DbContext.GetParameters(dbUpdateSet.ColumnData);

            var cdParameters = conditionData != null ? dbUpdateSet.DbContext.GetParameters(conditionData) : null;

            if (!string.IsNullOrWhiteSpace(conditionFormat))
            {
                condition = conditionFormat;
            }

            return dbUpdateSet.Execute(dbParameters, condition, cdParameters);
        }

        public static DbContextResult Where(this DbUpdateSet dbUpdateSet, bool isOr, object conditionData)
        {
            var condition = "1 = 1";

            var dbParameters = dbUpdateSet.DbContext.GetParameters(dbUpdateSet.ColumnData);

            var cdParameters = conditionData != null ? dbUpdateSet.DbContext.GetParameters(conditionData, "P_") : null;

            if (cdParameters != null && cdParameters.Any())
            {
                condition = string.Join(isOr ? " OR " : " AND ", cdParameters.Select(p => string.Format("{0} = {1}", p.ParameterName.Replace("@P_", string.Empty), p.ParameterName)));
            }

            return dbUpdateSet.Execute(dbParameters, condition, cdParameters);
        }

        private static DbContextResult Execute(this DbUpdateSet dbUpdateSet, DbParameter[] dbParameters, string condition, DbParameter[] cdParameters)
        {
            if (dbUpdateSet.ColumnData == null)
            {
                throw new ArgumentNullException("需要更新记录的字段 columnData 值不能为空");
            }

            if (dbParameters.Length == 0)
            {
                throw new ArgumentNullException("需要更新记录的字段 columnData 未赋值");
            }

            var sql = string.Format(@"UPDATE {0} SET {1}", dbUpdateSet.TableName, string.Join(",", dbParameters.Select(p => string.Format("{0} = {1}", p.ParameterName.TrimStart('@'), p.ParameterName))));

            if (!string.IsNullOrWhiteSpace(condition))
            {
                sql += string.Format(" WHERE {0}", condition);
            }

            return new DbContextResult
            (
                state: DbOperateState.Success,
                value: dbUpdateSet.DbContext.ExecuteNonQuery(sql, dbParameters.Union(cdParameters).ToArray())
            );
        }
    }
}
