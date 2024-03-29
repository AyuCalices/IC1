using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataStructures.Variables;
using UnityEditor;
using UnityEngine;
using Utils;

namespace StableDiffusionRuntimeIntegration.SDConfig
{
    [CreateAssetMenu]
    public class SDModelsVariable : ScriptableObject
    {
        [SerializeField] private StableDiffusionAPIVariable _stableDiffusionAPIVariable;
        [SerializeReference] private SDOutModel[] _modelList = Array.Empty<SDOutModel>();
        public SDOutModel[] ModelList => _modelList;
        
        [SerializeField] private int _currentModelIndex;
        public int CurrentModelIndex
        {
            get => _currentModelIndex;
            set => _currentModelIndex = value;
        }

        [ContextMenu("Get All Models")]
        public async Task<(APIResponse Response, SDOutModel[] Data)> GetAllModelsAsync()
        {
            var content = await _stableDiffusionAPIVariable.Get().GetAllModelsAsync();
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
            var currentModelSha256Content = await _stableDiffusionAPIVariable.Get().GetSDCheckpointSha256Async();
            APIResponse newestResponse = currentModelSha256Content.Response;
            if (currentModelSha256Content.Response.IsError) return (newestResponse, null);
            
            if (_modelList.All(x => x.sha256 != currentModelSha256Content.Data))
            {
                Debug.Log("The requested model is not locally available! Maybe you forgot to load all models?");
                return (newestResponse, null);
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
            await _stableDiffusionAPIVariable.Get().PostOptionsModelCheckpointAsync(_modelList[CurrentModelIndex].model_name);
            
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
        
        public async Task<APIResponse> LoadModelAsync(string modelName)
        {
            List<SDOutModel> modelList = _modelList.ToList();
            
            if (!modelList.Exists(x => x.model_name == modelName))
            {
                Debug.LogWarning("Couldn't load the model, because it doesn't exist in the current Model list. Maybe you forgot to load it first?");
            }

            _currentModelIndex = modelList.FindIndex(x => x.model_name == modelName);
            APIResponse response = await _stableDiffusionAPIVariable.Get().PostOptionsModelCheckpointAsync(modelName);
            
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif

            return response;
        }
    }
}
