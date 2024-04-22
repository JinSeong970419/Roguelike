using UnityEngine;
using UnityEngine.Events;

namespace Roguelike.Core
{
    public class Variable<T> : VariableBase, ISerializationCallbackReceiver
    {
        [SerializeField] private T initialValue;
        [SerializeField] private T runtimeValue;

        private UnityEvent<T> onValueChanged = new UnityEvent<T>();
        public UnityEvent<T> OnValueChanged { get { return onValueChanged; } }

        public T Value
        {
            get { return runtimeValue; }
            set
            {
                if (!value.Equals(runtimeValue))
                {
                    onValueChanged.Invoke(value);
                }

                runtimeValue = value;
            }
        }
        public override object BoxedValue
        {
            get { return runtimeValue; }
            set
            {
                if (!value.Equals(runtimeValue))
                {
                    //Debug.Log($"Change to {value} from {runtimeValue}");
                    onValueChanged.Invoke((T)value);
                }

                runtimeValue = (T)value;
            }
        }

        public void OnBeforeSerialize()
        {

        }

        public void OnAfterDeserialize()
        {
            runtimeValue = initialValue;
        }
    }

    public abstract class VariableBase : ScriptableObject
    {
        public abstract object BoxedValue { get; set; }
    }

}


