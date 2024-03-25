using Unity.VisualScripting;
using UnityEngine;
using VENTUS.DataStructures.Event;

namespace VENTUS.DataStructures.Variables
{
    public abstract class AbstractVariable<T> : ScriptableObject
    {
        [SerializeField] protected T runtimeValue;
        [SerializeField] protected T storedValue;
        [SerializeField] protected ActionEvent onValueChanged;

        private void OnEnable()
        {
            Restore();
        }

        public void Restore()
        {
            storedValue = SetStoredDefault();
            if (storedValue != null && storedValue.Equals(runtimeValue)) return;

            T bufferedPreviousValue = runtimeValue;
            runtimeValue = storedValue;
            InternalOnRestore(runtimeValue, bufferedPreviousValue);
            
            if (onValueChanged != null) onValueChanged.Raise();
        }

        protected abstract T SetStoredDefault();
        
        protected virtual void InternalOnRestore(T newValue, T previousValue) {}
        
        public bool NotNull()
        {
            return !runtimeValue.IsUnityNull();
        }

        public T Get() => runtimeValue;

        public void Set(T value)
        {
            if (value.Equals(runtimeValue)) return;

            T bufferedPreviousValue = runtimeValue;
            runtimeValue = value;
            InternalOnSet(runtimeValue, bufferedPreviousValue);
            
            if(onValueChanged != null) onValueChanged.Raise();
        }

        protected virtual void InternalOnSet(T newValue, T previousValue) {}

        public ActionEvent GetChangedEvent()
        {
            return onValueChanged;
        }

        public void Copy(AbstractVariable<T> other) => runtimeValue = other.runtimeValue;
    }
}
