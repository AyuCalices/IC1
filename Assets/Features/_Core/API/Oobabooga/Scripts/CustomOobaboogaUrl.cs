using Features._Core.UI.Scripts;
using Features._Core.UI.Scripts.ButtonToggle;
using TMPro;
using UnityEngine;

namespace Features._Core.API.Oobabooga.Scripts
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
                string selectedText = (_localServerUrl.isActiveAndEnabled ? _localServerUrl.text : string.Empty).Trim().RemoveIfLastSlash();
                UpdateVariable(selectedText);
            }
            else
            {
                string selectedText = _remoteServerUrl.text.Trim().RemoveIfLastSlash();
                UpdateVariable(selectedText);
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
