using System;
using System.Collections.Generic;

namespace XShell.Core
{
    public class NamedType : IEquatable<NamedType>
    {
        private readonly Type type;
        private readonly string name;
        private readonly int hashCode;

        public Type Type {get { return type; }}

        public string Name { get { return name; } }

        public NamedType(Type type, string name)
        {
            this.type = type;
            this.name = name;
            unchecked
            {
                hashCode = ((type != null ? type.GetHashCode() : 0) * 397) ^ (name != null ? name.GetHashCode() : 0);
            }
        }

        #region Equality members

        public bool Equals(NamedType other)
        {
            return type == other.type && string.Equals(name, other.name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is NamedType && Equals((NamedType)obj);
        }

        public override int GetHashCode()
        {
            return hashCode;
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

        private sealed class TYPENameEqualityComparer : IEqualityComparer<NamedType>
        {
            public bool Equals(NamedType x, NamedType y)
            {
                return x.type == y.type && string.Equals(x.name, y.name);
            }

            public int GetHashCode(NamedType obj)
            {
                return obj.hashCode;
            }
        }

        private static readonly IEqualityComparer<NamedType> comparerInstance = new TYPENameEqualityComparer();

        public static IEqualityComparer<NamedType> Comparer
        {
            get { return comparerInstance; }
        }

        #endregion

        public override string ToString()
        {
            return string.Format("Type: {0}, Name: {1}", type, name);
        }
    }
}