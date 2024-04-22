using System;
using UnityEngine;

namespace Roguelike.Core
{
    public partial class Automata
    {
        public enum OperatorType
        {
            Equals,
            NotEquals,
            Greater,
            Less,
            GreaterThanOrEqualTo,
            LessThanOrEqualTo,
            Move,
            AddAndMove,
        }

        public abstract class Operator
        {
            private OperatorType type;
            public OperatorType Type { get { return type; } }
            public Operator(OperatorType type)
            {
                this.type = type;
            }

            public virtual void Execute(ref object lhs, object rhs)
            {
                Type lhsType = lhs.GetType();
                Type rhsType = rhs.GetType();
                if (lhsType != rhsType)
                {
                    Debug.LogError($"Data types do not match. LHS: {lhsType.Name} / RHS: {rhsType.Name}");
                    return;
                }
                if (lhsType == typeof(bool))
                {
                    var result = (bool)lhs;
                    Execute(ref result, (bool)rhs);
                    lhs = result;
                }
                else if (lhsType == typeof(byte))
                {
                    var result = (byte)lhs;
                    Execute(ref result, (byte)rhs);
                    lhs = result;
                }
                else if (lhsType == typeof(sbyte))
                {
                    var result = (sbyte)lhs;
                    Execute(ref result, (sbyte)rhs);
                    lhs = result;
                }
                else if (lhsType == typeof(char))
                {
                    var result = (char)lhs;
                    Execute(ref result, (char)rhs);
                    lhs = result;
                }
                else if (lhsType == typeof(decimal))
                {
                    var result = (decimal)lhs;
                    Execute(ref result, (decimal)rhs);
                    lhs = result;
                }
                else if (lhsType == typeof(double))
                {
                    var result = (double)lhs;
                    Execute(ref result, (double)rhs);
                    lhs = result;
                }
                else if (lhsType == typeof(float))
                {
                    var result = (float)lhs;
                    Execute(ref result, (float)rhs);
                    lhs = result;
                }
                else if (lhsType == typeof(int))
                {
                    var result = (int)lhs;
                    Execute(ref result, (int)rhs);
                    lhs = result;
                }
                else if (lhsType == typeof(uint))
                {
                    var result = (uint)lhs;
                    Execute(ref result, (uint)rhs);
                    lhs = result;
                }
                else if (lhsType == typeof(nint))
                {
                    var result = (nint)lhs;
                    Execute(ref result, (nint)rhs);
                    lhs = result;
                }
                else if (lhsType == typeof(nuint))
                {
                    var result = (nuint)lhs;
                    Execute(ref result, (nuint)rhs);
                    lhs = result;
                }
                else if (lhsType == typeof(long))
                {
                    var result = (long)lhs;
                    Execute(ref result, (long)rhs);
                    lhs = result;
                }
                else if (lhsType == typeof(ulong))
                {
                    var result = (ulong)lhs;
                    Execute(ref result, (ulong)rhs);
                    lhs = result;
                }
                else if (lhsType == typeof(short))
                {
                    var result = (short)lhs;
                    Execute(ref result, (short)rhs);
                    lhs = result;
                }
                else if (lhsType == typeof(ushort))
                {
                    var result = (ushort)lhs;
                    Execute(ref result, (ushort)rhs);
                    lhs = result;
                }
                else if (lhsType == typeof(string))
                {
                    var result = (string)lhs;
                    Execute(ref result, (string)rhs);
                    lhs = result;
                }
            }
            public virtual void Execute(ref bool lhs, bool rhs) { }
            public virtual void Execute(ref byte lhs, byte rhs) { }
            public virtual void Execute(ref sbyte lhs, sbyte rhs) { }
            public virtual void Execute(ref char lhs, char rhs) { }
            public virtual void Execute(ref decimal lhs, decimal rhs) { }
            public virtual void Execute(ref double lhs, double rhs) { }
            public virtual void Execute(ref float lhs, float rhs) { }
            public virtual void Execute(ref int lhs, int rhs) { }
            public virtual void Execute(ref uint lhs, uint rhs) { }
            public virtual void Execute(ref nint lhs, nint rhs) { }
            public virtual void Execute(ref nuint lhs, nuint rhs) { }
            public virtual void Execute(ref long lhs, long rhs) { }
            public virtual void Execute(ref ulong lhs, ulong rhs) { }
            public virtual void Execute(ref short lhs, short rhs) { }
            public virtual void Execute(ref ushort lhs, ushort rhs) { }
            public virtual void Execute(ref string lhs, string rhs) { }

