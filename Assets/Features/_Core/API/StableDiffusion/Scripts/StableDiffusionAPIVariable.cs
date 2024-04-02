using Features._Core.DataStructures.Variables;
using UnityEngine;

namespace Features._Core.API.StableDiffusion.Scripts
{
    [CreateAssetMenu]
    public class StableDiffusionAPIVariable : AbstractVariable<StableDiffusionAPI>
    {
        [Header("Custom Default")]
        [SerializeField] private bool _useDefaultUrl;
        [SerializeField] private string _defaultURL;
        
        protected override StableDiffusionAPI SetStoredDefault()
        {
            return _useDefaultUrl ? new StableDiffusionAPI(_defaultURL) : new StableDiffusionAPI();
        }

        private void OnValidate()
        {
            Restore();
        }
    }
}
