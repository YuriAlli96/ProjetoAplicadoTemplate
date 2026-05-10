using Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Common.Base
{
    public abstract class Entity
    {
        public long Id { get; protected set; }
        public DateTimeOffset DataCriacao { get; protected set; }
        public DateTimeOffset? DataAtualizacao { get; protected set; }
        public bool IsTransient() => Id == 0;

        protected Entity()
        {
            DataCriacao = TimeProvider.System.GetUtcNow();
        }

        protected Entity(long id)
        {
            Id = id;
            DataCriacao = TimeProvider.System.GetUtcNow();
        }

        protected void SetDataAtualizacao()
        {
            DataAtualizacao = TimeProvider.System.GetUtcNow();
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Entity other) return false;
            if (ReferenceEquals(this, other)) return true;

            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return IsTransient() ? base.GetHashCode() : Id.GetHashCode();
        }

        public static bool operator == (Entity? left, Entity? right) =>
            left is not null ? left.Equals(right) : right is null;

        public static bool operator != (Entity? left, Entity? right) =>
            !(left == right);

        private readonly List<IDomainEvent> _domainEvents = [];
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
        protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
        public void RemoveDomainEvent(IDomainEvent domainEvent) => _domainEvents.Remove(domainEvent);
        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}