            public static Operator Create(OperatorType type)
            {
                switch (type)
                {
                    case OperatorType.Equals: return ComparisonOperator.Create(type);
                    case OperatorType.NotEquals: return ComparisonOperator.Create(type);
                    case OperatorType.Greater: return ComparisonOperator.Create(type);
                    case OperatorType.Less: return ComparisonOperator.Create(type);
                    case OperatorType.GreaterThanOrEqualTo: return ComparisonOperator.Create(type);
                    case OperatorType.LessThanOrEqualTo: return ComparisonOperator.Create(type);
                    case OperatorType.Move: return AssignmentOperator.Create(type);
                    case OperatorType.AddAndMove: return AdditionAssignmentOperator.Create(type);
                    default:
                        {
                            Debug.LogError("Invalid Operator!!");
                            return null;
                        }
                }
            }
        }

        public abstract class ComparisonOperator : Operator
        {
            protected ComparisonOperator(OperatorType type) : base(type)
            {

            }
            public virtual new bool Execute(ref object lhs, object rhs)
            {
                Type lhsType = lhs.GetType();
                Type rhsType = rhs.GetType();
                if (lhsType != rhsType)
                {
                    Debug.LogError($"Data types do not match. LHS: {lhsType.Name} / RHS: {rhsType.Name}");
                    return false;
                }
                if (lhsType == typeof(bool))
                {
                    var result = (bool)lhs;
                    return Execute(ref result, (bool)rhs);
                }
                else if (lhsType == typeof(byte))
                {
                    var result = (byte)lhs;
                    return Execute(ref result, (byte)rhs);
                }
                else if (lhsType == typeof(sbyte))
                {
                    var result = (sbyte)lhs;
                    return Execute(ref result, (sbyte)rhs);
                }
                else if (lhsType == typeof(char))
                {
                    var result = (char)lhs;
                    return Execute(ref result, (char)rhs);
                }
                else if (lhsType == typeof(decimal))
                {
                    var result = (decimal)lhs;
                    return Execute(ref result, (decimal)rhs);
                }
                else if (lhsType == typeof(double))
                {
                    var result = (double)lhs;
                    return Execute(ref result, (double)rhs);
                }
                else if (lhsType == typeof(float))
                {
                    var result = (float)lhs;
                    return Execute(ref result, (float)rhs);
                }
                else if (lhsType == typeof(int))
                {
                    var result = (int)lhs;
                    return Execute(ref result, (int)rhs);
                }
                else if (lhsType == typeof(uint))
                {
                    var result = (uint)lhs;
                    return Execute(ref result, (uint)rhs);
                }
                else if (lhsType == typeof(nint))
                {
                    var result = (nint)lhs;
                    return Execute(ref result, (nint)rhs);
                }
                else if (lhsType == typeof(nuint))
                {
                    var result = (nuint)lhs;
                    return Execute(ref result, (nuint)rhs);
                }
                else if (lhsType == typeof(long))
                {
                    var result = (long)lhs;
                    return Execute(ref result, (long)rhs);
                }
                else if (lhsType == typeof(ulong))
                {
                    var result = (ulong)lhs;
                    return Execute(ref result, (ulong)rhs);
                }
                else if (lhsType == typeof(short))
                {
                    var result = (short)lhs;
                    return Execute(ref result, (short)rhs);
                }
                else if (lhsType == typeof(ushort))
                {
                    var result = (ushort)lhs;
                    return Execute(ref result, (ushort)rhs);
                }
                else if (lhsType == typeof(string))
                {
                    var result = (string)lhs;
                    return Execute(ref result, (string)rhs);
                }
                return false;
            }
            public abstract new bool Execute(ref bool lhs, bool rhs);
            public abstract new bool Execute(ref byte lhs, byte rhs);
            public abstract new bool Execute(ref sbyte lhs, sbyte rhs);
            public abstract new bool Execute(ref char lhs, char rhs);
            public abstract new bool Execute(ref decimal lhs, decimal rhs);
            public abstract new bool Execute(ref double lhs, double rhs);
            public abstract new bool Execute(ref float lhs, float rhs);
            public abstract new bool Execute(ref int lhs, int rhs);
            public abstract new bool Execute(ref uint lhs, uint rhs);
            public abstract new bool Execute(ref nint lhs, nint rhs);
            public abstract new bool Execute(ref nuint lhs, nuint rhs);
            public abstract new bool Execute(ref long lhs, long rhs);
            public abstract new bool Execute(ref ulong lhs, ulong rhs);
            public abstract new bool Execute(ref short lhs, short rhs);
            public abstract new bool Execute(ref ushort lhs, ushort rhs);
            public abstract new bool Execute(ref string lhs, string rhs);

