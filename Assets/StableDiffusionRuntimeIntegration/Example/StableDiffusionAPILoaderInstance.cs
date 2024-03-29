using System;
using System.Threading.Tasks;
using DataStructures.Variables;
using Features.Core.UI.Scripts.ButtonToggle;
using StableDiffusionRuntimeIntegration.SDConfig;
using UnityEngine;
using Utils;

namespace StableDiffusionRuntimeIntegration.Example
{
    public class StableDiffusionAPILoaderInstance : BaseAPILoaderInstance
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
            return (await _sdModelsVariable.GetAllModelsAsync()).Response.IsValid &&
                   (await _sdSamplersVariable.SetupSampler()).Response.IsValid;
        }
    }
}
