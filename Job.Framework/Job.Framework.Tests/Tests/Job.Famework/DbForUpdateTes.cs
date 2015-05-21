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
