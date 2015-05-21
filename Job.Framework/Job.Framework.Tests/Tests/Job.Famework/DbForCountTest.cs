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
