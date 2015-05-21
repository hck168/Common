using Job.Framework.Common;
using Job.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Job.Framework
{
    internal static class DbContextExtensions
    {
        /// <summary>
        /// 获取属性元数据集合
        /// </summary>
        /// <param name="dbContext">数据库访问上下文</param>
        /// <param name="obj">需要解析的匿名对象</param>
        /// <returns>返回属性元数据集合</returns>
        public static PropertyInfo[] GetProperties(this DbContext dbContext, object obj)
        {
            return obj.GetType().GetProperties();
        }

        /// <summary>
        /// 设置属性值，高效的表达式目录树形式
        /// </summary>
        /// <param name="dbContext">数据库访问上下文</param>
        /// <param name="obj">传入的字段参数数据</param>
        /// <param name="property">需要操作的属性</param>
        /// <param name="value">赋值的属性值</param>
        public static void SetPropertyValue(this DbContext dbContext, object obj, PropertyInfo property, object value)
        {
            var setValue = Expression.Parameter(value.GetType());
            var setTarget = Expression.Parameter(obj.GetType());
            var setMethod = Expression.Call(setTarget, property.GetSetMethod(), setValue);
            var setLambda = Expression.Lambda(setMethod, new[] { setTarget, setValue });

            setLambda.Compile().DynamicInvoke(obj, value);
        }

        /// <summary>
        /// 获取属性值，高效的表达式目录树形式
        /// </summary>
        /// <param name="dbContext">数据库访问上下文</param>
        /// <param name="obj">传入的字段参数数据</param>
        /// <param name="property">需要操作的属性</param>
        /// <returns>返回属性值</returns>
        public static object GetPropertyValue(this DbContext dbContext, object obj, PropertyInfo property)
        {
            var getTarget = Expression.Parameter(obj.GetType());
            var getMethod = Expression.Call(getTarget, property.GetGetMethod());
            var getLambda = Expression.Lambda(getMethod, new[] { getTarget });

            return getLambda.Compile().DynamicInvoke(obj);
        }

        /// <summary>
        /// 对传入的参数进行类型转换
        /// </summary>
        /// <param name="dbContext">数据库访问上下文</param>
        /// <param name="obj">传入的字段参数数据</param>
        /// <param name="pre">传入的字段加入参数别名</param>
        /// <returns>返回数据库执行的参数集合</returns>
        public static DbParameter[] GetParameters(this DbContext dbContext, object obj, string pre = null)
        {
            var properties = dbContext.GetProperties(obj);

            var dbParameters = new DbParameter[properties.Length];

            for (var i = 0; i < properties.Length; i++)
            {
                dbParameters[i] = dbContext.ProviderFactory.CreateParameter();
                dbParameters[i].ParameterName = dbContext.GetParameterName(pre + properties[i].Name);
                dbParameters[i].Value = dbContext.GetPropertyValue(obj, properties[i]);
                dbParameters[i].Direction = ParameterDirection.Input;
            }

            return dbParameters;
        }

        /// <summary>
        /// 数据流转换为动态模型
        /// </summary>
        /// <param name="dbContext">数据库访问上下文</param>
        /// <param name="dr">一个只进的结果集流</param>
        /// <returns>返回动态模型对象</returns>
        public static dynamic TransferType(this DbContext dbContext, IDataReader dr)
        {
            var d = new ExpandoObject() as IDictionary<string, object>;

            for (var i = 0; i < dr.FieldCount; i++)
            {
                d.Add(dr.GetName(i), dr.GetValue(i));
            }

            return d;
        }

        /// <summary>
        /// 数据流转换为实体模型
        /// </summary>
        /// <typeparam name="T">实体模型对象</typeparam>
        /// <param name="dbContext">数据库访问上下文</param>
        /// <param name="dr">一个只进的结果集流</param>
        /// <returns>返回实体模型对象</returns>
        public static T TransferType<T>(this DbContext dbContext, IDataReader dr) where T : class,new()
        {
            var model = new T();

            var fieldNames = dbContext.GetFieldNames(dr);

            foreach (var property in dbContext.GetProperties(model))
            {
                if (fieldNames.Contains(property.Name.ToUpper()))    //判断字段名是否相等
                {
                    dbContext.SetPropertyValue(model, property, ConvertHelper.ChangeType(dr[property.Name], property.PropertyType));
                }
            }

            return model;
        }

        /// <summary>
        /// 获取数据流字段名称
        /// </summary>
        /// <param name="dr">一个只进的结果集流</param>
        /// <returns>返回数据流字段集合</returns>
        public static IEnumerable<string> GetFieldNames(this DbContext dbContext, IDataReader dr)
        {
            for (var i = 0; i < dr.FieldCount; i++)
            {
                yield return dr.GetName(i).ToUpper();
            }
        }

        /// <summary>
        /// 获取参数形式名称（前缀 + 名称）
        /// </summary>
        /// <param name="dbContext">数据库访问上下文</param>
        /// <param name="parameterName">参数名称</param>
        /// <returns>返回对应数据库的参数形式名称</returns>
        public static string GetParameterName(this DbContext dbContext, string parameterName)
        {
            switch (dbContext.ProviderName)
            {
                case "System.Data.SqlClient":
                case "System.Data.SQLite":
                    {
                        return string.Concat("@", parameterName);
                    }
                case "System.Data.OracleClient":
                    {
                        return string.Concat(":", parameterName);
                    }
                default:
                    {
                        throw new Exception("默认配置文件中未指定提供程序名称属性，关键字：ProviderName");
                    }
            }
        }
    }
}
