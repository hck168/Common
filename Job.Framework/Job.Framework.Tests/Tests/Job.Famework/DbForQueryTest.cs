using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Job.Framework.Tests.Job.Famework
{
    public class UserInfo
    {
        public Guid Id { get; set; }
        public string UserAccount { get; set; }
        public string UserPwd { get; set; }
    }

    [TestClass]
    public class DbForQueryTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (var dbContext = DbContext.BeginConnect())
            {
                /*
                 * 语句：SELECT * FROM T_User WHERE Id = @Id
                 * 备注：条件为 null 则不会生成查询条件
                 */

                var list1 = dbContext.Query("T_User").Columns("*").Where(new
                {
                    Id = "52c591e6-5405-4203-8c62-a49820583645"
                });

                foreach (var item in list1)
                {
                    var userId = item.Id;
                }

                /*
                 * 语句：SELECT Id,UserAccount,UserPwd FROM T_User WHERE Id = @Id
                 * 备注：条件为 null 则不会生成查询条件
                 */

                var list2 = dbContext.Query("T_User").Columns("Id", "UserAccount", "UserPwd").Where(true, new
                {
                    Id = "52c591e6-5405-4203-8c62-a49820583645"
                });

                foreach (var item in list2)
                {
                    var userId = item.Id;
                }

                /*
                 * 语句：SELECT * FROM T_User WHERE Id = @Id
                 * 备注：条件为 null 则不会生成查询条件
                 */

                var list3 = dbContext.Query<UserInfo>("T_User").Columns("*").Where("Id = @Id",new
                {
                    Id = "52c591e6-5405-4203-8c62-a49820583645"
                });

                foreach (var item in list3)
                {
                    var userId = item.Id;
                }

                /*
                 * 语句：SELECT Id,UserAccount FROM T_User
                 * 备注：条件为 null 则不会生成查询条件
                 */

                var list4 = dbContext.Query<UserInfo>("T_User").Columns("Id", "UserAccount").Where(null);

                foreach (var item in list4)
                {
                    var userId = item.Id;
                }
            }
        }
    }
}
