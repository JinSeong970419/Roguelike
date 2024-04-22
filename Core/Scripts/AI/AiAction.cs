using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Roguelike.Core
{
    public class AiAction : AiActionBase
    {
        public override void Invoke(AI ai)
        {
            if (methodInfo == null) return;
            methodInfo.Invoke(ai, null);
        }
    }

    public class AiAction<T> : AiActionBase
    {
        [SerializeField] private T parameter;

        public override void Invoke(AI ai)
        {
            if (methodInfo == null) return;
            methodInfo.Invoke(ai, new object[] { parameter });
        }
    }

    public class AiAction<T1, T2> : AiActionBase
    {
        [SerializeField] private T1 parameter1;
        [SerializeField] private T2 parameter2;

        public override void Invoke(AI ai)
        {
            if (methodInfo == null) return;
            methodInfo.Invoke(ai, new object[] { parameter1, parameter2 });
        }
    }

    public class AiAction<T1, T2, T3> : AiActionBase
    {
        [SerializeField] private T1 parameter1;
        [SerializeField] private T2 parameter2;
        [SerializeField] private T3 parameter3;

        public override void Invoke(AI ai)
        {
            if (methodInfo == null) return;
            methodInfo.Invoke(ai, new object[] { parameter1, parameter2, parameter3 });
        }
    }

    public class AiAction<T1, T2, T3, T4> : AiActionBase
    {
        [SerializeField] private T1 parameter1;
        [SerializeField] private T2 parameter2;
        [SerializeField] private T3 parameter3;
        [SerializeField] private T4 parameter4;

        public override void Invoke(AI ai)
        {
            if (methodInfo == null) return;
            methodInfo.Invoke(ai, new object[] { parameter1, parameter2, parameter3, parameter4 });
        }
    }

    public abstract class AiActionBase : ScriptableObject
    {
        [SerializeField] protected AiLogicType aiLogicType;
        protected MethodInfo methodInfo;
        public AiLogicType AiLogicType { get { return aiLogicType; } }

        protected virtual void OnEnable()
        {
            if (methodInfo == null)
            {
                Type classType = typeof(AI);
                var methodInfos = classType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                methodInfo = methodInfos.Where(o => o.Name == aiLogicType.ToString()).FirstOrDefault();
            }
        }
        public abstract void Invoke(AI ai);
    }

}
