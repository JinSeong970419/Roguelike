using System.Text;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Roguelike.Core
{
    [System.Serializable]
    [StructLayout(LayoutKind.Explicit)]
    public class Version
    {
        [FieldOffset(0)] private ulong _number;
        [SerializeField] [FieldOffset(6)] private ushort _major;
        [SerializeField] [FieldOffset(4)] private ushort _minor;
        [SerializeField] [FieldOffset(0)] private uint _patch;

        public ulong Number => _number;
        public ushort Major => _major;
        public ushort Minor => _minor;
        public uint Patch => _patch;

        public Version(ushort major, ushort minor, uint patch)
        {
            _major = major;
            _minor = minor;
            _patch = patch;
        }

        public static bool operator ==(Version lhs, Version rhs)
        {
            return lhs._number == rhs._number;
        }

        public static bool operator !=(Version lhs, Version rhs)
        {
            return lhs._number != rhs._number;
        }

        public static bool operator <(Version lhs, Version rhs)
        {
            return lhs._number < rhs._number;
        }

        public static bool operator >(Version lhs, Version rhs)
        {
            return lhs._number > rhs._number;
        }

        public static bool operator <=(Version lhs, Version rhs)
        {
            return lhs._number <= rhs._number;
        }

        public static bool operator >=(Version lhs, Version rhs)
        {
            return lhs._number >= rhs._number;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(_major.ToString());
            stringBuilder.Append(".");
            stringBuilder.Append(_minor.ToString());
            stringBuilder.Append(".");
            stringBuilder.Append(_patch.ToString());
            return stringBuilder.ToString();
        }

        public ulong ToNumber()
        {
            return _number;
        }
    }
}
