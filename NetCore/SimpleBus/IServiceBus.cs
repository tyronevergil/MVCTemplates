using System;

namespace SimpleBus
{
    public interface IServiceBus
    {
        void Send<T>(T command) where T : ICommandMessage;
    }
}
