using System.Collections.Generic;
using UnityEngine;

namespace Features.Connection.UI
{
    public abstract class BaseButtonRotationElement : MonoBehaviour
    {
        private readonly List<ButtonToggleGroupManager> _disablingManagers = new ();
    
        public void Activate(ButtonToggleGroupManager buttonToggleGroupManager)
        {
            _disablingManagers.Remove(buttonToggleGroupManager);
            if (_disablingManagers.Count == 0)
            {
                ActivateInternal(buttonToggleGroupManager);
            }
        }

        protected abstract void ActivateInternal(ButtonToggleGroupManager buttonToggleGroupManager);
    
        public void Deactivate(ButtonToggleGroupManager buttonToggleGroupManager)
        {
            _disablingManagers.Add(buttonToggleGroupManager);
            if (gameObject.activeSelf)
            {
                DeactivateInternal(buttonToggleGroupManager);
            }
        }
        
        protected abstract void DeactivateInternal(ButtonToggleGroupManager buttonToggleGroupManager);
    }
}
