using System.Threading.Tasks;
using DataStructures.Variables;
using OobaboogaRuntimeIntegration.OobaboogaConfig;
using TMPro;
using UnityEngine;
using Utils;
using Debug = UnityEngine.Debug;

namespace OobaboogaRuntimeIntegration.Example
{
    public class  OobaboogaLoaderInstance : BaseAPILoaderInstance
    {
        [SerializeField] private OobaboogaAPIVariable _oobaboogaAPIVariable;
        [SerializeField] private TMP_InputField _serverUrl;
        [SerializeField] private OobaboogaModelsVariable _oobaboogaModelsVariable;

        protected void Awake()
        {
            if (!string.IsNullOrEmpty(_serverUrl.text))
            {
                _oobaboogaAPIVariable.Set(new OobaboogaAPI(_serverUrl.text));
            }
            else
            {
                _oobaboogaAPIVariable.Restore();
            }
        }

        public override async Task<bool> StartupFailed()
        {
            UpdateProgressState("Setup Text Generation API");
            return (await _oobaboogaModelsVariable.SetupAllModelsAsync()).Response.IsError;
        }

        public override async Task Initiate()
        {
            if (_oobaboogaModelsVariable.CurrentModelIndex >= _oobaboogaModelsVariable.ModelList.model_names.Count)
            {
                Debug.LogWarning("Couldn't load the selected model, because the current index is out of bounds!");
                return;
            }

            UpdateProgressState("Get Current Text Generation Model");
            string currentModel = (await _oobaboogaAPIVariable.Get().GetCurrentModelAsync()).Data.model_name;
            if (currentModel == _oobaboogaModelsVariable.ModelList.model_names[_oobaboogaModelsVariable.CurrentModelIndex])
            {
                Debug.LogWarning($"Model already loaded: {currentModel}");
                return;
            }

            UpdateProgressState("Load Text Generation Model");
            await _oobaboogaModelsVariable.LoadModelAsync();
        }
    }
}
