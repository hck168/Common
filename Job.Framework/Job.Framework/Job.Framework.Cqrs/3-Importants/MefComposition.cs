using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;

namespace Job.Framework.Cqrs
{
    /// <summary>
    /// 定义用于组合容器的通用类
    /// </summary>
    public static class MefComposition
    {
        #region 相关属性

        /// <summary>
        /// 获取需要进行组合的DLL集合
        /// </summary>
        public static IEnumerable<string> Catalogs { get; private set; }

        /// <summary>
        /// 获取注入的对象组合容器
        /// </summary>
        public static CompositionContainer Container { get; private set; }

        #endregion

        #region 相关方法

        /// <summary>
        /// 注册组合容器
        /// </summary>
        /// <param name="catalogs">需要进行组合的DLL集合</param>
        public static void SetCatalogs(IEnumerable<string> catalogs)
        {
            var catalog = new AggregateCatalog();

            foreach (var item in MefComposition.Catalogs = catalogs)
            {
                catalog.Catalogs.Add(new DirectoryCatalog(AppDomain.CurrentDomain.SetupInformation.PrivateBinPath, item));
            }

            MefComposition.Container = new CompositionContainer(catalog);
        }

        #endregion
    }
}
