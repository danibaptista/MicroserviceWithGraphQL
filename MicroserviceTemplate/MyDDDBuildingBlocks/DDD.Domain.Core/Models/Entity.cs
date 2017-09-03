using System.Collections.Generic;

namespace DDD.Domain.Core.Models
{
    using DDD.EventSourcing.Core.Events;
    using SeedWork;
    using System;

    public abstract class Entity : IEntity<Guid>
    {
        private List<Event> domainEvents;
        private Guid id;
        private int? requestedHashCode;
        public IList<Event> DomainEvents => domainEvents.AsReadOnly();

        public virtual Guid Id
        {
            get
            {
                return id;
            }
            protected set
            {
                id = value;
            }
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }

        public static bool operator ==(Entity left, Entity right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
                return true;

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;

            return left.Equals(right);
        }

        public void AddDomainEvent(Event eventItem)
        {
            domainEvents = domainEvents ?? new List<Event>();
            domainEvents.Add(eventItem);
        }

        public override bool Equals(object obj)
        {
            var item = obj as Entity;

            if (item == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            if (item.IsTransient() || IsTransient())
                return false;
            else
                return Id.Equals(item.Id);
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!requestedHashCode.HasValue)
                    requestedHashCode = Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

                return requestedHashCode.Value;
            }
            else
                return base.GetHashCode();
        }

        public bool IsTransient()
        {
            return Equals(Id, default(Guid));
        }

        public void RemoveDomainEvent(Event eventItem)
        {
            if (domainEvents is null) return;
            domainEvents.Remove(eventItem);
        }

        public override string ToString()
        {
            return string.Format("{0} [Id={1}]", GetType().Name, Id);
        }
    }
}