using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Core.UI.Scripts.ButtonToggle
{
    public class ButtonToggleGroupManager : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private bool _startWithToggleGroup;
        [SerializeField] private ClickGroup _entryGroup;
        [SerializeField] private ClickGroup _toggledGroup;

        private bool _isSet;

        public bool IsToggleActive { get; private set; }

        private void Awake()
        {
            _button.onClick.AddListener(PerformToggle);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(PerformToggle);
        }

        private void Start()
        {
            if (_isSet) return;
            
            if (_startWithToggleGroup)
            {
                ActivateToggleGroup();
            }
            else
            {
                ActivateEntryGroup();
            }
        }

        public void ActivateEntryGroup()
        {
            _isSet = true;
            
            IsToggleActive = false;
            _toggledGroup.DeactivateAll(this);
            _entryGroup.ActivateAll(this);
        }

        public void ActivateToggleGroup()
        {
            _isSet = true;
            
            IsToggleActive = true;
            _entryGroup.DeactivateAll(this);
            _toggledGroup.ActivateAll(this);
        }

        public void PerformToggle()
        {
            if (IsToggleActive)
            {
                ActivateEntryGroup();
            }
            else
            {
                ActivateToggleGroup();
            }
        }


        [Serializable]
        public class ClickGroup
        {
            [SerializeField] private List<BaseButtonToggleElement> _activeObjects;

            public void ActivateAll(ButtonToggleGroupManager buttonToggleGroupManager)
            {
                foreach (BaseButtonToggleElement buttonRotationElement in _activeObjects)
                {
                    buttonRotationElement.Activate(buttonToggleGroupManager);
                }
            }
            
            public void DeactivateAll(ButtonToggleGroupManager buttonToggleGroupManager)
            {
                foreach (BaseButtonToggleElement buttonRotationElement in _activeObjects)
                {
                    buttonRotationElement.Deactivate(buttonToggleGroupManager);
                }
            }
        }
    }
}
