using DataStructures.Variables;
using OobaboogaRuntimeIntegration;
using TMPro;
using UnityEngine;

namespace Features.Connection.UI
{
    public class CustomOobaboogaUrl : MonoBehaviour
    {
        [SerializeField] private OobaboogaAPIVariable _oobaboogaAPIVariable;
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
            if (!string.IsNullOrEmpty(url))
            {
                _oobaboogaAPIVariable.Set(new OobaboogaAPI(url));
            }
            else
            {
                _oobaboogaAPIVariable.Restore();
            }
        }
    }
}