            public static new ComparisonOperator Create(OperatorType type)
            {
                switch (type)
                {
                    case OperatorType.Equals: return new EqualityOperator(type);
                    case OperatorType.NotEquals: return new InequalityOperator(type);
                    case OperatorType.Greater: return new GreaterThanOperator(type);
                    case OperatorType.Less: return new LessThanOperator(type);
                    case OperatorType.GreaterThanOrEqualTo: return new GreaterThanOrEqualOperator(type);
                    case OperatorType.LessThanOrEqualTo: return new LessThanOrEqualOperator(type);
                    default:
                        {
                            Debug.LogError("Invalid Operator!!");
                            return null;
                        }
                }
            }
        }

        public class EqualityOperator : ComparisonOperator
        {
            public EqualityOperator(OperatorType type) : base(type)
            {
            }

            public override bool Execute(ref bool lhs, bool rhs)
            {
                return lhs == rhs;
            }
            public override bool Execute(ref byte lhs, byte rhs)
            {
                return lhs == rhs;
            }

            public override bool Execute(ref sbyte lhs, sbyte rhs)
            {
                return lhs == rhs;
            }

            public override bool Execute(ref char lhs, char rhs)
            {
                return lhs == rhs;
            }

            public override bool Execute(ref decimal lhs, decimal rhs)
            {
                return lhs == rhs;
            }

            public override bool Execute(ref double lhs, double rhs)
            {
                return lhs == rhs;
            }

            public override bool Execute(ref float lhs, float rhs)
            {
                return lhs == rhs;
            }

            public override bool Execute(ref int lhs, int rhs)
            {
                return lhs == rhs;
            }

            public override bool Execute(ref uint lhs, uint rhs)
            {
                return lhs == rhs;
            }

            public override bool Execute(ref nint lhs, nint rhs)
            {
                return lhs == rhs;
            }

            public override bool Execute(ref nuint lhs, nuint rhs)
            {
                return lhs == rhs;
            }

            public override bool Execute(ref long lhs, long rhs)
            {
                return lhs == rhs;
            }

            public override bool Execute(ref ulong lhs, ulong rhs)
            {
                return lhs == rhs;
            }

            public override bool Execute(ref short lhs, short rhs)
            {
                return lhs == rhs;
            }

            public override bool Execute(ref ushort lhs, ushort rhs)
            {
                return lhs == rhs;
            }

            public override bool Execute(ref string lhs, string rhs)
            {
                return lhs == rhs;
            }
        }

        public class InequalityOperator : ComparisonOperator
        {
            public InequalityOperator(OperatorType type) : base(type)
            {
            }

            public override bool Execute(ref bool lhs, bool rhs)
            {
                return lhs != rhs;
            }

            public override bool Execute(ref byte lhs, byte rhs)
            {
                return lhs != rhs;
            }

            public override bool Execute(ref sbyte lhs, sbyte rhs)
            {
                return lhs != rhs;
            }

            public override bool Execute(ref char lhs, char rhs)
            {
                return lhs != rhs;
            }

            public override bool Execute(ref decimal lhs, decimal rhs)
            {
                return lhs != rhs;
            }

            public override bool Execute(ref double lhs, double rhs)
            {
                return lhs != rhs;
            }

            public override bool Execute(ref float lhs, float rhs)
            {
                return lhs != rhs;
            }

            public override bool Execute(ref int lhs, int rhs)
            {
                return lhs != rhs;
            }

            public override bool Execute(ref uint lhs, uint rhs)
            {
                return lhs != rhs;
            }

