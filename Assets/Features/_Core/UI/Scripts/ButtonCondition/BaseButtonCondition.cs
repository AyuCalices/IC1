using System;
using UnityEngine;

namespace Features._Core.UI.Scripts.ButtonCondition
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
