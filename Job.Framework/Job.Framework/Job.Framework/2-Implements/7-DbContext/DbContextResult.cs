using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Job.Framework
{
    public enum DbOperateState
    {
        Success = 1,
        Failed = 2
    }

    public sealed class DbContextResult
    {
        public DbOperateState State { get; private set; }

        public long Value { get; private set; }

        internal DbContextResult(DbOperateState state, long value)
        {
            this.State = state;
            this.Value = value;
        }
    }
}
