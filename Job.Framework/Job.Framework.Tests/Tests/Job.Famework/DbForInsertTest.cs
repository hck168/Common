using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Job.Framework.Tests.Job.Famework
{
    [TestClass]
    public class DbForInsertTest
    {
        [TestMethod]
        public void TestInsert1()
        {
            /*
             * 语句：INSERT INTO T_User (UserAccount,UserPwd) VALUES (@UserAccount,@UserPwd)
             * 备注：条件为 null 则不会生成查询条件
             */

            using (var dbContext = DbContext.BeginConnect())
            {
                var result1 = dbContext.Insert("T_User").Values(new
                {
                    UserAccount = "zerobase@qq.com",
                    UserPwd = "123456"
                });

                if (result1.State == DbOperateState.Success)
                {
                    var id = result1.Value;  //自增字段值
                }

                var result2 = dbContext.Insert("T_User", false).Values(new
                {
                    UserAccount = "zerobase@qq.com",
                    UserPwd = "123456"
                });

                if (result2.State == DbOperateState.Success)
                {
                    var rows = result2.Value;   //影响行数
                }
            }
        }
    }
}
