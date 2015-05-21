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
    public class SmsConfigStringsElement : ConfigurationElement
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
        /// 获取或设置参数1
        /// </summary>
        [ConfigurationProperty("arg1")]
        public string Arg1
        {
            get { return this["arg1"] as string; }
            set { this["arg1"] = value; }
        }

        /// <summary>
        /// 获取或设置参数2
        /// </summary>
        [ConfigurationProperty("arg2")]
        public string Arg2
        {
            get { return this["arg2"] as string; }
            set { this["arg2"] = value; }
        }

        /// <summary>
        /// 获取或设置发参数3
        /// </summary>
        [ConfigurationProperty("arg3")]
        public string Arg3
        {
            get { return this["arg3"] as string; }
            set { this["arg3"] = value; }
        }

        /// <summary>
        /// 获取或设置发参数4
        /// </summary>
        [ConfigurationProperty("arg4")]
        public string Arg4
        {
            get { return this["arg4"] as string; }
            set { this["arg4"] = value; }
        }

        /// <summary>
        /// 获取或设置发参数5
        /// </summary>
        [ConfigurationProperty("arg5")]
        public string Arg5
        {
            get { return this["arg5"] as string; }
            set { this["arg5"] = value; }
        }

        /// <summary>
        /// 获取或设置发参数6
        /// </summary>
        [ConfigurationProperty("arg6")]
        public string Arg6
        {
            get { return this["arg6"] as string; }
            set { this["arg6"] = value; }
        }
    }
}
