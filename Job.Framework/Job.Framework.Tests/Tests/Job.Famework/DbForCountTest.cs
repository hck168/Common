using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Job.Framework.Tests.Job.Famework
{
    [TestClass]
    public class DbForCountTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            /*
             * 语句：SELECT COUNT(1) FROM T_User WHERE Id = @Id
             * 备注：条件为 null 则不会生成查询条件
             */

            using (var dbContext = DbContext.BeginConnect())
            {
                var count1 = dbContext.Count("T_User").Where(new
                {
                    @Id = "52c591e6-5405-4203-8c62-a49820583645"
                });

                var count2 = dbContext.Count("T_User").Where(true, new
                {
                    @Id = "52c591e6-5405-4203-8c62-a49820583645"
                });

                
                var count3 = dbContext.Count("T_User").Where("Id = @Id", new
                {
                    @Id = "52c591e6-5405-4203-8c62-a49820583645"
                });
            }
        }
    }
}
