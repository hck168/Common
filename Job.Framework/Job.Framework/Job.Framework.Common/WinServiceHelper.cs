using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ServiceProcess;
using System.Configuration.Install;
using System.Threading;
using Microsoft.Win32;

namespace Job.Framework.Common
{
    /// <summary>
    /// Windows Service 操作类
    /// </summary>
    public sealed class WinServiceHelper
    {
        /// <summary>   
        /// 安装服务（安装完就启动，已经存在的服务直接启动）   
        /// </summary>   
        /// <param name="serviceName">服务名称</param>   
        /// <param name="servicePath">服务安装程序完整路径</param>   
        /// <param name="args">启动服务参数</param>   
        public static void InstallService(string serviceName, string servicePath, string[] args = null)
        {
            var mySavedState = new Hashtable();

            try
            {
                //服务已经存在则卸载   
                if (ServiceIsExisted(serviceName))
                {
                    UnInstallService(serviceName, servicePath);
                }

                //注册服务
                var myAssemblyInstaller = new AssemblyInstaller
                {
                    UseNewContext = true,
                    Path = servicePath
                };

                myAssemblyInstaller.Install(mySavedState);
                myAssemblyInstaller.Commit(mySavedState);
                myAssemblyInstaller.Dispose();

                StartService(serviceName, args);
            }
            catch (Exception ex)
            {
                throw new Exception("安装服务时出错：" + ex.Message);
            }
        }

