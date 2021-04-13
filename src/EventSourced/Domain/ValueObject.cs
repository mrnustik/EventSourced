using System;
using System.Linq;
using System.Reflection;

namespace EventSourced.Domain
{
    public abstract class ValueObject
    {
        protected bool Equals(ValueObject? other)
        {
            return other != null && GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as ValueObject);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            foreach (var equalityComponent in GetEqualityComponents()) hashCode.Add(equalityComponent);
            return hashCode.ToHashCode();
        }

        public static bool operator ==(ValueObject? left, ValueObject? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValueObject? left, ValueObject? right)
        {
            return !Equals(left, right);
        }

        protected virtual object?[] GetEqualityComponents()
        {
            return GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p => p.GetValue(this))
                .ToArray();
        }
    }
}