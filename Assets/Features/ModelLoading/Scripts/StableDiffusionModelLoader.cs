using System;
using System.Threading.Tasks;
using DataStructures.Variables;
using StableDiffusionRuntimeIntegration.SDConfig;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace Features.ModelLoading.Scripts
{
    public class StableDiffusionModelLoader : ModelLoaderInstance
    {
        [Header("Request")]
        [SerializeField] private TMP_Dropdown _dropdown;
        [SerializeField] private StableDiffusionAPIVariable _stableDiffusionAPIVariable;
        [SerializeField] private SDModelsVariable _sdModelsVariable;
        
        protected override async Task<APIResponse> InternalLoadModel(Action<string> updateProgressMethod)
        {
            updateProgressMethod.Invoke("Get Current Image Generation Model ...");
            (APIResponse GetCurrentModelResponse, string Data) content = await _stableDiffusionAPIVariable.Get().GetSDCheckpointSha256Async();
            if (content.GetCurrentModelResponse.IsError) return content.GetCurrentModelResponse;
            
            string currentModel = content.Data;
            if (currentModel == _dropdown.options[_dropdown.value].text)
            {
                Debug.LogWarning($"Model already loaded: {currentModel}");
                return new APIResponse()
                {
                    ResponseCode = 200, 
                    Result = UnityWebRequest.Result.Success
                };
            }

            updateProgressMethod.Invoke("Load Image Generation Model ...");
            return await _sdModelsVariable.LoadModelAsync(_dropdown.options[_dropdown.value].text);
        }
    }
}
