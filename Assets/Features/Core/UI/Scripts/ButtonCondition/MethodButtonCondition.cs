using Features.Connection.UI;
using UnityEngine;

namespace Features.Core.UI.Scripts.ButtonCondition
{
    public class MethodButtonCondition : BaseButtonCondition
    {
        [SerializeField] private bool _buttonDisabledAtSpawn;
        
        private bool _buttonEnabled;
        
        private void Awake()
        {
            _buttonEnabled = !_buttonDisabledAtSpawn;
        }

        private void OnEnable()
        {
            InternalOnConditionUpdate();
        }

        private void OnDisable()
        {
            InternalOnConditionUpdate();
        }

        public void EnableButton()
        {
            _buttonEnabled = true;
            InternalOnConditionUpdate();
        }

        public void DisableButton()
        {
            _buttonEnabled = false;
            InternalOnConditionUpdate();
        }
        
        public override bool ButtonIsEnabled()
        {
            return _buttonEnabled;
        }
    }
}
