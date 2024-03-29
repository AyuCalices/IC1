using System.Threading.Tasks;
using DataStructures.Variables;
using UnityEditor;
using UnityEngine;
using Utils;

namespace OobaboogaRuntimeIntegration.OobaboogaConfig
{
    [CreateAssetMenu]
    public class OobaboogaModelsVariable : ScriptableObject
    {
        [SerializeField] private OobaboogaAPIVariable _oobaboogaAPIVariable;
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
            await GetAllModelsAsync();
        }

        public async Task<(APIResponse Response, ModelListResponse Data)> GetAllModelsAsync()
        {
            var content = await _oobaboogaAPIVariable.Get().GetAllModelsAsync();
            if (content.Response.IsError) return (content.Response, null);
            
            _modelList = content.Data;
            _modelList.model_names.Insert(0, "None");

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif

            return content;
        }

        [ContextMenu("Get Current Model")]
        public async void GetCurrentModel()
        {
            await GetCurrentModelAsync();
        }
        
        public async Task<(APIResponse GetCurrentModelResponse, ModelInfoResponse Data)> GetCurrentModelAsync()
        {
            var content = await _oobaboogaAPIVariable.Get().GetCurrentModelAsync();
            if (content.GetCurrentModelResponse.IsError) return (content.GetCurrentModelResponse, null);
            
            _currentModel = content.Data;

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
            return content;
        }
        
        [ContextMenu("Load Current Model")]
        public async void LoadModel()
        {
            await LoadModelAsync();
        }
        
        public async Task LoadModelAsync()
        {
            if (_currentModelIndex == 0)
            {
                Debug.LogWarning("Cant load model None");
                return;
            }
            
            await _oobaboogaAPIVariable.Get().LoadModelAsync(_modelList.model_names[CurrentModelIndex]);
            
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
        
        public async Task<APIResponse> LoadModelAsync(string modelName)
        {
            if (!_modelList.model_names.Contains(modelName))
            {
                Debug.LogWarning("Couldn't load the model, because it doesn't exist in the current Model list. Maybe you forgot to load it first?");
            }

            _currentModelIndex = _modelList.model_names.FindIndex(x => x == modelName);
            APIResponse response = (await _oobaboogaAPIVariable.Get().LoadModelAsync(modelName));
            
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif

            return response;
        }
        
        [ContextMenu("Unload Current Model")]
        public async void UnloadModel()
        {
            await UnloadModelAsync();
        }
        
        public async Task UnloadModelAsync()
        {
            if (_currentModelIndex == 0)
            {
                Debug.LogWarning("Cant unload model None");
                return;
            }
            
            if ((await _oobaboogaAPIVariable.Get().UnloadModelAsync()).IsValid)
            {
                _currentModelIndex = 0;
            }
            
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }
}
