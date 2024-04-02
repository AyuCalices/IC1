using Features._Core.API.StableDiffusion.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Features.BookCreation.Scripts.ImageGeneration
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
