using System;
using System.Threading.Tasks;
using DataStructures.Variables;
using Features.Core.UI.Scripts.ButtonToggle;
using OobaboogaRuntimeIntegration.OobaboogaConfig;
using UnityEngine;
using Utils;

namespace OobaboogaRuntimeIntegration.Example
{
    public class OobaboogaAPILoaderInstance : BaseAPILoaderInstance
    {
        [Header("Request")]
        [SerializeField] private ButtonToggleGroupManager _isRemoteModeButtonRotationManager;
        [SerializeField] private OobaboogaAPIVariable _oobaboogaAPIVariable;
        [SerializeField] private OobaboogaModelsVariable _oobaboogaModelsVariable;

        public override bool CanStartupAPI => _isRemoteModeButtonRotationManager.IsToggleActive;
        public override string URL  => _oobaboogaAPIVariable.Get().ServerUrl;

        public override async Task<bool> TryStartup(Action<string> updateProgressMethod)
        {
            updateProgressMethod.Invoke("Setup Text Generation API ...");
            return (await _oobaboogaModelsVariable.GetAllModelsAsync()).Response.IsValid;
        }
    }
}