            public override bool Execute(ref nint lhs, nint rhs)
            {
                return lhs != rhs;
            }

            public override bool Execute(ref nuint lhs, nuint rhs)
            {
                return lhs != rhs;
            }

            public override bool Execute(ref long lhs, long rhs)
            {
                return lhs != rhs;
            }

            public override bool Execute(ref ulong lhs, ulong rhs)
            {
                return lhs != rhs;
            }

            public override bool Execute(ref short lhs, short rhs)
            {
                return lhs != rhs;
            }

            public override bool Execute(ref ushort lhs, ushort rhs)
            {
                return lhs != rhs;
            }

            public override bool Execute(ref string lhs, string rhs)
            {
                return lhs != rhs;
            }
        }

        public class LessThanOperator : ComparisonOperator
        {
            public LessThanOperator(OperatorType type) : base(type)
            {
            }

            public override bool Execute(ref bool lhs, bool rhs)
            {
                Debug.LogError($"It is a data type that cannot be operated on. Data Type: {lhs.GetType().Name} / Operator: {Type}");
                return false;
            }

            public override bool Execute(ref byte lhs, byte rhs)
            {
                return lhs < rhs;
            }

            public override bool Execute(ref sbyte lhs, sbyte rhs)
            {
                return lhs < rhs;
            }

            public override bool Execute(ref char lhs, char rhs)
            {
                return lhs < rhs;
            }

            public override bool Execute(ref decimal lhs, decimal rhs)
            {
                return lhs < rhs;
            }

            public override bool Execute(ref double lhs, double rhs)
            {
                return lhs < rhs;
            }

            public override bool Execute(ref float lhs, float rhs)
            {
                return lhs < rhs;
            }

            public override bool Execute(ref int lhs, int rhs)
            {
                return lhs < rhs;
            }

            public override bool Execute(ref uint lhs, uint rhs)
            {
                return lhs < rhs;
            }

            public override bool Execute(ref nint lhs, nint rhs)
            {
                return lhs < rhs;
            }

            public override bool Execute(ref nuint lhs, nuint rhs)
            {
                return lhs < rhs;
            }

            public override bool Execute(ref long lhs, long rhs)
            {
                return lhs < rhs;
            }

            public override bool Execute(ref ulong lhs, ulong rhs)
            {
                return lhs < rhs;
            }

            public override bool Execute(ref short lhs, short rhs)
            {
                return lhs < rhs;
            }

            public override bool Execute(ref ushort lhs, ushort rhs)
            {
                return lhs < rhs;
            }

            public override bool Execute(ref string lhs, string rhs)
            {
                Debug.LogError($"It is a data type that cannot be operated on. Data Type: {lhs.GetType().Name} / Operator: {Type}");
                return false;
            }
        }

        public class GreaterThanOperator : ComparisonOperator
        {
            public GreaterThanOperator(OperatorType type) : base(type)
            {
            }

            public override bool Execute(ref bool lhs, bool rhs)
            {
                Debug.LogError($"It is a data type that cannot be operated on. Data Type: {lhs.GetType().Name} / Operator: {Type}");
                return false;
            }

            public override bool Execute(ref byte lhs, byte rhs)
            {
                return lhs > rhs;
            }

            public override bool Execute(ref sbyte lhs, sbyte rhs)
            {
                return lhs > rhs;
            }

            public override bool Execute(ref char lhs, char rhs)
            {
                return lhs > rhs;
            }

            public override bool Execute(ref decimal lhs, decimal rhs)
            {
                return lhs > rhs;
            }

            public override bool Execute(ref double lhs, double rhs)
            {
                return lhs > rhs;
            }

            public override bool Execute(ref float lhs, float rhs)
            {
                return lhs > rhs;
            }

            public override bool Execute(ref int lhs, int rhs)
            {
                return lhs > rhs;
            }

            public override bool Execute(ref uint lhs, uint rhs)
            {
                return lhs > rhs;
            }

            public override bool Execute(ref nint lhs, nint rhs)
            {
                return lhs > rhs;
            }

            public override bool Execute(ref nuint lhs, nuint rhs)
            {
                return lhs > rhs;
            }

            public override bool Execute(ref long lhs, long rhs)
            {
                return lhs > rhs;
            }

