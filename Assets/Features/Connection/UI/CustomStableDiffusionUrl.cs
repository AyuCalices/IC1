using DataStructures.Variables;
using StableDiffusionRuntimeIntegration;
using TMPro;
using UnityEngine;

namespace Features.Connection.UI
{
    public class CustomStableDiffusionUrl : MonoBehaviour
    {
        [SerializeField] private StableDiffusionAPIVariable _stableDiffusionAPIVariable;
        [SerializeField] private BoolButtonRotationElement _isRemoteModeButtonRotationElement;
        [SerializeField] private TMP_InputField _remoteServerUrl;
        [SerializeField] private TMP_InputField _localServerUrl;
    
        public void SetURL()
        {
            string activeText = _isRemoteModeButtonRotationElement.IsActive ? _remoteServerUrl.text : _localServerUrl.text;
            
            if (!string.IsNullOrEmpty(activeText))
            {
                _stableDiffusionAPIVariable.Set(new StableDiffusionAPI(activeText));
            }
            else
            {
                _stableDiffusionAPIVariable.Restore();
            }
        }
    }
}
