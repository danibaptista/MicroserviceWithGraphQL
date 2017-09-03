using DDD.EventSourcing.Core.Commands;
using System;

namespace MicroserviceArchitecture.GameOfThrones.BusinessCommand.Commands
{
    public class IdentifiedCommand<T, R> : Command<R>
        where T : Command<R>
        where R : CommandResponse
    {
        public T Command { get; }
        public Guid Id { get; }

        public IdentifiedCommand(T command, Guid id)
        {
            Command = command;
            Id = id;
        }
    }
}