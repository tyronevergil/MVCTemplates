using System;
using System.Collections.Generic;
using System.Linq;
using CrudDatastore;

namespace Persistence
{
    public class EventDataContextFactory : IEventDataContextFactory
    {
        public DataContextBase CreateDataContext()
        {
            return new GenericDataContext(new EventUnitOfWorkInMemory());
        }
    }

    internal class EventUnitOfWorkInMemory : UnitOfWorkBase
    {
        private static readonly IList<Entities.Event> _events = new List<Entities.Event>();

        private IDataStore<Entities.Event> Events()
        {
            return new DataStore<Entities.Event>(
                new DelegateCrudAdapter<Entities.Event>(
                    /* create */
                    (e) =>
                    {
                        e.Id = (_events.Any() ? _events.Max(i => i.Id) : 0) + 1;
                        _events.Add(new Entities.Event
                        {
                            Id = e.Id,
                            Type = e.Type,
                            Payload = e.Payload
                        });
                    },

                    /* update */
                    (e) =>
                    {
                    },

                    /* delete */
                    (e) =>
                    {
                    },

                    /* read */
                    (predicate) =>
                    {
                        return _events.Where(predicate.Compile()).AsQueryable();
                    }
                ));
        }

        public EventUnitOfWorkInMemory()
        {
            this.Register(Events());
        }
    }
}
