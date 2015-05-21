using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Job.Framework.Config
{
    /// <summary>
    /// 表示配置文件中集合节点
    /// </summary>
    public class AppConfigElementCollection<T> : ConfigurationElementCollection where T : ConfigurationElement, new()
    {
        /// <summary>
        /// 指定子节点名称
        /// </summary>
        protected override string ElementName
        {
            get { return "add"; }
        }

        /// <summary>
        /// 如果子节点名称需要自定义，则应重写此属性（同时还要重写 ElementName 属性）并返回 BasicMap ，否则子节点名称只能为 add
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        /// <summary>
        /// 必须实现的虚方法，获得一个空节点
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new T();
        }

        /// <summary>
        /// 必须实现的虚方法，节点的Key
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as dynamic).Name;
        }

        /// <summary>
        /// 实现自定义索引器
        /// </summary>
        /// <param name="index">集合中的位置</param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                return BaseGet(index) as T;
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }

                BaseAdd(index, value);
            }
        }

        /// <summary>
        /// 实现自定义索引器
        /// </summary>
        /// <param name="name">节点的Key</param>
        /// <returns></returns>
        public new T this[string name]
        {
            get
            {
                return BaseGet(name) as T;
            }
        }
    }
}
