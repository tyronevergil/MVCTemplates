using System;
using System.Linq;
using Newtonsoft.Json;
using Persistence;
using SimpleBus;

namespace WebApplication.Infrastructure
{
    public class EventStore : IEventStore
    {
        private readonly IEventDataContextFactory _contextFactory;

        public EventStore(IEventDataContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public event EventHandler<CreatedEventArgs> Created;

        public void Add<T>(T command) where T : ICommandMessage
        {
            var type = command.GetType().ToString().Split('.').Last().Replace("CommandModel", "");
            var payload = JsonConvert.SerializeObject(command);

            using (var context = _contextFactory.CreateDataContext())
            {
                var entry = new Persistence.Entities.Event
                {
                    Type = type,
                    Payload = payload
                };

                context.Add(entry);
                context.SaveChanges();

                Created?.Invoke(this, new CreatedEventArgs(entry.Id, command));
            }
        }
    }
}
