using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Connection.UI
{
    public class ConditionalButtonManager : MonoBehaviour
    {
        [SerializeField] private List<Button> _buttons;
        [SerializeField] private List<BaseButtonCondition> _baseButtonCondition;

        private void Awake()
        {
            foreach (BaseButtonCondition baseButtonCondition in _baseButtonCondition)
            {
                baseButtonCondition.OnConditionUpdate += CheckInteractable;
            }
        }
        
        private void OnEnable()
        {
            CheckInteractable();
        }

        private void OnDisable()
        {
            CheckInteractable();
        }

        private void OnDestroy()
        {
            foreach (BaseButtonCondition baseButtonCondition in _baseButtonCondition)
            {
                baseButtonCondition.OnConditionUpdate -= CheckInteractable;
            }
        }

        public void CheckInteractable()
        {
            foreach (Button button in _buttons)
            {
                button.interactable = _baseButtonCondition.All(x => !x.isActiveAndEnabled || x.ButtonIsEnabled());
            }
        }
    }
}
