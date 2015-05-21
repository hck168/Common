using Job.Framework.Common;
using Job.Framework.Config;
using Job.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Job.Framework
{
    public sealed partial class DbContext : IDisposable
    {
        public static DbContext BeginConnect()
        {
            return new DbContext();
        }

        public static DbContext BeginConnect(int commandTimeout)
        {
            return new DbContext() { CommandTimeout = commandTimeout };
        }

        public static DbContext BeginConnect(string dbConnectionName)
        {
            return new DbContext(dbConnectionName);
        }

        public static DbContext BeginConnect(string dbConnectionName, int commandTimeout)
        {
            return new DbContext(dbConnectionName) { CommandTimeout = commandTimeout };
        }
    }
}
