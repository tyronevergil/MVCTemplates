using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBus
{
    public class ServiceBus : IServiceBus
    {
        private readonly IEnumerable<IMessageHandler> _handlers;
        private readonly IEventStore _store;

        public ServiceBus(IEnumerable<IMessageHandler> handlers, IEventStore store)
        {
            _handlers = handlers;
            _store = store;

            _store.Created += (sender, args) =>
            {
                Process(args.Command, args.Id);
            };
        }

        public void Send<T>(T command) where T : ICommandMessage
        {
            _store.Add(command);
        }

        private void Process(IMessage message, int eventId)
        {
            var messageType = message.GetType();
            var handlerType = typeof(IMessageHandler<>).MakeGenericType(messageType);
            foreach (var handler in _handlers.Where(h => handlerType.IsAssignableFrom(h.GetType())))
            {
                var method = handlerType.GetMethod("Handle");
                method.Invoke(handler,
                    new[] { Activator.CreateInstance(typeof(MessageContext<>).MakeGenericType(messageType),
                            eventId,
                            message,
                            new PublishHandler((m) => Process(m, eventId))) });
            }
        }
    }

    internal class MessageContext<T> : IMessageContext<T> where T : IMessage
    {
        private readonly PublishHandler _handler;

        public MessageContext(int eventId, T message, PublishHandler handler)
        {
            EventId = eventId;
            Message = message;

            _handler = handler;
        }

        public int EventId { get; private set; }

        public T Message { get; private set; }

        public void Publish<TE>(TE message) where TE : IEventMessage
        {
            _handler.Publish(message);
        }
    }

    internal class PublishHandler
    {
        private readonly Action<IMessage> _publish;

        public PublishHandler(Action<IMessage> publish)
        {
            _publish = publish;
        }

        public void Publish<T>(T message) where T : IEventMessage
        {
            _publish(message);
        }
    }
}
