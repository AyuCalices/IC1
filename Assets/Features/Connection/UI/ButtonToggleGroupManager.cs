using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Connection.UI
{
    public class ButtonToggleGroupManager : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private bool _startWithToggleGroup;
        [SerializeField] private ClickGroup _entryGroup;
        [SerializeField] private ClickGroup _toggledGroup;

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
            if (_startWithToggleGroup)
            {
                ActivateToggleGroup();
            }
            else
            {
                ActivateEntryGroup();
            }
        }

        private void ActivateEntryGroup()
        {
            IsToggleActive = false;
            _toggledGroup.DeactivateAll(this);
            _entryGroup.ActivateAll(this);
        }

        private void ActivateToggleGroup()
        {
            IsToggleActive = true;
            _entryGroup.DeactivateAll(this);
            _toggledGroup.ActivateAll(this);
        }

        private void PerformToggle()
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
            [SerializeField] private List<BaseButtonRotationElement> _activeObjects;

            public void ActivateAll(ButtonToggleGroupManager buttonToggleGroupManager)
            {
                foreach (BaseButtonRotationElement buttonRotationElement in _activeObjects)
                {
                    buttonRotationElement.Activate(buttonToggleGroupManager);
                }
            }
            
            public void DeactivateAll(ButtonToggleGroupManager buttonToggleGroupManager)
            {
                foreach (BaseButtonRotationElement buttonRotationElement in _activeObjects)
                {
                    buttonRotationElement.Deactivate(buttonToggleGroupManager);
                }
            }
        }
    }
}
