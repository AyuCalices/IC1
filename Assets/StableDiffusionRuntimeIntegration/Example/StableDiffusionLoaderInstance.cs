using System.Threading.Tasks;
using StableDiffusionRuntimeIntegration.SDConfig;
using UnityEngine;
using Utils;
using Debug = UnityEngine.Debug;

namespace StableDiffusionRuntimeIntegration.Example
{
    public class StableDiffusionLoaderInstance : BaseAPILoaderInstance
    {
        [SerializeField] private SDModelsVariable _sdModelsVariable;
        [SerializeField] private SDSamplersVariable _sdSamplersVariable;

        public override async Task<bool> StartupFailed()
        {
            return (await _sdModelsVariable.SetupAllModelsAsync()).Response.IsError ||
                   (await _sdSamplersVariable.SetupSampler()).Response.IsError;
        }

        public override async Task Initiate()
        {
            if (_sdModelsVariable.CurrentModelIndex >= _sdModelsVariable.ModelList.Length)
            {
                Debug.LogWarning("Couldn't load the selected model, because the current index is out of bounds!");
                return;
            }

            string currentModel = (await Automatic1111API.GetSDCheckpointSha256Async()).Data;
            if (currentModel == _sdModelsVariable.ModelList[_sdModelsVariable.CurrentModelIndex].sha256)
            {
                Debug.LogWarning($"Model already loaded: {currentModel}");
                return;
            }

            Debug.Log("Load Model");
            await _sdModelsVariable.SetCurrentModelAsync();
            Debug.Log("Load Model Complete");
        }
    }
}
