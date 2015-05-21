using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Job.Framework.Tests.Job.Famework
{
    [TestClass]
    public class DbForDeleteTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (var dbContext = DbContext.BeginConnect())
            {
                var result1 = dbContext.Delete("T_User").Where(new
                {
                    @Id = "52c591e6-5405-4203-8c62-a49820583646"
                });

                var result2 = dbContext.Delete("T_User").Where(true, new
                {
                    @Id = "52c591e6-5405-4203-8c62-a49820583646"
                });

                var result3 = dbContext.Delete("T_User").Where("Id = @Id", new
                {
                    @Id = "52c591e6-5405-4203-8c62-a49820583646"
                });
            }
        }
    }
}
