using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Job.Framework
{
    public static class DbInsertExtensions
    {
        public static DbContextResult Values(this DbInsert dbInsert, params object[] columnData)
        {
            if (columnData == null || columnData.Length == 0)
            {
                throw new ArgumentNullException("需要新增记录的字段 columnData 值为空");
            }

            var sql = new StringBuilder();

            var dbParameters = new List<DbParameter>();

            for (int i = 0; i < columnData.Length; i++)
            {
                var parameters = dbInsert.DbContext.GetParameters(columnData[i], "P" + i + "_");

                if (parameters.Length == 0)
                {
                    throw new ArgumentNullException("需要新增记录的字段 columnData 未赋值");
                }

                sql.AppendFormat(@"INSERT INTO {0} ({1}) VALUES ({2});",
                                       dbInsert.TableName,
                                       string.Join(",", parameters.Select(p => p.ParameterName.Replace('@' + "P" + i + "_", string.Empty))),
                                       string.Join(",", parameters.Select(p => p.ParameterName))
                                 );

                dbParameters.AddRange(parameters);
            }

            if (dbInsert.IsAutoIncrement && columnData.Length == 1)
            {
                sql.AppendFormat("SELECT SCOPE_IDENTITY() AS ID");

                return new DbContextResult
                (
                    state: DbOperateState.Success,
                    value: dbInsert.DbContext.ExecuteScalar<long>(sql.ToString(), dbParameters.ToArray())
                );
            }
            else
            {
                return new DbContextResult
                (
                    state: DbOperateState.Success,
                    value: dbInsert.DbContext.ExecuteNonQuery(sql.ToString(), dbParameters.ToArray())
                );
            }
        }
    }
}
