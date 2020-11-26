using System;

namespace SimpleBus
{
    public interface IMessage
    {
    }

    public interface ICommandMessage : IMessage
    {
    }

    public interface IEventMessage : IMessage
    {
    }
}
