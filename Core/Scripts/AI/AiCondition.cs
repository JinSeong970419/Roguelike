using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static Roguelike.Core.Automata;

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "AICondition", menuName = "TheSalt/AI/AICondition")]
    public class AiCondition : ScriptableObject
    {
        [SerializeField] private AiVariableType _variableType;
        [SerializeField] public OperatorType _operator;
        [SerializeField] public bool _isVariable;
        [DrawIf("_isVariable", false)]
        [SerializeField] public string _operand;
        [DrawIf("_isVariable", true)]
        [SerializeField] public VariableBase _operandVariable;
        private FieldInfo field;

        private void OnEnable()
        {
            if (field == null)
            {
                Type classType = typeof(AI);
                var fieldInfos = classType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                field = fieldInfos.Where(o => o.Name == _variableType.ToString()).FirstOrDefault();
            }
        }

        public bool GetResult(AI ai)
        {
            object lhs = field.GetValue(ai);
            var type = lhs.GetType();
            
            object rhs = _isVariable ? _operandVariable.BoxedValue : type.Parse(_operand);
            if (rhs == null) return false;

            ComparisonOperator operatorEx = ComparisonOperator.Create(_operator);
            return operatorEx.Execute(ref lhs, rhs);
        }
    }
}
