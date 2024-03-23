using System.Threading.Tasks;
using StableDiffusionRuntimeIntegration.SDConfig;
using TMPro;
using UnityEngine;
using Utils;
using Debug = UnityEngine.Debug;

namespace StableDiffusionRuntimeIntegration.Example
{
    public class StableDiffusionLoaderInstance : BaseAPILoaderInstance
    {
        [SerializeField] private TMP_InputField _serverUrl;
        [SerializeField] private SDModelsVariable _sdModelsVariable;
        [SerializeField] private SDSamplersVariable _sdSamplersVariable;
        
        protected void Awake()
        {
            if (!string.IsNullOrEmpty(_serverUrl.text))
            {
                Automatic1111API.SetCustomServerUrl(_serverUrl.text);
            }
        }

        public override async Task<bool> StartupFailed()
        {
            UpdateProgressState("Setup Image Generation API");
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

            UpdateProgressState("Get Current Image Generation Model");
            string currentModel = (await Automatic1111API.GetSDCheckpointSha256Async()).Data;
            if (currentModel == _sdModelsVariable.ModelList[_sdModelsVariable.CurrentModelIndex].sha256)
            {
                Debug.LogWarning($"Model already loaded: {currentModel}");
                return;
            }

            UpdateProgressState("Load Image Generation Model");
            await _sdModelsVariable.SetCurrentModelAsync();
        }
    }
}
