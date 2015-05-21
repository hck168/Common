using Job.Framework.Common;
using Job.Framework.Config;
using Job.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;

namespace Job.Framework
{
    public sealed partial class DbContext : IDisposable
    {
        #region 相关属性

        /// <summary>
        /// 获取数据库连接提供程序
        /// </summary>
        internal string ProviderName { get; set; }

        /// <summary>
        /// 获取数据库连接名称
        /// </summary>
        internal string ConnectionName { get; set; }

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        internal string ConnectionString { get; set; }

        /// <summary>
        /// 等待命令执行的时间（以秒为单位），默认为：30
        /// </summary>
        internal int CommandTimeout { get; set; }

        /// <summary>
        /// 获取提供程序对数据库类的实现实例
        /// </summary>
        internal DbProviderFactory ProviderFactory { get; private set; }

        public TextWriter Log { get; private set; }

        #endregion

        #region 构造函数

        /// <summary>
        /// 实例化 DbContext 对象
        /// </summary>
        /// <param name="dbConnectionName">数据库连接名称</param>
        internal DbContext(string dbConnectionName = null)
        {
            var settings = this.GetSettings(dbConnectionName);

            this.CommandTimeout = 30;
            this.ConnectionName = settings.Name;
            this.ConnectionString = settings.ConnectionString;
            this.ProviderName = settings.ProviderName;
            this.ProviderFactory = DbProviderFactories.GetFactory(settings.ProviderName);
            this.Log = new StringWriter();
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 获取数据库连接配置
        /// </summary>
        /// <param name="dbConnectionName">数据库连接名称</param>
        /// <returns>返回数据库连接配置</returns>
        private ConnectionStringSettings GetSettings(string dbConnectionName)
        {
            var config = AppConfigManager.ConnectionStrings[dbConnectionName ?? AppConfigManager.ConnectionSection.ReadDb];    //如果传入的数据库连接名称为空值，则使用默认的数据库连接名称

            if (config == null)
            {
                throw new ArgumentNullException(string.Format("连接字符串节点 {0} 未被配置", dbConnectionName));
            }

            if (string.IsNullOrWhiteSpace(config.ProviderName))
            {
                throw new ArgumentNullException(string.Format("连接字符串节点 {0} 必须正确指定数据库提供者（providerName）", dbConnectionName));
            }

            if (string.IsNullOrWhiteSpace(config.ConnectionString))
            {
                throw new ArgumentNullException(string.Format("连接字符串节点 {0} 必须正确指定数据库连接字符串（connectionString）", dbConnectionName));
            }

            return new ConnectionStringSettings(config.Name, config.ConnectionString, config.ProviderName);
        }

        #endregion

        #region 相关方法

        /// <summary>
        /// 对 DbCommand 进行对象重组，可用于SQL语句或者存储过程
        /// </summary>
        /// <param name="cmdText">数据库运行的文本命令</param>
        /// <param name="cmdParms">数据库运行的参数集合</param>
        /// <param name="cmdType">指定如何解释命令字符串</param>
        /// <param name="needDisposed">是否需要释放 DbCommand 事件</param>
        /// <returns>返回 DbCommand 对象</returns>
        private DbCommand CreateDbCommand(string cmdText, DbParameter[] cmdParms, CommandType cmdType, bool needDisposed = true)
        {
            this.Log.WriteLine(cmdText);

            var cmd = ProviderFactory.CreateCommand();  //初始化 DbCommand 对象

            if (needDisposed)
            {
                cmd.Disposed += cmd_Disposed;   //释放 DbCommand 事件
            }

            try
            {
                if ((cmd.Transaction = Transaction.Get(ConnectionName)) == null)    //事务不在事务连接池中（第一次使用）或者非事务使用
                {
                    cmd.Connection = ProviderFactory.CreateConnection();
                    cmd.Connection.ConnectionString = ConnectionString;
                    cmd.Connection.Open();

                    if (Transaction.IsTransaction)  //事务开启标记正在使用
                    {
                        Transaction.Set(ConnectionName, cmd.Transaction = cmd.Connection.BeginTransaction(Transaction.IsolationLevel));     //加入事务连接池
                    }
                }
                else
                {
                    cmd.Connection = cmd.Transaction.Connection;
                }

                cmd.CommandText = cmdText;
                cmd.CommandType = cmdType;

                cmd.CommandTimeout = CommandTimeout;

                if (cmdParms != null)
                {
                    foreach (var p in cmdParms)
                    {
                        cmd.Parameters.Add(((ICloneable)p).Clone() as DbParameter);
                    }
                }
            }
            catch (Exception ex)
            {
                cmd.Connection.Dispose();
                cmd.Dispose();

                throw ex;
            }

            return cmd;
        }

        /// <summary>
        /// 释放 DbCommand 所使用的资源
        /// </summary>
        /// <param name="sender">当前对数据库执行命令对象</param>
        /// <param name="e">事件对象</param>
        private void cmd_Disposed(object sender, EventArgs e)
        {
            var cmd = sender as DbCommand;

            if (cmd.Transaction == null)    //事务未开启时，每次操作都应该释放数据库连接
            {
                cmd.Connection.Dispose();   //关闭连接
            }
        }

        #endregion

        #region 命令方法

        #region ExecuteNonQuery

        #region Text

        /// <summary>
        /// 执行SQL语句,返回影响的记录数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdParms">参数集合</param>
        /// <returns>影响的记录数</returns>
        public int ExecuteNonQuery(string sql, DbParameter[] cmdParms = null)
        {
            return ExecuteNonQuery(sql, cmdParms, CommandType.Text);
        }

        #endregion

        #region StoredProcedure

        /// <summary>
        /// 执行存储过程,返回影响的记录数
        /// </summary>
        /// <param name="spName">存储过程名称</param>
        /// <param name="cmdParms">参数集合</param>
        /// <returns>影响的记录数</returns>
        internal int ExecuteNonQuerySp(string spName, DbParameter[] cmdParms = null)
        {
            return ExecuteNonQuery(spName, cmdParms, CommandType.StoredProcedure);
        }

        #endregion

        #region Common

        /// <summary>
        /// 执行SQL语句或者存储过程,返回影响的记录数
        /// </summary>
        /// <param name="objText">SQL语句或者存储过程名称</param>
        /// <param name="cmdParms">参数集合</param>
        /// <param name="cmdType">如何解释命令字符串</param>
        /// <returns>影响的记录数</returns>
        private int ExecuteNonQuery(string objText, DbParameter[] cmdParms, CommandType cmdType)
        {
            using (var cmd = CreateDbCommand(objText, cmdParms, cmdType))
            {
                try
                {
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    cmd.Connection.Dispose();

                    throw ex;
                }
            }
        }

        #endregion

        #endregion

        #region ExecuteScalar

        #region Text

        /// <summary>
        /// 执行SQL语句,返回第一行第一列的值
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdParms">参数集合</param>
        /// <returns>返回执行结果</returns>
        public T ExecuteScalar<T>(string sql, DbParameter[] cmdParms = null)
        {
            return ConvertHelper.ChangeType<T>(ExecuteScalar(sql, cmdParms, CommandType.Text));
        }

        #endregion

        #region StoredProcedure

        /// <summary>
        /// 执行SQL语句,返回第一行第一列的值
        /// </summary>
        /// <param name="spName">SQL存储过程名称</param>
        /// <param name="cmdParms">参数集合</param>
        /// <returns>返回执行结果</returns>
        internal T ExecuteScalarSp<T>(string spName, DbParameter[] cmdParms = null)
        {
            return ConvertHelper.ChangeType<T>(ExecuteScalar(spName, cmdParms, CommandType.StoredProcedure));
        }

        #endregion

        #region Common

        /// <summary>
        /// 执行SQL语句或者存储过程,返回查询结果(object)
        /// </summary>
        /// <param name="objText">SQL语句或者存储过程</param>
        /// <param name="cmdParms">参数集合</param>
        /// <param name="cmdType">如何解释命令字符串</param>
        /// <returns>查询结果(object)</returns>
        private object ExecuteScalar(string objText, DbParameter[] cmdParms, CommandType cmdType)
        {
            using (var cmd = CreateDbCommand(objText, cmdParms, cmdType))
            {
                try
                {
                    return cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    cmd.Connection.Dispose();

                    throw ex;
                }
            }
        }

        #endregion

        #endregion

        #region ExecuteReader

        #region Text

        /// <summary>
        /// 执行SQL语句,返回DbDataReader
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdParms">参数集合</param>
        /// <returns>DbDataReader</returns>
        public DbDataReader ExecuteReader(string sql, DbParameter[] cmdParms = null)
        {
            return ExecuteReader(sql, cmdParms, CommandType.Text);
        }

        #endregion

        #region StoredProcedure

        /// <summary>
        /// 执行存储过程,返回DbDataReader
        /// </summary>
        /// <param name="spName">存储过程名称</param>
        /// <param name="cmdParms">参数集合</param>
        /// <returns>DbDataReader</returns>
        internal DbDataReader ExecuteReaderSp(string spName, DbParameter[] cmdParms = null)
        {
            return ExecuteReader(spName, cmdParms, CommandType.StoredProcedure);
        }

        #endregion

        #region Common

        /// <summary>
        /// 执行SQL语句或者存储过程,返回DbDataReader
        /// </summary>
        /// <param name="objText">SQL语句或者存储过程</param>
        /// <param name="cmdParms">参数集合</param>
        /// <param name="cmdType">如何解释命令字符串</param>
        /// <returns>DbDataReader</returns>
        private DbDataReader ExecuteReader(string objText, DbParameter[] cmdParms, CommandType cmdType)
        {
            using (var cmd = CreateDbCommand(objText, cmdParms, cmdType, false))
            {
                try
                {
                    return cmd.ExecuteReader(cmd.Transaction == null ? CommandBehavior.CloseConnection : CommandBehavior.Default);
                }
                catch (Exception ex)
                {
                    cmd.Connection.Dispose();

                    throw ex;
                }
            }
        }

        #endregion

        #endregion

        #endregion

        #region 接口实现

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务
        /// </summary>
        public void Dispose()
        {

        }

        #endregion
    }
}
