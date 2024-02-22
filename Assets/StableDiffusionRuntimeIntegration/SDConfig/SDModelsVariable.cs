using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace StableDiffusionRuntimeIntegration.SDConfig
{
    [CreateAssetMenu]
    public class SDModelsVariable : ScriptableObject
    {
        [SerializeReference] private SDOutModel[] _models = Array.Empty<SDOutModel>();
        public SDOutModel[] Models => _models;
        
        [SerializeField] private int _currentModel;
        public int CurrentModel
        {
            get => _currentModel;
            set => _currentModel = value;
        }

        [ContextMenu("Get All Models")]
        public async void SetupAllModelsAsync()
        {
            _models = await StableDiffusionRequests.GetModelsAsync();

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        [ContextMenu("Get Current Model")]
        public async void GetCurrentModelAsync()
        {
            if (_models == null || _models.Length == 0)
            {
                _models = await StableDiffusionRequests.GetModelsAsync();
            }
            
            string currentModelSha256 = await StableDiffusionRequests.GetSDCheckpointSha256Async();

            if (_models.All(x => x.sha256 != currentModelSha256))
            {
                _models = await StableDiffusionRequests.GetModelsAsync();
            }

            for (var index = 0; index < _models.Length; index++)
            {
                var sdModel = _models[index];
                if (sdModel.sha256 == currentModelSha256)
                {
                    CurrentModel = index;
                }
            }
            
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
        
        [ContextMenu("Set Current Model")]
        public async void SetCurrentModelAsync()
        {
            await StableDiffusionRequests.PostOptionsModelCheckpointAsync(_models[CurrentModel].model_name);
            
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }
}
