using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.SharedKernel.DomainObjects
{
    public abstract class Entity
    {
        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; protected set; }

        private readonly List<Event> _events = [];
        public IReadOnlyCollection<Event> Events => _events.AsReadOnly();

        public void AddEvent(Event @event) => _events.Add(@event);

        public void RemoveEvent(Event @event) => _events.Remove(@event);

        public void PurgeEvents() => _events.Clear();

        public override bool Equals(object? obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (a is null && b is null) return true;
            if (!(a is not null && b is not null)) return false;

            return a.Equals(b);
        }

        public abstract void Validate();

        public static bool operator !=(Entity a, Entity b) => !(a == b);

        public override int GetHashCode() => (GetType().GetHashCode() * 907) + Id.GetHashCode();

        public override string ToString() => $"{GetType().Name} [Id = {Id}]";
    }
}