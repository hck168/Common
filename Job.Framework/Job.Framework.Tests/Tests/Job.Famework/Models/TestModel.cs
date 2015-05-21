using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Job.Framework.Tests.Job.Famework
{
    public sealed class TestModel
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime From { get; private set; }
        public DateTime To { get; private set; }

        public TestModel(string title, string description, DateTime from, DateTime to)
        {
            this.Title = title;
            this.Description = description;
            this.From = from;
            this.To = to;
        }
    }
}
