using System;
using CrudDatastore;

namespace Persistence.Entities
{
    public class Event : EntityBase
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Payload { get; set; }
    }
}
