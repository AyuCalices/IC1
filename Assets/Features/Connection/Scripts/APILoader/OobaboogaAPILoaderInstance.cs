using System;
using System.Threading.Tasks;
using Features._Core.API.Oobabooga.Scripts;
using Features._Core.API.Oobabooga.Scripts.Config;
using Features._Core.UI.Scripts.ButtonToggle;
using UnityEngine;

namespace Features.Connection.Scripts.APILoader
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
