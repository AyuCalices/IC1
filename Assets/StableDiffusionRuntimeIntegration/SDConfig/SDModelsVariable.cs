using System;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task SetupAllModelsAsync()
        {
            _models = await Automatic1111API.GetModelsAsync();

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        [ContextMenu("Get Current Model")]
        public async void GetCurrentModelAsync()
        {
            await SetupAllModelsAsync();
            string currentModelSha256 = await Automatic1111API.GetSDCheckpointSha256Async();

            if (_models.All(x => x.sha256 != currentModelSha256))
            {
                _models = await Automatic1111API.GetModelsAsync();
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
            await Automatic1111API.PostOptionsModelCheckpointAsync(_models[CurrentModel].model_name);
            
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }
}
