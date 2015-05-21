using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Job.CloudOffice.CommandDto
{
    public class ItemCreatedCommand : Command
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public CreateItemCommandResult CommandResult { get; private set; }

        public ItemCreatedCommand(Guid aggregateId, string title, string description, DateTime from, DateTime to, int version = 0)
        {
            this.AggregateId = aggregateId;

            this.Title = title;
            this.Description = description;
            this.From = from;
            this.To = to;

            this.Version = version;

            this.CommandResult = new CreateItemCommandResult();
        }
    }

    public class CreateItemCommandResult
    {
        public string ItemID { get; set; }
    }
}
