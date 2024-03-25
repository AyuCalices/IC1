using System;
using System.Threading.Tasks;
using DataStructures.Variables;
using Features.Connection.UI;
using StableDiffusionRuntimeIntegration.SDConfig;
using UnityEngine;
using Utils;
using Debug = UnityEngine.Debug;

namespace StableDiffusionRuntimeIntegration.Example
{
    public class StableDiffusionLoaderInstance : BaseAPILoaderInstance
    {
        [Header("Request")]
        [SerializeField] private ButtonToggleGroupManager _isRemoteModeButtonRotationManager;
        [SerializeField] private StableDiffusionAPIVariable _stableDiffusionAPIVariable;
        [SerializeField] private SDModelsVariable _sdModelsVariable;
        [SerializeField] private SDSamplersVariable _sdSamplersVariable;

        public override bool CanStartupAPI => _isRemoteModeButtonRotationManager.IsToggleActive;
        public override string URL => _stableDiffusionAPIVariable.Get().ServerUrl;

        public override async Task<bool> TryStartup(Action<string> updateProgressMethod)
        {
            updateProgressMethod.Invoke("Setup Image Generation API ...");
            return (await _sdModelsVariable.SetupAllModelsAsync()).Response.IsValid &&
                   (await _sdSamplersVariable.SetupSampler()).Response.IsValid;
        }

        public override async Task OnStart(Action<string> updateProgressMethod)
        {
            if (_sdModelsVariable.CurrentModelIndex >= _sdModelsVariable.ModelList.Length)
            {
                Debug.LogWarning("Couldn't load the selected model, because the current index is out of bounds!");
                return;
            }

            updateProgressMethod.Invoke("Get Current Image Generation Model ...");
            string currentModel = (await _stableDiffusionAPIVariable.Get().GetSDCheckpointSha256Async()).Data;
            if (currentModel == _sdModelsVariable.ModelList[_sdModelsVariable.CurrentModelIndex].sha256)
            {
                Debug.LogWarning($"Model already loaded: {currentModel}");
                return;
            }

            updateProgressMethod.Invoke("Load Image Generation Model ...");
            await _sdModelsVariable.SetCurrentModelAsync();
        }
    }
}
