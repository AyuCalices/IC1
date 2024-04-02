using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Features.ModelSelect.Scripts.ModelFetching
{
    public class ModelFetcher : MonoBehaviour
    {
        [SerializeField] private List<ModelFetcherInstance> _apiLoaderOrder;
        [SerializeField] private UnityEvent<string> _onFailed;
        
        public async void LoadModelInstances()
        {
            foreach (ModelFetcherInstance modelLoaderInstance in _apiLoaderOrder)
            {
                (bool IsValid, string ErrorMessage) result = await modelLoaderInstance.TryInitializeModelList();
                if (!result.IsValid)
                {
                    _onFailed?.Invoke(result.ErrorMessage);
                }
            }
        }
    }
}