            public override bool Execute(ref ulong lhs, ulong rhs)
            {
                return lhs > rhs;
            }

            public override bool Execute(ref short lhs, short rhs)
            {
                return lhs > rhs;
            }

            public override bool Execute(ref ushort lhs, ushort rhs)
            {
                return lhs > rhs;
            }

            public override bool Execute(ref string lhs, string rhs)
            {
                Debug.LogError($"It is a data type that cannot be operated on. Data Type: {lhs.GetType().Name} / Operator: {Type}");
                return false;
            }
        }

        public class LessThanOrEqualOperator : ComparisonOperator
        {
            public LessThanOrEqualOperator(OperatorType type) : base(type)
            {
            }

            public override bool Execute(ref bool lhs, bool rhs)
            {
                Debug.LogError($"It is a data type that cannot be operated on. Data Type: {lhs.GetType().Name} / Operator: {Type}");
                return false;
            }

            public override bool Execute(ref byte lhs, byte rhs)
            {
                return lhs <= rhs;
            }

            public override bool Execute(ref sbyte lhs, sbyte rhs)
            {
                return lhs <= rhs;
            }

            public override bool Execute(ref char lhs, char rhs)
            {
                return lhs <= rhs;
            }

            public override bool Execute(ref decimal lhs, decimal rhs)
            {
                return lhs <= rhs;
            }

            public override bool Execute(ref double lhs, double rhs)
            {
                return lhs <= rhs;
            }

            public override bool Execute(ref float lhs, float rhs)
            {
                return lhs <= rhs;
            }

            public override bool Execute(ref int lhs, int rhs)
            {
                return lhs <= rhs;
            }

            public override bool Execute(ref uint lhs, uint rhs)
            {
                return lhs <= rhs;
            }

            public override bool Execute(ref nint lhs, nint rhs)
            {
                return lhs <= rhs;
            }

            public override bool Execute(ref nuint lhs, nuint rhs)
            {
                return lhs <= rhs;
            }

            public override bool Execute(ref long lhs, long rhs)
            {
                return lhs <= rhs;
            }

            public override bool Execute(ref ulong lhs, ulong rhs)
            {
                return lhs <= rhs;
            }

            public override bool Execute(ref short lhs, short rhs)
            {
                return lhs <= rhs;
            }

            public override bool Execute(ref ushort lhs, ushort rhs)
            {
                return lhs <= rhs;
            }

            public override bool Execute(ref string lhs, string rhs)
            {
                Debug.LogError($"It is a data type that cannot be operated on. Data Type: {lhs.GetType().Name} / Operator: {Type}");
                return false;
            }
        }

        public class GreaterThanOrEqualOperator : ComparisonOperator
        {
            public GreaterThanOrEqualOperator(OperatorType type) : base(type)
            {
            }

            public override bool Execute(ref bool lhs, bool rhs)
            {
                Debug.LogError($"It is a data type that cannot be operated on. Data Type: {lhs.GetType().Name} / Operator: {Type}");
                return false;
            }

            public override bool Execute(ref byte lhs, byte rhs)
            {
                return lhs >= rhs;
            }

            public override bool Execute(ref sbyte lhs, sbyte rhs)
            {
                return lhs >= rhs;
            }

            public override bool Execute(ref char lhs, char rhs)
            {
                return lhs >= rhs;
            }

            public override bool Execute(ref decimal lhs, decimal rhs)
            {
                return lhs >= rhs;
            }

            public override bool Execute(ref double lhs, double rhs)
            {
                return lhs >= rhs;
            }

            public override bool Execute(ref float lhs, float rhs)
            {
                return lhs >= rhs;
            }

            public override bool Execute(ref int lhs, int rhs)
            {
                return lhs >= rhs;
            }

            public override bool Execute(ref uint lhs, uint rhs)
            {
                return lhs >= rhs;
            }

            public override bool Execute(ref nint lhs, nint rhs)
            {
                return lhs >= rhs;
            }

            public override bool Execute(ref nuint lhs, nuint rhs)
            {
                return lhs >= rhs;
            }

            public override bool Execute(ref long lhs, long rhs)
            {
                return lhs >= rhs;
            }

            public override bool Execute(ref ulong lhs, ulong rhs)
            {
                return lhs >= rhs;
            }

