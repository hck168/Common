using System;
using System.Dynamic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Job.Framework.Tests.Job.Famework
{
    [TestClass]
    public class DbForUpdateTes
    {
        [TestMethod]
        public void TestMethod1()
        {
            /*
             * 语句：UPDATE T_User SET UserAccount = @UserAccount,UserPwd = @UserPwd WHERE Id = @P_Id
             * 备注：条件为 null 则不会生成查询条件，默认加前缀 @P ，防止参数重复
             */

            using (var dbContext = DbContext.BeginConnect())
            {
                var columnData = new
                {
                    UserAccount = "13631487876",
                    UserPwd = "4297F44B13955235245B2497399D7A93"
                };

                var result1 = dbContext.Update("T_User").Set(columnData).Where(new
                {
                    Id = "52c591e6-5405-4203-8c62-a49820583645"
                });

                var result2 = dbContext.Update("T_User").Set(columnData).Where(true, new
                {
                    Id = "52c591e6-5405-4203-8c62-a49820583645"
                });

                var result3 = dbContext.Update("T_User").Set(columnData).Where("Id = @Id", new
                {
                    Id = "52c591e6-5405-4203-8c62-a49820583645"
                });
            }
        }
    }
}
