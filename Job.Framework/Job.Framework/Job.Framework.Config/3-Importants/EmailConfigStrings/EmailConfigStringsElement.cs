using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Job.Framework.Config
{
    /// <summary>
    /// 表示配置文件中单个节点
    /// </summary>
    public class EmailConfigStringsElement : ConfigurationElement
    {
        /// <summary>
        /// 获取或设置服务器组
        /// </summary>
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return this["name"] as string; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// 获取或设置用于 SMTP 事务的主机的名称或 IP 地址
        /// </summary>
        [ConfigurationProperty("host", IsRequired = true)]
        public string Host
        {
            get { return this["host"] as string; }
            set { this["host"] = value; }
        }

        /// <summary>
        /// 获取或设置与凭据关联的用户名
        /// </summary>
        [ConfigurationProperty("username", IsRequired = true)]
        public string UserName
        {
            get { return this["username"] as string; }
            set { this["username"] = value; }
        }

        /// <summary>
        /// 获取或设置与凭据关联的用户名的密码
        /// </summary>
        [ConfigurationProperty("password", IsRequired = true)]
        public string PassWord
        {
            get { return this["password"] as string; }
            set { this["password"] = value; }
        }

        /// <summary>
        /// 获取由创建此实例时指定的显示名和地址信息构成的显示名
        /// </summary>
        [ConfigurationProperty("displayname", IsRequired = true)]
        public string DisPlayName
        {
            get { return this["displayname"] as string; }
            set { this["displayname"] = value; }
        }

        /// <summary>
        /// 获取或设置用于 SMTP 事务的端口
        /// </summary>
        [ConfigurationProperty("port", IsRequired = true)]
        public int Port
        {
            get { return Convert.ToInt32(this["port"]); }
            set { this["port"] = value; }
        }

        /// <summary>
        /// 指定 System.Net.Mail.SmtpClient 是否使用安全套接字层 (SSL) 加密连接
        /// </summary>
        [ConfigurationProperty("enablessl", IsRequired = true)]
        public bool EnableSsl
        {
            get { return Convert.ToBoolean(this["enablessl"]); }
            set { this["enablessl"] = value; }
        }
    }
}
