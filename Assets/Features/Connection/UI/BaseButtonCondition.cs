using System;
using UnityEngine;

namespace Features.Connection.UI
{
    public abstract class BaseButtonCondition : MonoBehaviour
    {
        public event Action OnConditionUpdate;
        
        public abstract bool ButtonIsEnabled();

        protected virtual void InternalOnConditionUpdate()
        {
            OnConditionUpdate?.Invoke();
        }
    }
}
