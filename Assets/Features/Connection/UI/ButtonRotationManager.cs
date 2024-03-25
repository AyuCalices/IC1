using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Connection.UI
{
    public class ButtonRotationManager : MonoBehaviour
    {
        [SerializeField] private Button _button;

        [SerializeField] private uint _startingElement;
        [SerializeField] private List<ClickGroup> _clickGroup;

        private uint _currentIndex;

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
            _currentIndex = _startingElement;
            if (_currentIndex > _clickGroup.Count)
            {
                _currentIndex = (uint)(_currentIndex % _clickGroup.Count);
            }

            for (var index = 0; index < _clickGroup.Count; index++)
            {
                foreach (var activeObject in _clickGroup[index].activeObjects)
                {
                    if (index == _currentIndex)
                    {
                        activeObject.Activate(this);
                    }
                    else
                    {
                        activeObject.Deactivate(this);
                    }
                }
            }
        }

        private void PerformToggle()
        {
            foreach (var activeObject in _clickGroup[(int)_currentIndex].activeObjects)
            {
                activeObject.Deactivate(this);
            }

            _currentIndex++;
            if (_currentIndex >= _clickGroup.Count)
            {
                _currentIndex = 0;
            }
            
            foreach (var activeObject in _clickGroup[(int)_currentIndex].activeObjects)
            {
                activeObject.Activate(this);
            }
        }


        [Serializable]
        public class ClickGroup
        {
            public List<BaseButtonRotationElement> activeObjects;
        }
    }
}
