using UnityEngine;

namespace StableDiffusionRuntimeIntegration.SDConfig
{
    public class SDInitialization : MonoBehaviour
    {
        [SerializeField] private SDModelsVariable _sdModelsVariable;

        private void Awake()
        {
            _sdModelsVariable.GetCurrentModelAsync();
        }
    }
}
