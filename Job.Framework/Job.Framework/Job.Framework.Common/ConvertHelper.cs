using System;
using System.Text;
using System.Threading;

namespace Job.Framework.Common
{
    public sealed class ConvertHelper
    {
        #region 类型转换

        /// <summary>
        /// 类型转换帮助类，支持枚举的转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ChangeType<T>(object value)
        {
            return (T)ConvertHelper.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// 自定义类型转换，支持枚举的转换
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        public static object ChangeType(object obj, Type conversionType)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return conversionType.IsValueType ? Activator.CreateInstance(conversionType) : null;
            }

            var provider = Thread.CurrentThread.CurrentCulture;

            var nullableType = Nullable.GetUnderlyingType(conversionType);

            if (nullableType != null)
            {
                return Convert.ChangeType(obj, nullableType, provider);
            }

            if (typeof(Enum).IsAssignableFrom(conversionType))
            {
                return Enum.Parse(conversionType, obj.ToString());
            }

            if (conversionType == typeof(Guid))
            {
                return new Guid(obj.ToString());
            }
            if (conversionType == typeof(Version))
            {
                return new Version(obj.ToString());
            }

            return Convert.ChangeType(obj, conversionType, provider);
        }

        #endregion

        #region 字节转换

        /// <summary>
        /// 以字节数组的形式返回指定的字符串
        /// </summary>
        /// <param name="value">要转换的字符串</param>
        /// <returns>返回字节数组</returns>
        public static byte[] GetBytes(string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        /// <summary>
        /// 返回由字节数组中指定位置的字符串
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">value 内的起始位置</param>
        /// <returns>返回字符串</returns>
        public static string ToString(byte[] value, int startIndex = 0)
        {
            return Encoding.UTF8.GetString(value, startIndex, value.Length);
        }

        /// <summary>
        /// 以字节数组的形式返回指定的 32 位有符号整数值
        /// </summary>
        /// <param name="value">要转换的数字</param>
        /// <returns>长度为 4 的字节数组</returns>
        public static byte[] GetBytes(int value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// 返回由字节数组中指定位置的四个字节转换来的 32 位有符号整数
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">value 内的起始位置</param>
        /// <returns>由四个字节构成、从 startIndex 开始的 32 位有符号整数</returns>
        public static int ToInt32(byte[] value, int startIndex = 0)
        {
            return BitConverter.ToInt32(value, startIndex);
        }

        /// <summary>
        /// 以字节数组的形式返回指定的双精度浮点值
        /// </summary>
        /// <param name="value">要转换的数字</param>
        /// <returns>长度为 8 的字节数组</returns>
        public static byte[] GetBytes(double value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// 返回由字节数组中指定位置的八个字节转换来的双精度浮点数
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">value 内的起始位置</param>
        /// <returns>由八个字节构成、从 startIndex 开始的双精度浮点数</returns>
        public static double ToDouble(byte[] value, int startIndex = 0)
        {
            return BitConverter.ToDouble(value, startIndex);
        }

        /// <summary>
        /// 以字节数组的形式返回指定的布尔值
        /// </summary>
        /// <param name="value">一个布尔值</param>
        /// <returns>长度为 1 的字节数组</returns>
        public static byte[] GetBytes(bool value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// 返回由字节数组中指定位置的一个字节转换来的布尔值
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">value 内的起始位置</param>
        /// <returns>如果 value 中的 startIndex 处的字节非零，则为 true；否则为 false</returns>
        public static bool ToBoolean(byte[] value, int startIndex = 0)
        {
            return BitConverter.ToBoolean(value, startIndex);
        }

        /// <summary>
        /// 以字节数组的形式返回指定的 Unicode 字符值
        /// </summary>
        /// <param name="value">要转换的字符</param>
        /// <returns>长度为 2 的字节数组</returns>
        public static byte[] GetBytes(char value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// 返回由字节数组中指定位置的两个字节转换来的 Unicode 字符
        /// </summary>
        /// <param name="value">一个数组</param>
        /// <param name="startIndex">value 内的起始位置</param>
        /// <returns>由两个字节构成、从 startIndex 开始的字符</returns>
        public static char ToChar(byte[] value, int startIndex = 0)
        {
            return BitConverter.ToChar(value, startIndex);
        }

        /// <summary>
        /// 以字节数组的形式返回指定的单精度浮点值
        /// </summary>
        /// <param name="value">要转换的数字</param>
        /// <returns>长度为 4 的字节数组</returns>
        public static byte[] GetBytes(float value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// 返回由字节数组中指定位置的四个字节转换来的单精度浮点数
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">value 内的起始位置</param>
        /// <returns>由四个字节构成、从 startIndex 开始的单精度浮点数</returns>
        public static float ToSingle(byte[] value, int startIndex = 0)
        {
            return BitConverter.ToSingle(value, startIndex);
        }

        /// <summary>
        /// 以字节数组的形式返回指定的 64 位有符号整数值
        /// </summary>
        /// <param name="value">要转换的数字</param>
        /// <returns>长度为 8 的字节数组</returns>
        public static byte[] GetBytes(long value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// 返回由字节数组中指定位置的八个字节转换来的 64 位有符号整数
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">value 内的起始位置</param>
        /// <returns>由八个字节构成、从 startIndex 开始的 64 位有符号整数</returns>
        public static long ToInt64(byte[] value, int startIndex = 0)
        {
            return BitConverter.ToInt64(value, startIndex);
        }

        /// <summary>
        /// 以字节数组的形式返回指定的 16 位有符号整数值
        /// </summary>
        /// <param name="value">要转换的数字</param>
        /// <returns>长度为 2 的字节数组</returns>
        public static byte[] GetBytes(short value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// 以字节数组的形式返回指定的 16 位有符号整数值
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">value 内的起始位置</param>
        /// <returns>由两个字节构成、从 startIndex 开始的 16 位有符号整数</returns>
        public static short ToInt16(byte[] value, int startIndex = 0)
        {
            return BitConverter.ToInt16(value, startIndex);
        }

        /// <summary>
        /// 以字节数组的形式返回指定的 16 位无符号整数值
        /// </summary>
        /// <param name="value">要转换的数字</param>
        /// <returns>长度为 2 的字节数组</returns>
        public static byte[] GetBytes(ushort value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// 返回由字节数组中指定位置的两个字节转换来的 16 位无符号整数
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">value 内的起始位置</param>
        /// <returns>由两个字节构成、从 startIndex 开始的 16 位无符号整数</returns>
        public static ushort ToUInt16(byte[] value, int startIndex = 0)
        {
            return BitConverter.ToUInt16(value, startIndex);
        }

        /// <summary>
        /// 以字节数组的形式返回指定的 32 位无符号整数值
        /// </summary>
        /// <param name="value">要转换的数字</param>
        /// <returns>长度为 4 的字节数组</returns>
        public static byte[] GetBytes(uint value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// 返回由字节数组中指定位置的四个字节转换来的 32 位无符号整数
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">value 内的起始位置</param>
        /// <returns>由四个字节构成、从 startIndex 开始的 32 位无符号整数</returns>
        public static uint ToUInt32(byte[] value, int startIndex = 0)
        {
            return BitConverter.ToUInt32(value, startIndex);
        }

        /// <summary>
        /// 以字节数组的形式返回指定的 64 位无符号整数值
        /// </summary>
        /// <param name="value">要转换的数字</param>
        /// <returns>长度为 8 的字节数组</returns>
        public static byte[] GetBytes(ulong value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// 返回由字节数组中指定位置的八个字节转换来的 64 位无符号整数
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">value 内的起始位置</param>
        /// <returns>由八个字节构成、从 startIndex 开始的 64 位无符号整数</returns>
        public static ulong ToUInt64(byte[] value, int startIndex = 0)
        {
            return BitConverter.ToUInt64(value, startIndex);
        }

        #endregion
    }
}
