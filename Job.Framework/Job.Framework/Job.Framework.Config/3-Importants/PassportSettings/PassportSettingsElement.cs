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
    public class PassportSettingsElement : ConfigurationElement
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
        /// 获取或设置跨域登录地址
        /// </summary>
        [ConfigurationProperty("loginUrl", IsRequired = true)]
        public string LoginUrl
        {
            get { return this["loginUrl"] as string; }
            set { this["loginUrl"] = value; }
        }

        /// <summary>
        /// 获取或设置跨域注销地址
        /// </summary>
        [ConfigurationProperty("logoutUrl", IsRequired = true)]
        public string LogoutUrl
        {
            get { return this["logoutUrl"] as string; }
            set { this["logoutUrl"] = value; }
        }
    }
}
