using System;
using DataStructures.Variables;
using UnityEngine;
using UnityEngine.UI;

namespace StableDiffusionRuntimeIntegration.Example
{
    public class StableDiffusionInterruptGeneration : MonoBehaviour
    {
        [SerializeField] private StableDiffusionAPIVariable _stableDiffusionAPIVariable;
        [SerializeField] private Button _button;

        private void Awake()
        {
            if (!_button) return;
            
            _button.onClick.AddListener(InterruptGeneration);
        }

        private void OnDestroy()
        {
            if (!_button) return;
            
            _button.onClick.RemoveListener(InterruptGeneration);
        }

        public async void InterruptGeneration()
        {
            await _stableDiffusionAPIVariable.Get().InterruptGeneration();
        }
    }
}