            public override bool Execute(ref short lhs, short rhs)
            {
                return lhs >= rhs;
            }

            public override bool Execute(ref ushort lhs, ushort rhs)
            {
                return lhs >= rhs;
            }

            public override bool Execute(ref string lhs, string rhs)
            {
                Debug.LogError($"It is a data type that cannot be operated on. Data Type: {lhs.GetType().Name} / Operator: {Type}");
                return false;
            }
        }

        public class AssignmentOperator : Operator
        {
            protected AssignmentOperator(OperatorType type) : base(type)
            {
            }

            public override void Execute(ref bool lhs, bool rhs)
            {
                lhs = rhs;
            }
            public override void Execute(ref byte lhs, byte rhs)
            {
                lhs = rhs;
            }
            public override void Execute(ref sbyte lhs, sbyte rhs)
            {
                lhs = rhs;
            }
            public override void Execute(ref char lhs, char rhs)
            {
                lhs = rhs;
            }
            public override void Execute(ref decimal lhs, decimal rhs)
            {
                lhs = rhs;
            }
            public override void Execute(ref double lhs, double rhs)
            {
                lhs = rhs;
            }
            public override void Execute(ref float lhs, float rhs)
            {
                lhs = rhs;
            }
            public override void Execute(ref int lhs, int rhs)
            {
                lhs = rhs;
            }
            public override void Execute(ref uint lhs, uint rhs)
            {
                lhs = rhs;
            }
            public override void Execute(ref nint lhs, nint rhs)
            {
                lhs = rhs;
            }
            public override void Execute(ref nuint lhs, nuint rhs)
            {
                lhs = rhs;
            }
            public override void Execute(ref long lhs, long rhs)
            {
                lhs = rhs;
            }
            public override void Execute(ref ulong lhs, ulong rhs)
            {
                lhs = rhs;
            }
            public override void Execute(ref short lhs, short rhs)
            {
                lhs = rhs;
            }
            public override void Execute(ref ushort lhs, ushort rhs)
            {
                lhs = rhs;
            }
            public override void Execute(ref string lhs, string rhs)
            {
                lhs = rhs;
            }

            public static new AssignmentOperator Create(OperatorType type)
            {
                return new AssignmentOperator(type);
            }
        }

        public class AdditionAssignmentOperator : Operator
        {
            protected AdditionAssignmentOperator(OperatorType type) : base(type)
            {
            }

            public override void Execute(ref bool lhs, bool rhs)
            {
                Debug.LogError($"It is a data type that cannot be operated on. Data Type: {lhs.GetType().Name} / Operator: {Type}");
            }
            public override void Execute(ref byte lhs, byte rhs)
            {
                lhs += rhs;
            }
            public override void Execute(ref sbyte lhs, sbyte rhs)
            {
                lhs += rhs;
            }
            public override void Execute(ref char lhs, char rhs)
            {
                lhs += rhs;
            }
            public override void Execute(ref decimal lhs, decimal rhs)
            {
                lhs += rhs;
            }
            public override void Execute(ref double lhs, double rhs)
            {
                lhs += rhs;
            }
            public override void Execute(ref float lhs, float rhs)
            {
                lhs += rhs;
            }
            public override void Execute(ref int lhs, int rhs)
            {
                lhs += rhs;
            }
            public override void Execute(ref uint lhs, uint rhs)
            {
                lhs += rhs;
            }
            public override void Execute(ref nint lhs, nint rhs)
            {
                lhs += rhs;
            }
            public override void Execute(ref nuint lhs, nuint rhs)
            {
                lhs += rhs;
            }
            public override void Execute(ref long lhs, long rhs)
            {
                lhs += rhs;
            }
            public override void Execute(ref ulong lhs, ulong rhs)
            {
                lhs += rhs;
            }
            public override void Execute(ref short lhs, short rhs)
            {
                lhs += rhs;
            }
            public override void Execute(ref ushort lhs, ushort rhs)
            {
                lhs += rhs;
            }
            public override void Execute(ref string lhs, string rhs)
            {
                lhs += rhs;
            }

            public static new AdditionAssignmentOperator Create(OperatorType type)
            {
                return new AdditionAssignmentOperator(type);
            }
        }
    }
}
