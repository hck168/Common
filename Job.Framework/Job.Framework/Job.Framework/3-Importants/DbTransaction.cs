using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading;
using System.Data.Common;
using System.Data;

namespace Job.Core
{
    /// <summary>
    /// 事务机制常用基础类
    /// </summary>
    public sealed class Transaction : IDisposable
    {
        #region 内部使用

        /// <summary>
        /// 内部使用的事务状态类
        /// </summary>
        internal class TransationState
        {
            public Dictionary<string, DbTransaction> TransactionPool { get; set; }
            public IsolationLevel IsolationLevel { get; set; }
            public bool IsRollBack { get; set; }
            public bool IsCommit { get; set; }
        }

        #endregion

        #region 相关属性

        /// <summary>
        /// 事务状态常量声明
        /// </summary>
        private const string TransationStateConst = "Job.Core.Transaction.TransationState";

        [ThreadStatic]
        private static TransationState transationState;
        /// <summary>
        /// 获取当前的事务连接池
        /// </summary>
        /// <returns></returns>
        private static TransationState Current
        {
            get
            {
                return HttpContext.Current != null ? HttpContext.Current.Items[TransationStateConst] as TransationState : transationState;
            }
        }

        /// <summary>
        /// 获取当前是否为顶级事务
        /// </summary>
        public bool IsParent { get; private set; }

        /// <summary>
        /// 获取事务是否已经启用
        /// </summary>
        public static bool IsTransaction
        {
            get
            {
                return Transaction.Current != null;
            }
        }

        /// <summary>
        /// 获取当前连接事务锁
        /// </summary>
        public static IsolationLevel IsolationLevel
        {
            get
            {
                return Transaction.Current.IsolationLevel;
            }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 开启事务，默认事务锁级别为：ReadCommitted
        /// </summary>
        /// <param name="isolationLevel">事务锁行为</param>
        public Transaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (!IsParent && !IsTransaction)    //标志着事务是否启用
            {
                Thread.BeginThreadAffinity();   //开始标识指令

                var current = new TransationState
                {
                    TransactionPool = new Dictionary<string, DbTransaction>(),
                    IsolationLevel = isolationLevel,
                    IsRollBack = true,
                    IsCommit = false
                };

                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Items[TransationStateConst] = current;
                }
                else
                {
                    transationState = current;
                }

                IsParent = true;
            }
        }

        #endregion

        #region 事务方法

        /// <summary>
        /// 获取指定的事务
        /// </summary>
        /// <param name="connectName"></param>
        /// <returns></returns>
        public static DbTransaction Get(string connectName)
        {
            return Transaction.Current != null && Transaction.Current.TransactionPool.ContainsKey(connectName) ? Transaction.Current.TransactionPool[connectName] : null;
        }

        /// <summary>
        /// 设置指定的事务
        /// </summary>
        /// <param name="connectName"></param>
        /// <param name="transaction"></param>
        public static void Set(string connectName, DbTransaction transaction)
        {
            if (Transaction.Current.TransactionPool.ContainsKey(connectName) == false)
            {
                Transaction.Current.TransactionPool.Add(connectName, transaction);
            }
        }

        /// <summary>
        /// 对当前挂起事务进行回滚
        /// </summary>
        public void Rollback()
        {
            Transaction.Current.IsRollBack = true;
            Transaction.Current.IsCommit = false;
        }

        /// <summary>
        /// 对当前挂起事务进行提交
        /// </summary>
        public void Commit()
        {
            Transaction.Current.IsRollBack = false;
            Transaction.Current.IsCommit = true;
        }

        /// <summary>
        /// 释放资源，对事务进行判断
        /// </summary>
        public void Dispose()
        {
            if (IsParent && Transaction.Current.TransactionPool.Count > 0)  //顶级事务且事务连接池存在事务
            {
                try
                {
                    foreach (var item in Transaction.Current.TransactionPool.Values)  //依次提交事务
                    {
                        using (var conn = item.Connection)  //自动关闭数据库连接
                        {
                            if (conn == null)    //数据库连接已被释放
                            {
                                continue;
                            }

                            if (Transaction.Current.IsRollBack) //回滚与提交事务为互斥对象
                            {
                                item.Rollback();
                            }

                            if (Transaction.Current.IsCommit)   //回滚与提交事务为互斥对象
                            {
                                item.Commit();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Items.Remove(TransationStateConst);
                }
                else
                {
                    transationState = null;
                }

                //结束标识指令
                Thread.EndThreadAffinity();
            }
        }

        #endregion
    }
}
