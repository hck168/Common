using System;
using System.Data;
using System.Collections.Generic;
using System.Dynamic;

namespace Job.Framework.Common
{
    public sealed class EnumHelper
    {
        /// <summary>
        /// 列出指定类型枚举的方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<dynamic> ListEnum<T>() where T : class
        {
            var d = new ExpandoObject() as IDictionary<string, object>;

            foreach (var item in Enum.GetValues(typeof(T)))
            {
                d.Add(item.ToString(), Convert.ToInt32(Enum.Parse(typeof(T), item.ToString())));

                yield return d;
            }
        }

        /// <summary>
        /// 枚举值比较
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static bool Compare(int value1, Enum value2)
        {
            return value1 == ConvertHelper.ChangeType<int>(value2);
        }

        /// <summary>
        /// 枚举值比较
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static bool Compare(string value1, Enum value2)
        {
            return value1 == ConvertHelper.ChangeType<string>(value2);
        }

        /// <summary>
        /// 枚举值转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Parse<T>(object value)
        {
            return ConvertHelper.ChangeType<T>(value);
        }
    }
}
