using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace OobaboogaRuntimeIntegration.OobaboogaConfig
{
    [CreateAssetMenu]
    public class OobaboogaModelsVariable : ScriptableObject
    {
        [SerializeField] private ModelListResponse _modelList;
        public ModelListResponse ModelList => _modelList;
        
        [SerializeField] private int _currentModelIndex;
        public int CurrentModelIndex
        {
            get => _currentModelIndex;
            set => _currentModelIndex = value;
        }
        
        private ModelInfoResponse _currentModel;
        
        [ContextMenu("Get All Models")]
        public async void SetupAllModels()
        {
            await SetupAllModelsAsync();
        }

        public async Task SetupAllModelsAsync()
        {
            _modelList = await OobaboogaAPI.GetAllModelAsync();
            _modelList.model_names.Insert(0, "None");

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        [ContextMenu("Get Current Model")]
        public async void GetCurrentModel()
        {
            await GetCurrentModelAsync();
        }
        
        public async Task GetCurrentModelAsync()
        {
            await SetupAllModelsAsync();
            
            _currentModel = await OobaboogaAPI.GetCurrentModelAsync();

            bool isContained = false;
            for (var i = 0; i < _modelList.model_names.Count; i++)
            {
                if (_currentModel.model_name == _modelList.model_names[i])
                {
                    _currentModelIndex = i;
                    isContained = true;
                }
            }

            if (!isContained)
            {
                Debug.LogWarning("The current model is not contained in the list of all models!");
            }
            
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
        
        [ContextMenu("Load Current Model")]
        public async void LoadModel()
        {
            await LoadModelAsync();
        }
        
        public async Task LoadModelAsync()
        {
            await OobaboogaAPI.LoadModelAsync(_modelList.model_names[CurrentModelIndex]);
            
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
        
        [ContextMenu("Unload Current Model")]
        public async void UnloadModel()
        {
            await UnloadModelAsync();
        }
        
        public async Task UnloadModelAsync()
        {
            if (await OobaboogaAPI.UnloadModelAsync())
            {
                _currentModelIndex = 0;
            }
            
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }
}
