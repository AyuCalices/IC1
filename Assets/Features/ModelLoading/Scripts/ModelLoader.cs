using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Features.ModelLoading.Scripts
{
    public class ModelLoader : MonoBehaviour
    {
        [SerializeField] private List<ModelLoaderInstance> _apiLoaderOrder;
        
        [Header("Timeout Text")]
        [SerializeField] private float _timeoutInSeconds = 100;
        [SerializeField] private GameObject _timeoutContainer;
        
        [Header("Visualization")]
        [SerializeField] private TMP_Text _loadingText;
        
        [Header("Events")]
        [SerializeField] private UnityEvent _onLoadComplete;
        [SerializeField] private UnityEvent<string> _onLoadFailed;
        
        private CancellationTokenSource _cancellationToken;
        private float _timeoutDelta;
        
        private async void OnEnable()
        {
            _loadingText.text = string.Empty;
            _timeoutContainer.SetActive(false);
            _cancellationToken = new();
        }

        private void Update()
        {
            _timeoutContainer.SetActive(_timeoutDelta > _timeoutInSeconds);
            
            _timeoutDelta += Time.deltaTime;
        }

        private void OnDisable()
        {
            _cancellationToken.Cancel();
        }

        public async void LoadModelInstances()
        {
            foreach (ModelLoaderInstance apiLoaderInstance in _apiLoaderOrder)
            {
                if (_cancellationToken.IsCancellationRequested)
                    return;

                _timeoutDelta = 0f;
                (bool IsValid, string ErrorMessage) result = await apiLoaderInstance.LoadModel(UpdateProgressState);
                if (!result.IsValid)
                {
                    _onLoadFailed?.Invoke(result.ErrorMessage);
                    return;
                }
            }
            _timeoutDelta = 0f;
            
            _onLoadComplete?.Invoke();
        }
        
        private void UpdateProgressState(string text)
        {
            Debug.Log(text);
            _loadingText.text = text;
        }
    }
}
