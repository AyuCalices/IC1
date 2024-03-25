using System.Collections.Generic;
using UnityEngine;

namespace Features.Connection.UI
{
    public abstract class BaseButtonRotationElement : MonoBehaviour
    {
        private readonly List<ButtonRotationManager> _disablingManagers = new ();
    
        public void Activate(ButtonRotationManager buttonRotationManager)
        {
            _disablingManagers.Remove(buttonRotationManager);
            if (_disablingManagers.Count == 0)
            {
                ActivateInternal(buttonRotationManager);
            }
        }

        protected abstract void ActivateInternal(ButtonRotationManager buttonRotationManager);
    
        public void Deactivate(ButtonRotationManager buttonRotationManager)
        {
            _disablingManagers.Add(buttonRotationManager);
            if (gameObject.activeSelf)
            {
                DeactivateInternal(buttonRotationManager);
            }
        }
        
        protected abstract void DeactivateInternal(ButtonRotationManager buttonRotationManager);
    }
}
