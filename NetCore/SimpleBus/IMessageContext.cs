using System;

namespace SimpleBus
{
    public interface IMessageContext<T> where T : IMessage
    {
        int EventId { get; }
        T Message { get; }
        void Publish<TE>(TE message) where TE : IEventMessage;
    }
}
