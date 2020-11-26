using System;

namespace SimpleBus
{
    public interface IEventStore
    {
        event EventHandler<CreatedEventArgs> Created;
        void Add<T>(T command) where T : ICommandMessage;
    }

    public class CreatedEventArgs : EventArgs
    {
        public CreatedEventArgs(int id, ICommandMessage command)
        {
            Id = id;
            Command = command;
        }

        public int Id { get; private set; }
        public ICommandMessage Command { get; private set; }
    }
}
