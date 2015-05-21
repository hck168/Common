using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Web;

namespace Job.Framework.Common
{
    /// <summary>
    /// 通用工具帮助类
    /// </summary>
    public static class GlobalHelper
    {
        #region 对象序列化

        public static byte[] Serialize(object graph)
        {
            using (var stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, graph);

                return stream.GetBuffer();
            }
        }

        public static T Deserialize<T>(byte[] buffer) where T : class
        {
            return Deserialize(buffer) as T;
        }

        public static object Deserialize(byte[] buffer)
        {
            using (var stream = new MemoryStream(buffer))
            {
                return new BinaryFormatter().Deserialize(stream);
            }
        }

        #endregion

        #region 对象实例化

        public static T CreateInstance<T>(Type type) where T : class
        {
            return FormatterServices.GetUninitializedObject(type) as T;
        }

        #endregion

        #region 获取配置文件值

        /// <summary>
        /// 获取配置文件中的应用设置值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="appKey"></param>
        /// <returns></returns>
        public static T GetConfig<T>(string appKey)
        {
            var configValue = ConfigurationManager.AppSettings[appKey];

            if (configValue == null)
            {
                return default(T);
            }

            return ConvertHelper.ChangeType<T>(configValue);
        }

        #endregion

        #region 装载X.509证书

        /// <summary>
        /// 装载X.509证书
        /// </summary>
        /// <param name="filePath">证书路径</param>
        /// <returns></returns>
        public static X509Certificate2 LoadCertificate(string filePath)
        {
            return LoadCertificate(filePath, null);
        }

