using Features._Core.DataStructures.Variables;
using UnityEngine;

namespace Features._Core.API.Oobabooga.Scripts
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
