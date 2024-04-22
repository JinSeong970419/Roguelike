using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Roguelike.Core
{
    public class GameAction : GameActionBase
    {
        public override void Invoke()
        {
            if (methodInfo == null) return;
            methodInfo.Invoke(null, null);
        }
    }

    public class GameAction<T> : GameActionBase
    {
        [SerializeField] private T parameter;

        public override void Invoke()
        {
            if (methodInfo == null) return;
            methodInfo.Invoke(null, new object[] { parameter });
        }
    }

    public class GameAction<T1, T2> : GameActionBase
    {
        [SerializeField] private T1 parameter1;
        [SerializeField] private T2 parameter2;

        public override void Invoke()
        {
            if (methodInfo == null) return;
            methodInfo.Invoke(null, new object[] { parameter1, parameter2 });
        }
    }

    public class GameAction<T1, T2, T3> : GameActionBase
    {
        [SerializeField] private T1 parameter1;
        [SerializeField] private T2 parameter2;
        [SerializeField] private T3 parameter3;

        public override void Invoke()
        {
            if (methodInfo == null) return;
            methodInfo.Invoke(null, new object[] { parameter1, parameter2, parameter3 });
        }
    }

    public class GameAction<T1, T2, T3, T4> : GameActionBase
    {
        [SerializeField] private T1 parameter1;
        [SerializeField] private T2 parameter2;
        [SerializeField] private T3 parameter3;
        [SerializeField] private T4 parameter4;

        public override void Invoke()
        {
            if (methodInfo == null) return;
            methodInfo.Invoke(null, new object[] { parameter1, parameter2, parameter3, parameter4 });
        }
    }

    public abstract class GameActionBase : ScriptableObject
    {
        [SerializeField] protected StageLogicKind stageLogicKind;
        protected MethodInfo methodInfo;
        public StageLogicKind StageLogicKind { get { return stageLogicKind; } }

        protected virtual void OnEnable()
        {
            if (methodInfo == null)
            {
                Type classType = typeof(StageLogic);
                var methodInfos = classType.GetMethods(BindingFlags.Public | BindingFlags.Static);
                methodInfo = methodInfos.Where(o => o.Name == stageLogicKind.ToString()).FirstOrDefault();
            }
        }
        public abstract void Invoke();
    }

}
