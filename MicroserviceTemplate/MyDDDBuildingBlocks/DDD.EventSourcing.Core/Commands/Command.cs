using MediatR;
using System;

namespace DDD.EventSourcing.Core.Commands
{
    public abstract class Command<TResponse> : IRequest<TResponse>
        where TResponse : CommandResponse
    {
        public Guid AggregateId { get; protected set; }
        public string CommandType { get; protected set; }
        public DateTime Timestamp { get; private set; }

        protected Command()
        {
            CommandType = GetType().Name;
            Timestamp = DateTime.Now;
        }
    }
}