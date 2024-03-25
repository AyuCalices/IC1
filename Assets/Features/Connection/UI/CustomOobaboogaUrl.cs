using DataStructures.Variables;
using OobaboogaRuntimeIntegration;
using TMPro;
using UnityEngine;

namespace Features.Connection.UI
{
    public class CustomOobaboogaUrl : MonoBehaviour
    {
        [SerializeField] private OobaboogaAPIVariable _oobaboogaAPIVariable;
        [SerializeField] private BoolButtonRotationElement _isRemoteModeButtonRotationElement;
        [SerializeField] private TMP_InputField _remoteServerUrl;
        [SerializeField] private TMP_InputField _localServerUrl;
    
        public void SetURL()
        {
            string activeText = _isRemoteModeButtonRotationElement.IsActive ? _remoteServerUrl.text : _localServerUrl.text;
            
            if (!string.IsNullOrEmpty(activeText))
            {
                _oobaboogaAPIVariable.Set(new OobaboogaAPI(activeText));
            }
            else
            {
                _oobaboogaAPIVariable.Restore();
            }
        }
    }
}
