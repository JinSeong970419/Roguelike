using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Roguelike.Core
{
    public partial class Automata
    {
        public enum DataType
        {
            Bool,
            Int,
            Float,
            Timer,
        }

        [System.Serializable]
        public class Parameter
        {
            [SerializeField] public DataType _type;
            [SerializeField] public string _name;
            [SerializeField] public string _value;

            public DataType DataType { get { return _type; } }
            public string Name { get { return _name; } }
            public object Value
            {
                get
                {
                    return _type.Parse(_value);
                }
                set
                {
                    _value = value.ToString();
                }
            }
        }

        [System.Serializable]
        public class Condition
        {
            [SerializeField] public VariableBase _param;
            [SerializeField] public OperatorType _operator;
            [SerializeField] public bool _isVariable;
            [DrawIf("_isVariable", false)]
            [SerializeField] public string _operand;
            [DrawIf("_isVariable", true)]
            [SerializeField] public VariableBase _operandVariable;

            public VariableBase ParameterKind { get { return _param; } }
            public OperatorType Operator { get { return _operator; } }
            public bool IsVariable { get { return _isVariable; } }
            public string Operand { get { return _operand; } }
            public VariableBase OperandVariable { get { return _operandVariable; } }
            public bool Result
            {
                get
                {
                    var param = _param;
                    var type = param.BoxedValue.GetType();

                    object lhs = param.BoxedValue;
                    if (lhs == null) return false;

                    object rhs = _isVariable ? _operandVariable.BoxedValue : type.Parse(_operand);
                    if (rhs == null) return false;

                    ComparisonOperator operatorEx = ComparisonOperator.Create(_operator);
                    return operatorEx.Execute(ref lhs, rhs);
                }
            }
        }

        [System.Serializable]
        public class Operation
        {
            [SerializeField] public VariableBase _param;
            [SerializeField] public OperatorType _operator;
            [SerializeField] public bool _isVariable;
            [DrawIf("_isVariable", false)]
            [SerializeField] public string _operand;
            [DrawIf("_isVariable", true)]
            [SerializeField] public VariableBase _operandVariable;

            public VariableBase ParameterKind { get { return _param; } }
            public OperatorType OperatorType { get { return _operator; } }
            public bool IsVariable { get { return _isVariable; } }
            public string Operand { get { return _operand; } }
            public VariableBase OperandVariable { get { return _operandVariable; } }
            public void Execute()
            {
                var param = _param;
                var type = param.BoxedValue.GetType();

                object lhs = param.BoxedValue;
                if (lhs == null) return;

                object rhs = _isVariable ? _operandVariable.BoxedValue : type.Parse(_operand);
                if (rhs == null) return;

                Operator operatorEx = Operator.Create(_operator);
                operatorEx.Execute(ref lhs, rhs);
                param.BoxedValue = lhs;
                return;
            }
        }


        [System.Serializable]
        public class ConditionalBehavior
        {
            [SerializeField] private string _name;
            [SerializeField] private string _description;
            [Header("Conditional Actions")]
            [SerializeField] private List<Condition> _conditions;
            [SerializeField] private List<Operation> _conditionalOperation;
            [SerializeField] private List<UnityEvent> _conditionalActions;

            public string Name { get { return _name; } }
            public string Description { get { return _description; } }
            public List<Condition> Conditions { get { return _conditions; } }
            public List<Operation> ConditionalOperation { get { return _conditionalOperation; } }
            public List<UnityEvent> ConditionalActions { get { return _conditionalActions; } }

            public void Execute()
            {
                int conditionCount = _conditions.Count;
                for (int i = 0; i < conditionCount; i++)
                {
                    var condition = _conditions[i];
                    if (condition.Result == false) return;
                }

                int operationCount = _conditionalOperation.Count;
                for (int i = 0; i < operationCount; i++)
                {
                    var operation = _conditionalOperation[i];
                    operation.Execute();
                }

                int actionCount = _conditionalActions.Count;
                for (int i = 0; i < actionCount; i++)
                {
                    var action = _conditionalActions[i];
                    action.Invoke();
                }
            }
        }
    }
}
