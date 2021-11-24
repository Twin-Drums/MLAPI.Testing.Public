using System;
using Unity.Collections;

namespace DefaultNamespace
{
    public struct UnmanagedTestContainer : IEquatable<UnmanagedTestContainer>
    {
        public FixedString32Bytes Value02;
        public FixedString32Bytes Value01;
        public FixedString32Bytes Value03;

        public bool Equals(UnmanagedTestContainer other) => Value01.Equals(other.Value01) && Value02.Equals(other.Value02) && Value03.Equals(other.Value03);
        public override bool Equals(object obj) => obj is UnmanagedTestContainer other && Equals(other);
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Value01.GetHashCode();
                hashCode = (hashCode * 397) ^ Value02.GetHashCode();
                hashCode = (hashCode * 397) ^ Value03.GetHashCode();
                return hashCode;
            }
        }
    }
}
