using UnityEngine;

namespace Features._Core.UI.Scripts.ButtonCondition
{
    public class MethodButtonCondition : BaseButtonCondition
    {
        [SerializeField] private bool _buttonDisabledAtSpawn;
        [SerializeField] private bool _buttonDisabledAtEnabled;
        
        private bool _buttonEnabled;
        
        private void Awake()
        {
            _buttonEnabled = !_buttonDisabledAtSpawn;
        }

        private void OnEnable()
        {
            _buttonEnabled = !_buttonDisabledAtEnabled;
            
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
