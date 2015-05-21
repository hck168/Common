using System;
using System.Dynamic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Job.Framework.Tests.Job.Famework
{
    [TestClass]
    public class DbForSelectTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            /*
             * 语句：SELECT * FROM T_User WHERE Id = @Id
             * 备注：获取单条记录，查询多条记录则报异常
             */

            using (var dbContext = DbContext.BeginConnect())
            {
                var result1 = dbContext.Select("T_User").Columns("*").Where(new
                {
                    @Id = "52c591e6-5405-4203-8c62-a49820583645"
                });

                var result2 = dbContext.Select("T_User").Columns("*").Where(true, new
                {
                    @Id = "52c591e6-5405-4203-8c62-a49820583645"
                });

                var result3 = dbContext.Select("T_User").Columns("*").Where("Id = @Id", new
                {
                    @Id = "52c591e6-5405-4203-8c62-a49820583645"
                });
            }
        }
    }
}
