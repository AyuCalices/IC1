using System.Threading.Tasks;
using OobaboogaRuntimeIntegration.OobaboogaConfig;
using UnityEngine;
using Utils;
using Debug = UnityEngine.Debug;

namespace OobaboogaRuntimeIntegration.Example
{
    public class OobaboogaLoaderInstance : BaseAPILoaderInstance
    {
        [SerializeField] private OobaboogaModelsVariable _oobaboogaModelsVariable;
        
        public override async Task<bool> StartupFailed()
        {
            return (await _oobaboogaModelsVariable.SetupAllModelsAsync()).Response.IsError;
        }

        public override async Task Initiate()
        {
            if (_oobaboogaModelsVariable.CurrentModelIndex >= _oobaboogaModelsVariable.ModelList.model_names.Count)
            {
                Debug.LogWarning("Couldn't load the selected model, because the current index is out of bounds!");
                return;
            }

            string currentModel = (await OobaboogaAPI.GetCurrentModelAsync()).Data.model_name;
            if (currentModel == _oobaboogaModelsVariable.ModelList.model_names[_oobaboogaModelsVariable.CurrentModelIndex])
            {
                Debug.LogWarning($"Model already loaded: {currentModel}");
                return;
            }

            Debug.Log("Load Model");
            await _oobaboogaModelsVariable.LoadModelAsync();
            Debug.Log("Load Model Complete");
        }
    }
}
