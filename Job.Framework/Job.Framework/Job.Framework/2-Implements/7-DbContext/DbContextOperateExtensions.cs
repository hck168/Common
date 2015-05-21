using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Job.Framework
{
    public static class DbContextOperateExtensions
    {
        public static DbInsert Insert(this DbContext dbContext, string tableName, bool isAutoIncrement = true)
        {
            return new DbInsert
            (
                dbContext: dbContext,
                tableName: tableName,
                isAutoIncrement: isAutoIncrement
            );
        }

        public static DbDelete Delete(this DbContext dbContext, string tableName)
        {
            return new DbDelete
            (
                dbContext: dbContext,
                tableName: tableName
            );
        }

        public static DbUpdate Update(this DbContext dbContext, string tableName)
        {
            return new DbUpdate
            (
                dbContext: dbContext,
                tableName: tableName
            );
        }

        public static DbUpdateSet Set(this DbUpdate dbUpdate, object columnData)
        {
            return new DbUpdateSet
            (
                dbContext: dbUpdate.DbContext,
                tableName: dbUpdate.TableName,
                columnData: columnData
            );
        }

        public static DbSelect<dynamic> Select(this DbContext dbContext, string tableName, string sort = null)
        {
            return Select<dynamic>(dbContext, tableName, sort);
        }

        public static DbSelect<T> Select<T>(this DbContext dbContext, string tableName, string sort = null) where T : class,new()
        {
            return new DbSelect<T>
            (
                dbContext: dbContext,
                tableName: tableName,
                sort: sort
            );
        }

        public static DbSelectColumns<T> Columns<T>(this DbSelect<T> dbQuery, params string[] columns) where T : class,new()
        {
            return new DbSelectColumns<T>
            (
                dbContext: dbQuery.DbContext,
                tableName: dbQuery.TableName,
                sort: dbQuery.Sort,
                columns: columns
            );
        }

        public static DbQuery<dynamic> Query(this DbContext dbContext, string tableName)
        {
            return Query<dynamic>(dbContext, tableName, -99, string.Empty);
        }

        public static DbQuery<dynamic> Query(this DbContext dbContext, string tableName, int top)
        {
            return Query<dynamic>(dbContext, tableName, top, string.Empty);
        }

        public static DbQuery<dynamic> Query(this DbContext dbContext, string tableName, string sort)
        {
            return Query<dynamic>(dbContext, tableName, -99, sort);
        }

        public static DbQuery<dynamic> Query(this DbContext dbContext, string tableName, int top, string sort)
        {
            return Query<dynamic>(dbContext, tableName, top, sort);
        }

        public static DbQuery<T> Query<T>(this DbContext dbContext, string tableName) where T : class,new()
        {
            return Query<T>(dbContext, tableName, -99, string.Empty);
        }

        public static DbQuery<T> Query<T>(this DbContext dbContext, string tableName, int top) where T : class,new()
        {
            return Query<T>(dbContext, tableName, top, string.Empty);
        }

        public static DbQuery<T> Query<T>(this DbContext dbContext, string tableName, string sort) where T : class,new()
        {
            return Query<T>(dbContext, tableName, -99, sort);
        }

        public static DbQuery<T> Query<T>(this DbContext dbContext, string tableName, int top, string sort) where T : class,new()
        {
            return new DbQuery<T>
            (
                dbContext: dbContext,
                tableName: tableName,
                top: top,
                sort: sort
            );
        }

        public static DbQueryColumns<T> Columns<T>(this DbQuery<T> dbQuery, params string[] columns) where T : class,new()
        {
            return new DbQueryColumns<T>
            (
                dbContext: dbQuery.DbContext,
                tableName: dbQuery.TableName,
                top: dbQuery.Top,
                sort: dbQuery.Sort,
                columns: columns
            );
        }

        public static DbCount Count(this DbContext dbContext, string tableName)
        {
            return new DbCount
            (
                dbContext: dbContext,
                tableName: tableName
            );
        }
    }
}
