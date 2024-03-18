using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Utils;

namespace StableDiffusionRuntimeIntegration.SDConfig
{
    [CreateAssetMenu]
    public class SDModelsVariable : ScriptableObject
    {
        [SerializeReference] private SDOutModel[] _modelList = Array.Empty<SDOutModel>();
        public SDOutModel[] ModelList => _modelList;
        
        [SerializeField] private int _currentModelIndex;
        public int CurrentModelIndex
        {
            get => _currentModelIndex;
            set => _currentModelIndex = value;
        }

        [ContextMenu("Get All Models")]
        public async Task<(APIResponse Response, SDOutModel[] Data)> SetupAllModelsAsync()
        {
            var content = await Automatic1111API.GetModelsAsync();
            if (content.Response.IsError) return content;
            
            _modelList = content.Data;
            
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
            
            return content;
        }

        [ContextMenu("Get Current Model")]
        public async Task<(APIResponse Response, SDOutModel Data)> GetCurrentModelAsync()
        {
            var currentModelSha256Content = await Automatic1111API.GetSDCheckpointSha256Async();
            APIResponse newestResponse = currentModelSha256Content.Response;
            if (currentModelSha256Content.Response.IsError) return (newestResponse, null);
            
            if (_modelList.All(x => x.sha256 != currentModelSha256Content.Data))
            {
                Debug.Log("Setting up all models, because the requested model is not locally available!");
                var content = await SetupAllModelsAsync();
                newestResponse = content.Response;
                if (content.Response.IsError) return (newestResponse, null);
            }

            SDOutModel foundModel = null;
            for (var index = 0; index < _modelList.Length; index++)
            {
                var sdModel = _modelList[index];
                if (sdModel.sha256 == currentModelSha256Content.Data)
                {
                    foundModel = sdModel;
                    CurrentModelIndex = index;
                }
            }
            
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif

            return (newestResponse, foundModel);
        }
        
        [ContextMenu("Set Current Model")]
        public async Task SetCurrentModelAsync()
        {
            await Automatic1111API.PostOptionsModelCheckpointAsync(_modelList[CurrentModelIndex].model_name);
            
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }
}
