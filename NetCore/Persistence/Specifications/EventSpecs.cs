using System;
using System.Linq.Expressions;
using CrudDatastore;

namespace Persistence.Specifications
{
    public class EventSpecs : Specification<Entities.Event>
    {
        private EventSpecs(Expression<Func<Entities.Event, bool>> predicate)
            : base(predicate)
        { }

        private EventSpecs(string command, params object[] parameters)
            : base(command, parameters)
        { }

        public static EventSpecs GetEvent(int id)
        {
            return new EventSpecs(t => t.Id == id);
        }

        public static EventSpecs GetEvents()
        {
            return new EventSpecs(t => true);
        }
    }
}
