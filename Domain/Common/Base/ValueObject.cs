using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Common.Base
{
    public abstract class ValueObject
    {
        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (ValueObject)obj;
            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }
        public override int GetHashCode()
        {
            // Combina os HashCodes de todos os componentes
            return GetEqualityComponents()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }

        public static bool operator ==(ValueObject a, ValueObject b)
        => a?.Equals(b) ?? b is null;

        public static bool operator !=(ValueObject a, ValueObject b)
            => !(a == b);
    }
}
