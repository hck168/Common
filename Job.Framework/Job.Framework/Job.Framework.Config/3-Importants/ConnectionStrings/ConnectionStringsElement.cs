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
    public class ConnectionStringsElement : ConfigurationElement
    {
        /// <summary>
        /// 获取或设置连接字符串名称
        /// </summary>
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return this["name"] as string; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// 获取或设置连接字符串
        /// </summary>
        [ConfigurationProperty("connectionString", IsRequired = true)]
        public string ConnectionString
        {
            get { return this["connectionString"] as string; }
            set { this["connectionString"] = value; }
        }

        /// <summary>
        /// 获取或设置提供程序名称属性
        /// </summary>
        [ConfigurationProperty("providerName", DefaultValue = "System.Data.SqlClient")]
        public string ProviderName
        {
            get { return this["providerName"] as string; }
            set { this["providerName"] = value; }
        }
    }
}