        /// <summary>   
        /// 卸载服务
        /// </summary>   
        /// <param name="serviceName">服务名称</param>   
        /// <param name="servicePath">服务安装程序完整路径</param>   
        public static void UnInstallService(string serviceName, string servicePath)
        {
            var mySavedState = new Hashtable();

            try
            {
                if (ServiceIsExisted(serviceName))
                {
                    var myAssemblyInstaller = new AssemblyInstaller
                    {
                        UseNewContext = true,
                        Path = servicePath
                    };

                    myAssemblyInstaller.Uninstall(mySavedState);
                    myAssemblyInstaller.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("卸载服务时出错：" + ex.Message);
            }
        }

        /// <summary>   
        /// 判断服务是否存在   
        /// </summary>   
        /// <param name="serviceName">服务名</param>   
        /// <returns></returns>   
        public static bool ServiceIsExisted(string serviceName)
        {
            return ServiceController.GetServices().Where(e => e.ServiceName == serviceName).Count() >= 1;
        }

        /// <summary>   
        /// 判断服务是否启动   
        /// </summary>   
        /// <returns></returns>   
        public static bool ServiceIsRunning(string serviceName)
        {
            if (!ServiceIsExisted(serviceName))
            {
                return false;
            }

            return new ServiceController(serviceName).Status == ServiceControllerStatus.Running;
        }

        /// <summary>   
        /// 启动服务（启动存在的服务，30秒后启动失败报错）   
        /// </summary>   
        /// <param name="serviceName">服务名</param>
        /// <param name="args">服务参数</param>   
        public static void StartService(string serviceName, string[] args = null)
        {
            if (ServiceIsExisted(serviceName))
            {
                var service = new ServiceController(serviceName);

                if (service.Status != ServiceControllerStatus.Running && service.Status != ServiceControllerStatus.StartPending)
                {
                    service.Start(args ?? new string[0]);

                    for (int i = 1; i <= 30; i++)
                    {
                        service.Refresh();

                        Thread.Sleep(1000);

                        if (service.Status == ServiceControllerStatus.Running)
                        {
                            break;
                        }

                        if (i == 30)
                        {
                            throw new Exception("服务" + serviceName + "启动失败！");
                        }
                    }
                }
            }
        }

        /// <summary>   
        /// 启动服务（启动存在的服务，30秒后启动失败报错）   
        /// </summary>   
        /// <param name="serviceName">服务名</param>
        /// <param name="args">服务参数</param>   
        public static void ReStartService(string serviceName, string[] args = null)
        {
            try
            {
                if (ServiceIsExisted(serviceName))
                {
                    StopService(serviceName);
                    StartService(serviceName, args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("服务重启时出错：" + ex.Message);
            }
        }

        /// <summary>   
        /// 停止服务（停止存在的服务，30秒后停止失败报错）
        /// </summary>   
        /// <param name="serviceName"></param>   
        public static void StopService(string serviceName)
        {
            if (ServiceIsExisted(serviceName))
            {
                var service = new ServiceController(serviceName);

                if (service.Status == ServiceControllerStatus.Running)
                {
                    service.Stop();

                    for (int i = 1; i <= 30; i++)
                    {
                        service.Refresh();

                        Thread.Sleep(1000);

                        if (service.Status == ServiceControllerStatus.Stopped)
                        {
                            break;
                        }

                        if (i == 30)
                        {
                            throw new Exception("服务" + serviceName + "停止失败！");
                        }
                    }
                }
            }
        }

        /// <summary>   
        /// 执行服务指令
        /// </summary>   
        /// <param name="serviceName"></param>   
        /// <param name="commond"></param>   
        public static void ExecuteServiceCommand(string serviceName, int commond)
        {
            try
            {
                if (ServiceIsExisted(serviceName))
                {
                    var service = new ServiceController(serviceName);

                    if (service.Status == ServiceControllerStatus.Running)
                    {
                        service.ExecuteCommand(commond);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("执行服务指令时出错：" + ex.Message);
            }
        }

        /// <summary>   
        /// 修改服务的启动项（2：自动，3：手动，4：禁用）
        /// </summary>   
        /// <param name="serviceName"></param> 
        /// <param name="startType"></param>  
        /// <returns></returns>   
        public static void ChangeServiceStartType(string serviceName, int startType)
        {
            try
            {
                var regKey = Registry.LocalMachine.OpenSubKey("SYSTEM")
                                                  .OpenSubKey("CurrentControlSet")
                                                  .OpenSubKey("Services")
                                                  .OpenSubKey(serviceName, true);

                regKey.SetValue("Start", startType);
                regKey.Flush();
                regKey.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("修改服务启动项时出错：" + ex.Message);
            }
        }

        /// <summary>   
        /// 获取服务启动类型（2：自动，3：手动，4：禁用）
        /// </summary>   
        /// <param name="serviceName"></param>   
        /// <returns></returns>   
        public static string GetServiceStartType(string serviceName)
        {
            try
            {
                var regKey = Registry.LocalMachine.OpenSubKey("SYSTEM")
                                                  .OpenSubKey("CurrentControlSet")
                                                  .OpenSubKey("Services")
                                                  .OpenSubKey(serviceName, true);

                return regKey.GetValue("Start") as string;
            }
            catch (Exception ex)
            {
                throw new Exception("获取服务启动项时出错：" + ex.Message);
            }
        }

        /// <summary>
        /// 给服务添加命令行参数
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static void AddCommandLineArgs(string serviceName, string[] args)
        {
            try
            {
                if (args == null)
                {
                    throw new NullReferenceException("未设置命令行参数，请检查确认");
                }

                var regKey = Registry.LocalMachine.OpenSubKey("SYSTEM")
                                                  .OpenSubKey("CurrentControlSet")
                                                  .OpenSubKey("Services")
                                                  .OpenSubKey(serviceName, true);

                var path = regKey.GetValue("ImagePath") as string;

                if (string.IsNullOrEmpty(path))
                {
                    throw new NullReferenceException("服务路径异常，请检查确认");
                }

                foreach (var item in args)
                {
                    path = string.Format("{0} {1}", path, item);
                }

                regKey.SetValue("ImagePath", path);
                regKey.Flush();
                regKey.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("获取服务启动项时出错：" + ex.Message);
            }
        }
    }
}
