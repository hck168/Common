using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Job.Framework.Config
{
    /// <summary>
    /// 应用程序配置管理类
    /// </summary>
    public static class AppConfigManager
    {
        /// <summary>
        /// 默认配置组节点
        /// </summary>
        private const string JobConfigGroupKey = "job.config/";

        #region 系统默认

        /// <summary>
        /// 获取当前应用程序默认配置的 System.Configuration.AppSettingsSection 数据
        /// </summary>
        public static NameValueCollection AppSettings
        {
            get
            {
                return ConfigurationManager.AppSettings;
            }
        }

        #endregion

        #region 扩展配置

        #region 连接配置

        /// <summary>
        /// 获取当前应用程序扩展配置的 Job.Framework.Config.ConnectionStringsSection 数据
        /// </summary>
        public static ConnectionStringsSection ConnectionSection
        {
            get
            {
                return ConfigurationManager.GetSection(string.Concat(JobConfigGroupKey, "dbConnectionString")) as ConnectionStringsSection;
            }
        }

        /// <summary>
        /// 获取当前应用程序扩展配置的 Job.Framework.Config.ConnectionStringsCollection 数据
        /// </summary>
        public static AppConfigElementCollection<ConnectionStringsElement> ConnectionStrings
        {
            get
            {
                return ConnectionSection != null ? ConnectionSection.ConnectionStrings : null;
            }
        }

        /// <summary>
        /// 获取当前应用程序扩展配置的 Job.Framework.Config.ConnectionStringsElement 数据
        /// </summary>
        public static ConnectionStringsElement ConnectionElement
        {
            get
            {
                return ConnectionStrings != null ? ConnectionStrings[ConnectionSection.ReadDb] : null;
            }
        }

        #endregion

        #region 短信配置

        /// <summary>
        /// 获取当前应用程序扩展配置的 Job.Framework.Config.SmsConfigStringsSection 数据
        /// </summary>
        public static SmsConfigStringsSection SmsConfigSection
        {
            get
            {
                return ConfigurationManager.GetSection(string.Concat(JobConfigGroupKey, "smsConfigString")) as SmsConfigStringsSection;
            }
        }

        /// <summary>
        /// 获取当前应用程序扩展配置的 Job.Framework.Config.SmsConfigStringsCollection 数据
        /// </summary>
        public static AppConfigElementCollection<SmsConfigStringsElement> SmsConfigStrings
        {
            get
            {
                return SmsConfigSection != null ? SmsConfigSection.SmsConfigStrings : null;
            }
        }

        /// <summary>
        /// 获取当前应用程序扩展配置的 Job.Framework.Config.SmsConfigStringsElement 数据
        /// </summary>
        public static SmsConfigStringsElement SmsConfigElement
        {
            get
            {
                return SmsConfigStrings != null ? SmsConfigStrings[SmsConfigSection.DefaultName] : null;
            }
        }

        #endregion

        #region 邮箱配置

        /// <summary>
        /// 获取当前应用程序扩展配置的 Job.Framework.Config.EmailConfigStringsSection 数据
        /// </summary>
        public static EmailConfigStringsSection EmailConfigSection
        {
            get
            {
                return ConfigurationManager.GetSection(string.Concat(JobConfigGroupKey, "emailConfigString")) as EmailConfigStringsSection;
            }
        }

        /// <summary>
        /// 获取当前应用程序扩展配置的 Job.Framework.Config.EmailConfigStringsCollection 数据
        /// </summary>
        public static AppConfigElementCollection<EmailConfigStringsElement> EmailConfigStrings
        {
            get
            {
                return EmailConfigSection != null ? EmailConfigSection.EmailConfigStrings : null;
            }
        }

        /// <summary>
        /// 获取当前应用程序扩展配置的 Job.Framework.Config.EmailConfigStringsElement 数据
        /// </summary>
        public static EmailConfigStringsElement EmailConfigElement
        {
            get
            {
                return EmailConfigStrings != null ? EmailConfigStrings[EmailConfigSection.DefaultName] : null;
            }
        }

        #endregion

        #region 缓存配置

        /// <summary>
        /// 获取当前应用程序扩展配置的 Job.Framework.Config.MemcachedSettingsSection 数据
        /// </summary>
        public static MemcachedSettingsSection MemcachedConfigSection
        {
            get
            {
                return ConfigurationManager.GetSection(string.Concat(JobConfigGroupKey, "memcachedSetting")) as MemcachedSettingsSection;
            }
        }

        /// <summary>
        /// 获取当前应用程序扩展配置的 Job.Framework.Config.MemcachedSettingsCollection 数据
        /// </summary>
        public static AppConfigElementCollection<MemcachedSettingsElement> MemcachedSettings
        {
            get
            {
                return MemcachedConfigSection != null ? MemcachedConfigSection.MemcachedSettings : null;
            }
        }

        /// <summary>
        /// 获取当前应用程序扩展配置的 Job.Framework.Config.MemcachedSettingsElement 数据
        /// </summary>
        public static MemcachedSettingsElement MemcachedElement
        {
            get
            {
                return MemcachedSettings != null ? MemcachedSettings[MemcachedConfigSection.DefaultName] : null;
            }
        }

        #endregion

        #region 单点配置

        /// <summary>
        /// 获取当前应用程序扩展配置的 Job.Framework.Config.PassportSettingsSection 数据
        /// </summary>
        public static PassportSettingsSection PassportConfigSection
        {
            get
            {
                return ConfigurationManager.GetSection(string.Concat(JobConfigGroupKey, "passportSetting")) as PassportSettingsSection;
            }
        }

        /// <summary>
        /// 获取当前应用程序扩展配置的 Job.Framework.Config.PassportSettingsCollection 数据
        /// </summary>
        public static AppConfigElementCollection<PassportSettingsElement> PassportSettings
        {
            get
            {
                return PassportConfigSection != null ? PassportConfigSection.PassportSettings : null;
            }
        }

        #endregion

        #region 队列配置

        /// <summary>
        /// 获取当前应用程序扩展配置的 Job.Framework.Config.QueueSettingsSection 数据
        /// </summary>
        public static QueueSettingsSection QueueSettingSection
        {
            get
            {
                return ConfigurationManager.GetSection(string.Concat(JobConfigGroupKey, "queueSetting")) as QueueSettingsSection;
            }
        }

        /// <summary>
        /// 获取当前应用程序扩展配置的 Job.Framework.Config.QueueSettingsCollection 数据
        /// </summary>
        public static AppConfigElementCollection<QueueSettingsElement> QueueSettings
        {
            get
            {
                return QueueSettingSection != null ? QueueSettingSection.QueueSettings : null;
            }
        }

        /// <summary>
        /// 获取当前应用程序扩展配置的 Job.Framework.Config.QueueSettingsElement 数据
        /// </summary>
        public static QueueSettingsElement QueueSettingElement
        {
            get
            {
                return QueueSettings != null ? QueueSettings[EmailConfigSection.DefaultName] : null;
            }
        }

        #endregion

        #endregion
    }
}
