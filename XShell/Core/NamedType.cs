using System;
using System.Collections.Generic;

namespace XShell.Core
{
    public class NamedType : IEquatable<NamedType>
    {
        private readonly int _hashCode;

        public Type Type { get; }

        public string Name { get; }

        public NamedType(Type type, string name)
        {
            Type = type;
            Name = name;
            unchecked
            {
                _hashCode = ((type != null ? type.GetHashCode() : 0) * 397) ^ (name?.GetHashCode() ?? 0);
            }
        }

        #region Equality members

        public bool Equals(NamedType other)
        {
            return other != null && (Type == other.Type && string.Equals(Name, other.Name));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is NamedType type && Equals(type);
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }

        public static bool operator ==(NamedType left, NamedType right)
        {
            return ReferenceEquals(left, right) || (left != null && left.Equals(right));
        }

        public static bool operator !=(NamedType left, NamedType right)
        {
            return !ReferenceEquals(left, right) || left == null || !left.Equals(right);
        }

        #endregion

        #region EqualityComparer

        private sealed class TypeNameEqualityComparer : IEqualityComparer<NamedType>
        {
            public bool Equals(NamedType x, NamedType y)
            {
                if (x == null && y == null) return true;
                if (x == null || y == null) return false;
                return x.Type == y.Type && string.Equals(x.Name, y.Name);
            }

            public int GetHashCode(NamedType obj)
            {
                return obj._hashCode;
            }
        }

        public static IEqualityComparer<NamedType> Comparer { get; } = new TypeNameEqualityComparer();

        #endregion

        public override string ToString()
        {
            return $"Type: {Type}, Name: {Name}";
        }
    }
}