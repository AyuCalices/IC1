using DataStructures.Variables;
using StableDiffusionRuntimeIntegration;
using TMPro;
using UnityEngine;

namespace Features.Connection.UI
{
    public class CustomStableDiffusionUrl : MonoBehaviour
    {
        [SerializeField] private StableDiffusionAPIVariable _stableDiffusionAPIVariable;
        [SerializeField] private ButtonToggleGroupManager _isRemoteModeButtonRotationManager;
        [SerializeField] private TMP_InputField _remoteServerUrl;
        [SerializeField] private TMP_InputField _localServerUrl;
    
        public void SetURL()
        {
            if (_isRemoteModeButtonRotationManager.IsToggleActive)
            {
                UpdateVariable(_localServerUrl.isActiveAndEnabled ? _localServerUrl.text : string.Empty);
            }
            else
            {
                UpdateVariable(_remoteServerUrl.text);
            }
        }
        
        private void UpdateVariable(string url)
        {
            if (!string.IsNullOrEmpty(UpdateSlashOnURL(url)))
            {
                _stableDiffusionAPIVariable.Set(new StableDiffusionAPI(url));
            }
            else
            {
                _stableDiffusionAPIVariable.Restore();
            }
        }
        
        //TODO: update slash
        private string UpdateSlashOnURL(string firstString)
        {
            return !firstString.EndsWith("\\") ? firstString.Remove(firstString.Length - 1) : firstString;
        }
    }
}
