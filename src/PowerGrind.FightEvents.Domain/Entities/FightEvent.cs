using System;
using System.Collections.Generic;
using System.Text;

namespace PowerGrind.FightEvents.Domain.Entities
{
    public class FightEvent
    {
        public Guid Id { get; set; }
        public string OrganizationName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public Uri SourceUrl { get; set; }
    }
}
