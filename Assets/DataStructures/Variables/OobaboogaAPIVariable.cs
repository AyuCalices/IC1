using OobaboogaRuntimeIntegration;
using UnityEngine;
using VENTUS.DataStructures.Variables;

namespace DataStructures.Variables
{
    [CreateAssetMenu]
    public class OobaboogaAPIVariable : AbstractVariable<OobaboogaAPI>
    {
        [Header("Custom Default")]
        [SerializeField] private bool _useDefaultUrl;
        [SerializeField] private string _defaultURL;
        
        protected override OobaboogaAPI SetStoredDefault()
        {
            return _useDefaultUrl ? new OobaboogaAPI(_defaultURL) : new OobaboogaAPI();
        }

        private void OnValidate()
        {
            Restore();
        }
    }
}
