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
            using (var dbContext = DbContext.BeginConnect())
            {
                var result = dbContext.Insert("T_User2").Values(new
                {
                    UserAccount = "zerobase@qq.com",
                    UserPwd = "123456"
                });

                if (result.State == DbOperateState.Success)
                {

                }
            }
        }
    }
}