        /// <summary>
        /// 装载X.509证书
        /// </summary>
        /// <param name="filePath">证书路径</param>
        /// <param name="passWord">证书密码</param>
        /// <returns></returns>
        public static X509Certificate2 LoadCertificate(string filePath, string passWord)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentException(string.Format("X.509证书[{0}]不存在.", filePath));
            }
            try
            {
                return new X509Certificate2(filePath, passWord, X509KeyStorageFlags.MachineKeySet);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Format("X.509证书[{0}]不能被装载 - {1}", filePath, ex.Message));
            }
        }

        #endregion

        #region 时间戳操作

        /// <summary>
        /// 时间戳转换成时间格式
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime ParseUnixTimeStamp(int timeStamp)
        {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return dt.AddSeconds(timeStamp);
        }

        /// <summary>
        /// 计算时间戳并返回结果
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int GetUnixTimeStamp(DateTime dt)
        {
            DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan span = dt - dt1970;
            return (int)(span.TotalSeconds);
        }

        #endregion

        #region 字段长度截取

        /// <summary>
        /// 截取字段（加省略号）
        /// </summary>
        /// <param name="str">内容</param>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public static string CutString(string str, int len)
        {
            return CutString(str, len, "...");
        }

        /// <summary>
        /// 截取字段（加自定义字符）
        /// </summary>
        /// <param name="str">内容</param>
        /// <param name="len">长度</param>
        /// <param name="type">省略样式</param>
        /// <returns></returns>
        public static string CutString(string str, int len, string type)
        {
            var temp = string.Empty;

            if (System.Text.Encoding.Default.GetByteCount(str) <= len)//如果长度比需要的长度n小,返回原字符串
            {
                return str;
            }
            else
            {
                int t = 0;
                char[] q = str.ToCharArray();
                for (int i = 0; i < q.Length && t < len; i++)
                {
                    if ((int)q[i] >= 0x4E00 && (int)q[i] <= 0x9FA5)//是否汉字
                    {
                        temp += q[i];
                        t += 2;
                    }
                    else
                    {
                        temp += q[i];
                        t++;
                    }
                }
                return (temp + type);
            }
        }

        #endregion

        #region IP地址操作

        /// <summary> 
        /// 取得客户端真实IP，代理IP则取第一个非内网地址
        /// </summary> 
        public static string IpAddress
        {
            get
            {
                var current = HttpContext.Current;
                //判断是否是Web情况下
                if (current != null)
                {
                    string ipAddress = current.Request.ServerVariables["REMOTE_ADDR"];

                    if (string.IsNullOrEmpty(ipAddress))
                    {
                        ipAddress = current.Request.UserHostAddress;
                    }

                    //验证是否为IP6格式，转换为IP4格式
                    if (ipAddress.Length > 15 || ipAddress == "::1")
                    {
                        //取得客户端主机 IPv4 位址(通过DNS反查)
                        string ipv4 = string.Empty;

                        foreach (IPAddress ip in Dns.GetHostAddresses(ipAddress))
                        {
                            if (ip.AddressFamily == AddressFamily.InterNetwork)
                            {
                                ipv4 = ip.ToString();
                                break;
                            }
                        }

                        if (ipv4.Length == 0)
                        {
                            foreach (IPAddress ip2 in Dns.GetHostEntry(ipAddress).AddressList)
                            {
                                if (ip2.AddressFamily == AddressFamily.InterNetwork)
                                {
                                    ipv4 = ip2.ToString();
                                    break;
                                }
                            }
                        }

                        return ipv4.Length > 0 ? ipv4 : string.Empty;
                    }
                    else
                    {
                        return ipAddress;
                    }
                }
                else
                {
                    return "127.0.0.1";
                }
            }
        }

        /// <summary>
        /// 判断是否是有效IP地址格式
        /// </summary>
        /// <param name="ipAddress">IP地址</param>
        /// <returns>true or false</returns>
        public static bool IsIPAddress(string ipAddress)
        {
            IPAddress ip;

            return IPAddress.TryParse(ipAddress, out ip);
        }

        public static IPAddress GetLocalIPV4
        {
            get
            {
                return Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork);
            }
        }

        #endregion

        #region 正则表达式操作

        /// <summary>
        /// 通用正则表达式验证方法
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        private static bool MathRegex(string input, string pattern, RegexOptions options = RegexOptions.None)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 判断是否是Guid格式
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsGuid(string input)
        {
            return GlobalHelper.MathRegex(input, @"^[0-9a-f]{8}(-[0-9a-f]{4}){3}-[0-9a-f]{12}$", RegexOptions.IgnoreCase);
        }

        // <summary>
        /// 判断是否为有效的Url地址
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsUrlAddress(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            return GlobalHelper.MathRegex(input, @"^(http|https|ftp)://[a-zA-Z0-9-.]+.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9-._?,'/\+&%$#=~])*[^.,)(s]$");
        }

        /// <summary>
        /// 判断是否为国内固话（0511 - 4405222 或 021 - 87888822）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsTelephone(string input)
        {
            return GlobalHelper.MathRegex(input, @"^\d{4}-\d{7}|\d{3}-\d{8}$");
        }

        /// <summary>
        /// 判断是否为身份证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsIDCard(string input)
        {
            return GlobalHelper.MathRegex(input, @"^(^\d{15}$)|(\d{17}(?:\d|x|X)$)$");
        }

        /// <summary>
        /// 判断是否是QQ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsQQ(string input)
        {
            return GlobalHelper.MathRegex(input, @"^[1-9]{1}[0-9]{4,8}$");
        }

        /// <summary>
        /// 判断是否为双字节字符（包括汉字在内）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsDoubleCharacter(string input)
        {
            return GlobalHelper.MathRegex(input, @"^[^\x00-\xff]$");
        }

        /// <summary>
        /// 判断是否为汉字字符
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsChineseCharacter(string input)
        {
            return GlobalHelper.MathRegex(input, @"^[\u4e00-\u9fa5]$");
        }

        /// <summary>
        /// 判断是否为邮箱地址
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEmailAddress(string input)
        {
            return GlobalHelper.MathRegex(input, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        }

        /// <summary>
        /// 判断是否为手机号码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsPhoneNumber(string input)
        {
            return GlobalHelper.MathRegex(input, @"^(1[3-9])\d{9}$");
        }

        #endregion

        #region 清除Html标签

        /// <summary>
        /// 清除Html标签
        /// </summary>
        /// <param name="htmlstr">含Html的内容</param>
        /// <returns></returns>
        public static string ClearHtml(string htmlstr)
        {
            string tmpStr = htmlstr;
            tmpStr = ReplaceHtml("&#[^>]*;", tmpStr, "");
            tmpStr = ReplaceHtml("</?marquee[^>]*>", tmpStr, "");
            tmpStr = ReplaceHtml("</?object[^>]*>", tmpStr, "");
            tmpStr = ReplaceHtml("</?param[^>]*>", tmpStr, "");
            tmpStr = ReplaceHtml("</?embed[^>]*>", tmpStr, "");
            tmpStr = ReplaceHtml("</?table[^>]*>", tmpStr, "");
            tmpStr = ReplaceHtml("&nbsp;", tmpStr, "");
            tmpStr = ReplaceHtml("</?tr[^>]*>", tmpStr, "");
            tmpStr = ReplaceHtml("</?th[^>]*>", tmpStr, "");
            tmpStr = ReplaceHtml("</?p[^>]*>", tmpStr, "");
            tmpStr = ReplaceHtml("</?a[^>]*>", tmpStr, "");
            tmpStr = ReplaceHtml("</?img[^>]*>", tmpStr, "");
            tmpStr = ReplaceHtml("</?tbody[^>]*>", tmpStr, "");
            tmpStr = ReplaceHtml("</?li[^>]*>", tmpStr, "");
            tmpStr = ReplaceHtml("</?span[^>]*>", tmpStr, "");
            tmpStr = ReplaceHtml("</?div[^>]*>", tmpStr, "");
            tmpStr = ReplaceHtml("</?th[^>]*>", tmpStr, "");
            tmpStr = ReplaceHtml("</?td[^>]*>", tmpStr, "");
            tmpStr = ReplaceHtml("</?script[^>]*>", tmpStr, "");
            tmpStr = ReplaceHtml("(javascript|jscript|vbscript|vbs):", tmpStr, "");
            tmpStr = ReplaceHtml("on(mouse|exit|error|click|key)", tmpStr, "");
            tmpStr = ReplaceHtml("<\\?xml[^>]*>", tmpStr, "");
            tmpStr = ReplaceHtml("<\\/?[a-z]+:[^>]*>", tmpStr, "");
            tmpStr = ReplaceHtml("</?font[^>]*>", tmpStr, "");
            tmpStr = ReplaceHtml("</?b[^>]*>", tmpStr, "");
            tmpStr = ReplaceHtml("</?u[^>]*>", tmpStr, "");
            tmpStr = ReplaceHtml("</?i[^>]*>", tmpStr, "");
            tmpStr = ReplaceHtml("</?h[1-7][^>]*>", tmpStr, "");
            return tmpStr;
        }

        /// <summary>
        /// 替换HTML标签
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="htmlstr"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        private static string ReplaceHtml(string pattern, string htmlstr, string replacement)
        {
            Regex rx = new Regex(pattern, RegexOptions.IgnoreCase);

            if (rx.IsMatch(htmlstr))
            {
                return rx.Replace(htmlstr, replacement);
            }

            return htmlstr;
        }

        #endregion
    }
}
