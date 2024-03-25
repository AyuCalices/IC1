using System;
using System.Threading.Tasks;
using DataStructures.Variables;
using Features.Connection.UI;
using OobaboogaRuntimeIntegration.OobaboogaConfig;
using UnityEngine;
using Utils;
using Debug = UnityEngine.Debug;

namespace OobaboogaRuntimeIntegration.Example
{
    public class OobaboogaLoaderInstance : BaseAPILoaderInstance
    {
        [Header("Request")]
        [SerializeField] private BoolButtonRotationElement _isRemoteModeButtonRotationElement;
        [SerializeField] private OobaboogaAPIVariable _oobaboogaAPIVariable;
        [SerializeField] private OobaboogaModelsVariable _oobaboogaModelsVariable;

        public override bool CanStartupAPI => !_isRemoteModeButtonRotationElement.IsActive;

        public override async Task<bool> TryStartup(Action<string> updateProgressMethod)
        {
            updateProgressMethod.Invoke("Setup Text Generation API");
            return (await _oobaboogaModelsVariable.SetupAllModelsAsync()).Response.IsValid;
        }

        public override async Task OnStart(Action<string> updateProgressMethod)
        {
            if (_oobaboogaModelsVariable.CurrentModelIndex >= _oobaboogaModelsVariable.ModelList.model_names.Count)
            {
                Debug.LogWarning("Couldn't load the selected model, because the current index is out of bounds!");
                return;
            }

            updateProgressMethod.Invoke("Get Current Text Generation Model");
            string currentModel = (await _oobaboogaAPIVariable.Get().GetCurrentModelAsync()).Data.model_name;
            if (currentModel == _oobaboogaModelsVariable.ModelList.model_names[_oobaboogaModelsVariable.CurrentModelIndex])
            {
                Debug.LogWarning($"Model already loaded: {currentModel}");
                return;
            }

            updateProgressMethod.Invoke("Load Text Generation Model");
            await _oobaboogaModelsVariable.LoadModelAsync();
        }
    }
}
